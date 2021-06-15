using System;
using System.Collections.Generic;

namespace gratch_core
{
    /// <summary>
    /// Класс, что представляет собой одного человека в графике
    /// </summary>
    internal class Unit
    {
        private string name;
        private List<DateTime> dutyDates;
        private bool IsEnabled => dutyDates != null;
        public string Name
        {
            get => name;
            set
            {
                if (value != null && value.Length > 2) name = value;
            }
        }
        public List<DateTime> DutyDates
        {
            get => dutyDates; 
            set { dutyDates = value; }
        }

        public Unit() { }
        /// <param name="Name">Имя елемента</param>
        public Unit(string Name)
        {
            if (Name != null && Name.Length > 2) name = Name; 
            else throw new ArgumentException("Value can not be less than 2 symbols or null",nameof(Name));
        }
    }
}
