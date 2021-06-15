using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    /// <summary>
    /// Класс для работы с рабочей неделей.
    /// Выходных должно быть не больше 6.
    /// </summary>
    internal class Workweek
    {
        //Constants
        private const int daysInWeek = 7;
        //Fields
        /// <summary>
        /// <see cref="local"/> - файл с локализацией. local.GetString(DayOfWeek.*);
        /// </summary>
        private readonly ResourceManager local = new("dayofweek-ru", // TODO: полноценные переводы
            typeof(Group).Assembly);
        private Dictionary<DayOfWeek, bool> week;
        //Properties
        public int CountHolidays
        {
            get {
                int counter = 0;
                foreach(DayOfWeek day in week.Keys)
                {
                    if (week[day] == true) counter++;
                }
                return counter;
            }
        }
        public int CountWorkDays => daysInWeek - CountHolidays;
        public Dictionary<DayOfWeek, bool> Week => week;
        //Constructors
        /// <summary>
        /// Конструктор стандартной рабочей недели с субботой и воскресеньем в качетсве выходных дней
        /// </summary>
        public Workweek() : this(DayOfWeek.Saturday, DayOfWeek.Sunday)
        {

        }
        /// <summary>
        /// Конструктор рабочей недели с пользовательскими выходными
        /// </summary>
        /// <param name="weekend">Набор пользовательских выходных</param>
        public Workweek(params DayOfWeek[] weekend)
        {
            //Ошибка когда вся неделя выходная
            if (weekend.Length > 6) throw new ArgumentOutOfRangeException(nameof(weekend), "Слишком много выходных");
            //Инициализация словаря День - Логика
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek))) week.Add(day, false);
            //Заполнение данными словаря
            foreach (DayOfWeek day in weekend) week[day] = true;
        }
        //Methods
        /// <summary>
        /// Проверяет выходной ли <paramref name="day"/>
        /// </summary>
        /// <param name="day">День недели</param>
        /// <returns>
        /// <c>true</c> если <paramref name="day"/> выходной, иначе - <c>false</c>
        /// </returns>
        public bool IsHoliday(DayOfWeek day) => week[day];
        public string DayToString(DayOfWeek day) => local.GetString(day.ToString());
        public void AddHoliday(DayOfWeek day)
        {
            //Ошибка когда вся неделя выходная
            if (CountHolidays >= 6) throw new ArgumentOutOfRangeException(nameof(day), "Слишком много выходных");
            if (!IsHoliday(day)) week[day] = true;
        }
        public void AddWorkday(DayOfWeek day)
        {
            if(IsHoliday(day)) week[day] = false;
        }
    }
}
