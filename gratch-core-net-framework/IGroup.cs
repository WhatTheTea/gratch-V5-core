using System.Collections.Generic;
using System.Linq;

namespace gratch_core
{
    public interface IGroup : IList<Person>
    {
        Graph Graph { get; }
        string Name { get; set; }
    }
}