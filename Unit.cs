using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    /// <summary>
    /// Класс, что представляет собой одного человека в графике
    /// </summary>
    internal class Unit
    {
        private string name;
        private DateTime[] dutyDates;
        private bool isEnabled;

        /// <param name="Name">Имя елемента</param>
        public Unit(string Name) : this(Name,null)
        {

        }
        /// <param name="Name">Имя елемента</param>
        /// <param name="DutyDates">Дата или несколько дат, когда елемент дежурит</param>
        public Unit(string Name, params DateTime[] DutyDates)
        {
            name = Name;
            dutyDates = DutyDates;
            isEnabled = dutyDates != null;
        }
    }
}
