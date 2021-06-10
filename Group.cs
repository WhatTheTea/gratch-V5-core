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
    /// Класс для работы с группами юнитов.
    /// </summary>
    internal class Group
    {
        private string name;
        /// dof - файл с локализацией. dayOfWeek.GetString(DayOfWeek.*);
        private ResourceManager dof = new ("dayofweek-ru",
            typeof(Group).Assembly);
        private List<DayOfWeek> weekend;
        private List<Unit> units;
        private DateTime lastCommit;

        public string Name
        {
            get { return name; }
            set
            {
                if (value != null && value.Length > 2) name = value;
            }
        }
        public List<Unit> Units => units;
        /// <summary>
        /// Конструктор новой группы
        /// </summary>
        /// <param name="GroupName">Имя создаваемой группы</param>
        public Group(string GroupName)
        {
            if (GroupName != null && GroupName.Length > 2) name = GroupName;
        }

        /*
        /// <summary>
        /// Стандартный конструктор группы юнитов, выходные: Суббота, Воскресенье
        /// </summary>
        /// <param name="Units">Набор юнитов</param>
        public Group(List<Unit> Units) : this(Units,
            DayOfWeek.Saturday, DayOfWeek.Sunday) { }
        /// <summary>
        /// Конструктор группы юнитов с ручной настройкой выходных
        /// </summary>
        /// <param name="Units">Набор юнитов</param>
        /// <param name="Weekend">Выходные</param>
        public Group(List<Unit> Units, params DayOfWeek[] Weekend)
        {
            id++;
            units = Units;
            weekend = Weekend.ToList();
            lastCommit = DateTime.Now;
        }
        */
    }
}
