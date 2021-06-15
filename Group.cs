using System;
using System.Collections.Generic;
using System.Resources;


namespace gratch_core
{
    /// <summary>
    /// Класс для работы с группами юнитов.
    /// </summary>
    internal class Group
    {
        //Fields
        private string name;
        private Workweek workweek;
        private List<Unit> units;
        //Properties
        public string Name
        {
            get => name;
            set
            {
                if (value != null && value.Length > 2) name = value;
            }
        }
        public Workweek Workweek { get => workweek; set => workweek = value; }
        public List<Unit> Units { get => units; set => units = value; }
        //Constructors
        public Group() { }
        public Group(string GroupName)
        {
            if (GroupName != null && GroupName.Length > 2) name = GroupName;
        }
        //Methods

    }
}
