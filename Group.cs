using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class Group
    {
        public List<Person> People { get; set; } = new();
        public List<DayOfWeek> weekend { get; set; } = new();
        public Group()
        {
            
        }
    }
}
