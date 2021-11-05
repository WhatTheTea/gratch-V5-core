using System.Collections.Generic;

namespace gratch_core
{
    public interface IGroup : IList<Person>
    {
        Graph Graph { get; }
        string Name { get; set; }

        /*protected static readonly List<IGroup> instances = new List<IGroup>(); //кыш
        internal static IList<IGroup> AllInstances
        {
            get
            {
                return instances.Where(instance => instance.Count > 0).Distinct().ToList().AsReadOnly();
            }
        }*/
    }
}