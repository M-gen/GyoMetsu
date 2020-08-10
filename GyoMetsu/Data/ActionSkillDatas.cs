using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Data
{
    public class ActionSkillDatas
    {
        static public ActionSkillDatas Instance;
        public List<ActionSkill> Items = new List<ActionSkill>();

        public ActionSkillDatas()
        {
            Instance = this;
        }

        public void Load( string path )
        {
            var Creator = new ActionSkillCreator(path);
            var ActionSkill = Creator.scriptAPI.ActionSkill;
            Items.Add(ActionSkill);
        }

        public ActionSkill Get( string name )
        {
            foreach( var i in Items)
            {
                if (i.Name == name) return i;
            }
            return null;
        }
    }
}
