using System;
using System.Collections.Generic;

namespace gratch_core
{
    /// <summary>
    /// Класс, что представляет собой одного человека в графике
    /// </summary>
    internal class Unit
    {
        //Constants
        private readonly Group group;
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
        public Unit() {
        }
        /// <param name="Name">Имя елемента</param>
        public Unit(string Name)
        {
            if (Name != null && Name.Length > 2) name = Name;
            else throw new ArgumentException("Value can not be less than 2 symbols or null", nameof(Name));
        }
        //Methods
        /// <summary>
        /// Добавляет дату дежурства в список в границах этого месяца.
        /// </summary>
        /// <param name="date">Дата дежурства, которую надо добавить в список дат дежурства человека</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public void AddDutyDate(DateTime date)
        {
            if (date.Month == now.Month)
            {
                if (!group.Workweek.IsHoliday(date))
                {
                    dutyDates.Add(date);
                }
                else throw new ArgumentException(nameof(date), "DutyDate cant be assigned to holiday");
            }
            else throw new ArgumentOutOfRangeException(nameof(date), "DutyDate cant be assigned out of bounds of this month");
        }
        /// <summary>
        /// Убирает дату дежурства из списка дат.
        /// </summary>
        /// <param name="date">Дата дежурства</param>
        /// <exception cref="ArgumentException"/>
        public void RemoveDutyDate(DateTime date)
        {
            if (dutyDates.Contains(date))
            {
                dutyDates.Remove(date);
            }
            else throw new ArgumentException("Cannot remove non-existing item", nameof(date));
        }
        /// <summary>
        /// Чистит список полностью
        /// </summary>
        public void RemoveAllDutyDates()
        {
            dutyDates.Clear();
        }
        /// <summary>
        /// Выдача штрафа человеку в днях.
        /// </summary>
        /// <param name="days">Количество штрафных дней</param>
        public void Penalty(int days)
        {
            dutyDates.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            for(int i = 0; i < days; i++)
            {
                var nextdate = dutyDates[dutyDates.Count].AddDays(1);
                try
                {
                    AddDutyDate(nextdate);
                }
                catch (ArgumentException)
                {
                    AddDutyDate(nextdate.AddDays(1));
                }
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
