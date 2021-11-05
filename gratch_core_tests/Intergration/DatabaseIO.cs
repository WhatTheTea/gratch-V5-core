using gratch_core;
using gratch_core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace gratch_core_tests.Integration
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class DatabaseIO
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(40)]
        public void Repository_BasicReadWrite_GroupEqualsGroupModel(int pCount)
        {
            var group = DataFiller.GetGroup(pCount);
            var expected = group[0];

            var actual = DataFiller.Repository?.GetGroup(group.Name)?.People?[0];

            Assert.IsTrue(expected?.Name == actual?.Name);
            Assert.IsTrue(expected?.DutyDates?.Count == actual?.ToPerson()?.DutyDates?.Count);
            for (int i = 0; i < expected?.DutyDates?.Count; i++)
            {
                Assert.AreEqual(expected?.DutyDates?[i], actual?.ToPerson()?.DutyDates?[i]);
            }

            //DataFiller.ReturnTable();
        }
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(30)]
        public void Repository_LoadAllGroups_GroupsAreLoaded(int pCount)
        {
            var group = DataFiller.GetGroup(pCount);

            List<IGroup> groups = new();

            /*Group.subscriber.Dispose();
            foreach (var grp in IGroup.AllInstances) grp.Clear();
            Group.subscriber = SQLiteSubscriber.GetSubscriber();*/

            groups = DataFiller.Repository.LoadAllGroups();

            groups.ForEach(grp => Assert.IsTrue(grp.Any()));
            groups.ForEach(grp =>
            {
                foreach (var p in grp)
                {
                    Assert.IsTrue(p.Name != null);
                    Assert.IsTrue(p.DutyDates.Any());
                }
            });
        }
        [DataTestMethod]
        [DataRow(1, 0)]
        [DataRow(1, 1)]
        [DataRow(1, 40)]
        [DataRow(2, 0)]
        [DataRow(2, 1)]
        [DataRow(2, 40)]
        [DataRow(3, 0)]
        [DataRow(3, 1)]
        [DataRow(3, 40)]
        public void Repository_SeveralGroups_ModelsShouldntBeCorrupted(int gCount, int pCount)
        {
            var gList = new List<Group>();
            var mList = new List<GroupModel>();

            for (int g = 0; g < gCount; g++)
            {
                if (gCount == 0) break;
                gList.Add(DataFiller.GetGroup(pCount));
                mList.Add(DataFiller.Repository.GetGroup(gList[g].Name));
            }

            if (gList.Count == mList.Count)
            {
                if (new GroupRepository().GetAllGroups().Count != 0)
                {
                    mList.ForEach(gModel => gModel.People.ForEach(pModel =>
                    {
                        Assert.AreEqual(gModel.Id, pModel.GroupId);
                        Assert.AreEqual(gList[mList.IndexOf(gModel)]
                            [gModel.People.IndexOf(pModel)].Name, pModel.Name);//pModel.Name == Person.Name
                    }));
                    //DataFiller.ReturnTable();
                }
            }
            else
            {
                Assert.Fail("Models or groups are corrupted");
            }
        }
        [TestCleanup]
        public void CleanUp()
        {
            DataFiller.Repository.DeleteAll();
            /*Group.subscriber.Dispose();
            foreach (var grp in IGroup.AllInstances) grp.Clear();
            Group.subscriber = SQLiteSubscriber.GetSubscriber();*/
        }
    }
}