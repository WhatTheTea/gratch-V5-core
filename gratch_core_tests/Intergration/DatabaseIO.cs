using gratch_core;
using gratch_core.Models;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

namespace gratch_core_tests.Integration
{
    [TestFixture]
    public class DatabaseIO
    {
        
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(40)]
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
        
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(30)]
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
        
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 40)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        [TestCase(2, 40)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 40)]
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
        [TearDown]
        public void CleanUp()
        {
            DataFiller.CleanUp();
        }
    }
}