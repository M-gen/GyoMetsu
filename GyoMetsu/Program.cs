using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using GyoMetsu.Data;

namespace GyoMetsu
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 準備
            {
                GyoMetsu.Config.MainConfig.Setup();
                var scriptFrags = new ScriptFlags();

                {
                    if (ActionSkillDatas.Instance == null) new ActionSkillDatas();
                    var datas = ActionSkillDatas.Instance;

                    var files = System.IO.Directory.GetFiles("data/script/action_skill/", "*.cs");

                    foreach (var i in files)
                    {
                        datas.Load(i);
                    }

                }
            }

            var script = new Emugen.Script.Script<StartScript>("data/script/init.cs", new StartScript());
            script.Run();

            var runner = new Emugen.OpenTK.Runner(script.api.Title);

            foreach ( var action in script.api._Actions )
            {
                action(runner);
            }
        }
    }
}
