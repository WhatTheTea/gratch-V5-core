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
        private int id = 0;
        private string name;
        private DateTime[] dutyDates;
        private bool isEnabled;
        /// <param name="Name">Имя елемента</param>
        /// <param name="IsEnabled">Включение елемента в график</param>
        /// <param name="DutyDates">Дата или несколько дат, когда елемент дежурит</param>
        public Unit(string Name, bool IsEnabled = true, params DateTime[] DutyDates)
        {
            id++;
            name = Name;
            isEnabled = IsEnabled;
            dutyDates = DutyDates;
        }
    }
}
