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

        public void AddPlayerCharacter(string scriptPath)
        {
            var data = Data.DataCreater.Instance;

            var creator = new Data.PlayerCreator(scriptPath);

            var chara = new Data.Character();
            chara.ViewName = creator.scriptAPI.Name;
            chara.imagePath = creator.scriptAPI.ImagePath;
            chara.voiceDirecotryPath = creator.scriptAPI.VoiceDirecotryPath;
            chara.HP.Now = chara.HP.Base = chara.HP.Max = creator.scriptAPI.HP;

            foreach (var i in creator.scriptAPI.BattleStatusBaseParams)
            {
                foreach (var j in chara.BattleStatusParams)
                {
                    if (i.Key == j.Key)
                    {
                        j.Value.Base = j.Value.Now = i.Value;
                        break;
                    }
                }
            }

            foreach (var ad in creator.scriptAPI.ActionSkillDatas)
            {
                var datas = Data.ActionSkillDatas.Instance;

                foreach (var i in datas.Items)
                {
                    if (i.Name == ad.Name)
                    {
                        chara.Actions.Add(i);
                        break;
                    }
                }
            }

            foreach (var p in creator.scriptAPI.ElementLingPart)
            {
                chara.ElementLing.Bodys.Add(p);
            }

            foreach (var p in creator.scriptAPI.DefalutStockElement)
            {
                chara.Elements.Add(new Data.Element(p.ToString()));
            }
            data.playerCharacters.Add(chara);

        }
    }
}
