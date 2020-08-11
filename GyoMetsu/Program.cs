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

            var runner = new Emugen.OpenTK.Runner("魚滅の槍");

            runner.Run(new Scene.TitleScene());
            //runner.Run(new Scene.TestScene(@"data/script/battle_01.cs") );

            //runner.Run(new Scene.BattleScene(@"data/script/battle_01.cs") );
            //runner.Run(new Scene.BattleScene(@"data/script/battle_02.cs"));
            //runner.Run(new Scene.BattleScene(@"data/script/battle_03.cs"));
            //runner.Run( new Scene.TalkScene(@"data/script/talk_script_01.cs") );
            //runner.Run( new Scene.TalkScene(@"data/script/talk_script_02.cs") );
            //runner.Run( new Scene.TalkScene(@"data/script/talk_script_03.cs") );
            //runner.Run( new Scene.TalkScene(@"data/script/talk_script_04.cs") );
            //runner.Run( new Scene.TalkScene(@"data/script/talk_script_06.cs") );

        }
    }
}
