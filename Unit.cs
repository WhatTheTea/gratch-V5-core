using System;
using System.Collections.Generic;

namespace gratch_core
{
    /// <summary>
    /// Класс, что представляет собой одного человека в графике
    /// </summary>
    internal class Unit
    {
        //Fields
        private string name;
        private List<DateTime> dutyDates;
        //Properties
        private DateTime now = DateTime.Now;
        private bool IsEnabled => dutyDates != null;
        public string Name
        {
            get => name;
            set
            {
                if (value != null && value.Length > 2) name = value;
                else throw new ArgumentException("Value can not be less than 2 symbols or null", nameof(Name));
            }
        }
        public List<DateTime> DutyDates
        {
            get => dutyDates;
            internal set => dutyDates = value;
        }

        //Constructors
        public Unit() { }
        /// <param name="Name">Имя елемента</param>
        public Unit(string Name)
        {
            if (Name != null && Name.Length > 2) name = Name;
            else throw new ArgumentException("Value can not be less than 2 symbols or null", nameof(Name));
        }
        //Methods
        public void AddDutyDate(DateTime date)
        {
            if (date.Month == now.Month)
            {
                dutyDates.Add(date);
            }
        }
        public void RemoveDutyDate(DateTime date)
        {
            if (dutyDates.Contains(date))
            {
                dutyDates.Remove(date);
            }
            else throw new ArgumentException("Cannot remove non-existing item", nameof(date));
        }
        public void Penalty(int days)
        {
            dutyDates.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            for(int i = 0; i < days; i++)
            {
                var nextdate = dutyDates[dutyDates.Count].AddDays(1);
                if (nextdate.Month == now.Month)
                {
                    AddDutyDate(nextdate);
                }
                else throw new ArgumentOutOfRangeException(nameof(nextdate), "DutyDate cant be assigned out of bounds of this month");
            }
        }
        public void SwapDutyDates(Unit SwapWith)
        {
            List<DateTime> buffer = SwapWith.DutyDates;
            SwapWith.DutyDates = dutyDates;
            dutyDates = buffer;
        }
    }
}
