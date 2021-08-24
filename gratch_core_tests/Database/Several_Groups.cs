
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gratch_core_tests.Database
{
    [TestClass]
    public class Several_Groups
    {
        [TestMethod]
        public void TwoGroups()
        {
            var group1 = DataFiller.GetGroup(10);
            var group2 = DataFiller.GetGroup(20);

            var model1 = DataFiller.Repository.GetGroup(group1.Name);
            var model2 = DataFiller.Repository.GetGroup(group2.Name);

            model1.People.ForEach(mod =>
            {
                Assert.AreEqual(model1.Name, mod.GroupModel.Name);
                Assert.AreEqual(model1.Id, mod.GroupId);
                Assert.AreEqual(group1[model1.People.IndexOf(mod)].Name, mod.Name);
            });
            model2.People.ForEach(mod =>
            {
                Assert.AreEqual(model2.Name, mod.GroupModel.Name);
                Assert.AreEqual(model2.Id, mod.GroupId);
                Assert.AreEqual(group2[model2.People.IndexOf(mod)].Name, mod.Name);
            });


        }
        [TestMethod]
        public void ThreeGroups()
        {
            var group1 = DataFiller.GetGroup(10);
            var group2 = DataFiller.GetGroup(20);
            var group3 = DataFiller.GetGroup(30);

            var model1 = DataFiller.Repository.GetGroup(group1.Name);
            var model2 = DataFiller.Repository.GetGroup(group2.Name);
            var model3 = DataFiller.Repository.GetGroup(group3.Name);

            model1.People.ForEach(mod =>
            {
                Assert.AreEqual(model1.Name, mod.GroupModel.Name);
                Assert.AreEqual(model1.Id, mod.GroupId);
                Assert.AreEqual(group1[model1.People.IndexOf(mod)].Name, mod.Name);
            });
            model2.People.ForEach(mod =>
            {
                Assert.AreEqual(model2.Name, mod.GroupModel.Name);
                Assert.AreEqual(model2.Id, mod.GroupId);
                Assert.AreEqual(group2[model2.People.IndexOf(mod)].Name, mod.Name);
            });
            model3.People.ForEach(mod =>
            {
                Assert.AreEqual(model3.Name, mod.GroupModel.Name);
                Assert.AreEqual(model3.Id, mod.GroupId);
                Assert.AreEqual(group3[model3.People.IndexOf(mod)].Name, mod.Name);
            });
        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
