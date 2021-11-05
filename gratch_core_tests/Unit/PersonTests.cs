using NUnit.Framework;

namespace gratch_core_tests.Unit
{
    [TestFixture]
    public class PersonTests
    {
        [Test]
        public void Rename_UnSuccessful_PersonIsNotRenamed()
        {
            var group = DataFiller.GetGroup(20);
            var name = group[0].Name;

            group.RenamePerson(4,name);

            Assert.AreNotEqual(name, group[4]);
        }
        [Test]
        public void Rename_Succesful_PersonRenamed()
        {
            var group = DataFiller.GetGroup(20);
            var name = "Gosha";

            group.RenamePerson(0,name);

            Assert.AreEqual(name, group[0].Name);
        }
        [TearDown]
        public void CleanUp()
        {
            DataFiller.CleanUp();
        }
    }
}
