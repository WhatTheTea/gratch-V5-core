using System;
using System.Collections.Generic;

namespace gratch_core
{
    /// <summary>
    /// Класс, что представляет собой одного человека в графике
    /// </summary>
    public class Unit
    {
        //Constants

        //Fields
        private string name;
        private Group group;
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
                else throw new ArgumentException("Value can not be less than 2 symbols or null", nameof(value));
            }
        }
        public List<DateTime> DutyDates
        {
            get => dutyDates;
            internal set => dutyDates = value;
        }
        internal Group Group { get => group; set => group = value; }

        //Constructors
        public Unit()
        {
        }
        /// <param name="name">Имя елемента</param>
        public Unit(string name)
        {
            Name = name;
        }
        //Methods
        /// <summary>
        /// Добавляет дату дежурства в список в границах этого месяца.
        /// </summary>
        /// <param name="date">Дата дежурства, которую надо добавить в список дат дежурства человека</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        internal void AddDutyDate(DateTime date)
        {
            if (date.Month == now.Month)
            {
                if (!group.Workweek.IsHoliday(date))
                {
                    dutyDates.Add(date);
                }
                else throw new ArgumentException("DutyDate cant be assigned to holiday", nameof(date));
            }
            else throw new ArgumentOutOfRangeException(nameof(date), "DutyDate cant be assigned out of bounds of this month");
        }
        /// <summary>
        /// Убирает дату дежурства из списка дат.
        /// </summary>
        /// <param name="date">Дата дежурства</param>
        /// <exception cref="ArgumentException"/>
        internal void RemoveDutyDate(DateTime date)
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
        internal void RemoveAllDutyDates()
        {
            dutyDates.Clear();
        }
        /// <summary>
        /// Выдача штрафа человеку в днях.
        /// </summary>
        /// <param name="days">Количество штрафных дней</param>
        internal void Penalty(int days)
        {
            dutyDates.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            for (int i = 0; i < days; i++)
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
        /// <summary>
        /// Взаимозаменяет даты дежурств данного <see cref="Unit"/> и <paramref name="SwapWith"/>.
        /// </summary>
        /// <param name="SwapWith">Определяет с каким <see cref="Unit"/> обменятся датами дежурств</param>
        internal void SwapDutyDates(Unit SwapWith)
        {
            List<DateTime> buffer = SwapWith.DutyDates;
            SwapWith.DutyDates = dutyDates;
            dutyDates = buffer;
        }
    }
}
