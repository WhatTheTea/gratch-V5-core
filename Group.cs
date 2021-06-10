using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Resources;


namespace gratch_core
{
    /// <summary>
    /// Класс для работы с группами юнитов
    /// dayOfWeek - файл с локализацией System.DayOfWeek
    /// </summary>
    internal class Group
    {
        private ResourceManager dayOfWeek = new ("dayofweek-ru", typeof(Group).Assembly);
        private List<DayOfWeek> weekend;
        private List<Unit> units;
        private DateTime lastCommit;
        /// <summary>
        /// Стандартный конструктор группы юнитов, выходные: Суббота, Воскресенье
        /// </summary>
        /// <param name="Units">Набор юнитов</param>
        public Group(List<Unit> Units)
        {
            units = Units;
            weekend = new () { DayOfWeek.Saturday, DayOfWeek.Sunday };
            lastCommit = DateTime.Now;
        }
        /// <summary>
        /// Конструктор группы юнитов с ручной настройкой выходных
        /// </summary>
        /// <param name="Units">Набор юнитов</param>
        /// <param name="Weekend">Выходные System.DayOfWeek</param>
        public Group(List<Unit> Units, params DayOfWeek[] Weekend)
        {
            units = Units;
            weekend = Weekend.ToList();
            lastCommit = DateTime.Now;
        }
    }
}
