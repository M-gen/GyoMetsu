using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu
{
    public class StartScript
    {
        public string Title = "None";

        public List<Action<Emugen.OpenTK.Runner>> _Actions = new List<Action<Emugen.OpenTK.Runner>>();

        public void StartTitleScene()
        {
            _Actions.Add( (runner)=> {
                runner.Run(new Scene.TitleScene());
            });
        }

        public void StartTestScene()
        {
            _Actions.Add((runner) => {
                runner.Run(new Scene.TestScene(""));
            });
        }

        public void StartBattleScene(string scriptPath)
        {
            _Actions.Add((runner) => {
                runner.Run(new Scene.BattleScene(scriptPath));
            });
        }

        public void StartTalkScene(string scriptPath)
        {
            _Actions.Add((runner) => {
                runner.Run(new Scene.TalkScene(scriptPath));
            });
        }
    }
}
