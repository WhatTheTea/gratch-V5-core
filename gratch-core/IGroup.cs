using System.Collections.Generic;

namespace gratch_core
{
    public interface IGroup : IList<Person>
    {
        Graph Graph { get; }
        string Name { get; set; }

    }
}