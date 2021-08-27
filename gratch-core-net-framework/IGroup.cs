using System.Collections.Generic;
using System.Linq;

namespace gratch_core
{
    public interface IGroup : IList<Person>
    {
        Graph Graph { get; }
        string Name { get; set; }

        protected static readonly List<IGroup> instances = new List<IGroup>();
        internal static IList<IGroup> AllInstances
        {
            get
            {
                return instances.Where(instance => instance.Count > 0).Distinct().ToList().AsReadOnly();
            }
        }
    }
}