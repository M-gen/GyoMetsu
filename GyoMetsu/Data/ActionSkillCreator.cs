using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Data
{
    public class ActionSkillCreator
    {
        public ScriptAPI scriptAPI = new ScriptAPI();

        public ActionSkillCreator(string scriptPath)
        {
            var script = new Emugen.Script.Script<ScriptAPI>(scriptPath, scriptAPI);
            script.Run();
        }

        public class ScriptAPI
        {
            public ActionSkill ActionSkill = new ActionSkill();

            public void SetMain(string TargetWord, string HitWord, string EffectWord, string SubElementWord)
            {
                var i = new ActionStatus();
                i.TargetWord = TargetWord;
                i.HitWord = HitWord;
                i.EffectWord = EffectWord;
                i.SubElementWord = SubElementWord;
                ActionSkill.Main = i;
            }
            public void AddSub(string TargetWord, string HitWord, string EffectWord, string SubElementWord)
            {
                var i = new ActionStatus();
                i.TargetWord = TargetWord;
                i.HitWord = HitWord;
                i.EffectWord = EffectWord;
                i.SubElementWord = SubElementWord;
                ActionSkill.SubItems.Add( i );
            }

        }
    }
}
