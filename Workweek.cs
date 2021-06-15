using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    /// <summary>
    /// Класс для работы с рабочей неделей
    /// </summary>
    internal class Workweek
    {
        //Fields
        /// <summary>
        /// <see cref="local"/> - файл с локализацией. dof.GetString(DayOfWeek.*);
        /// </summary>
        private readonly ResourceManager local = new("dayofweek-ru",
            typeof(Group).Assembly);
        private Dictionary<DayOfWeek, bool> week;

        //Properties

        //Constructors
        public Workweek() : this(DayOfWeek.Saturday, DayOfWeek.Sunday)
        {

        }
        public Workweek(params DayOfWeek[] weekend)
        {
            //Ошибка когда вся неделя выходная
            if (weekend.Length > 6) throw new ArgumentOutOfRangeException(nameof(weekend), "Слишком много выходных");
            //Инициализация словаря День - Логика
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek))) week.Add(day, false);
            //Заполнение данными словаря
            foreach (var day in weekend) week[day] = true;
        }
        //Methods
        public bool IsWeekend(DayOfWeek day) => week[day];
    }
}
