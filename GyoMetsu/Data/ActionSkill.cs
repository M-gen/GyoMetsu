using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Data
{
    public class ActionSkill
    {
        public string Name;
        public string Cost;

        public ActionStatus Main;
        public List<ActionStatus> SubItems = new List<ActionStatus>();

    }
    public class ActionStatus
    {
        public string TargetWord;     //
        public string HitWord;        // 命中 85 , 自動成功
        public string EffectWord;     // ダメージ 15 1 2 = 15 + 1D2
        public string SubElementWord; // 炎, 魚, 槍
    }
}
