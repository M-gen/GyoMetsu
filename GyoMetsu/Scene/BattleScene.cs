using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Image.Primitive;
using Emugen.Script;
using Emugen.Input;

using GyoMetsu.Data;
using GyoMetsu.Config;

namespace GyoMetsu.Scene
{
    public class BattleScene : Emugen.OpenTK.Scene
    {
        LayerSprite layer = new LayerSprite();

        UI.SkillCard.SkillCards SkillCards;
        UI.PlayerCard.PlayerCards PlayerCards;
        UI.EnemyCard.EnemyCards EnemyCards;

        UI.PlayerCard.PlayerCard SelectPlayerCard;
        UI.SkillCard.SkillCard SelectSkillCard;

        public enum UIStep
        {
            None,
            SkillSelect,
            SkillTargetSelect,
        }
        public enum UITargetSelectType
        {
            None,
            One,
            LineHorizontal,
        }


        public enum BattleStep
        {
            Main,
            End,
            LostClickWait,
        }

        UIStep uiStep = UIStep.None;
        //UITargetSelectType uITargetSelectType = UITargetSelectType.None;
        //int[] uITargetSelectTypePalams;

        BattleStep battleStep = BattleStep.Main;

        ScriptAPI scriptAPI;
        Script<ScriptAPI> script;

        UI.Talk.Choice LostClickWaitChoice;

        int actionTimer = 0;
        int actionTimerMax = 15;

        bool isInputEventNowFrame = false;

        public BattleScene( string scriptPath )
        {
            scriptAPI = new ScriptAPI();
            script = new Emugen.Script.Script<ScriptAPI>(scriptPath, scriptAPI);
            script.Run();

            var data = new Data.DataCreater();

            if (scriptAPI.__bgmPath != null)
            {
                var bgm = new Emugen.Sound.SoundPlayer(scriptAPI.__bgmPath, 0.20f, true, Emugen.Sound.SoundPlayer.SoundType.BGM);
            }

            SetupEnemys();
            SetupPlayers();

            {
                PlayerCards = new UI.PlayerCard.PlayerCards();
                layer.Add(PlayerCards, 30);
            }

            {
                EnemyCards = new UI.EnemyCard.EnemyCards();
                layer.Add(EnemyCards, 20);
            }

            {
                var bmp = new System.Drawing.Bitmap(scriptAPI.__backGroundImagePath);
                var texture = new Texture(bmp);

                var w = 1600;
                var h = (int)((double)bmp.Height / (double)bmp.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(0, 0), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite, 10);
            }
            {
                var skillCards = new UI.SkillCard.SkillCards();
                layer.Add(skillCards, 40);
                SkillCards = skillCards;
            }

            {
                var sprite = new PlaneSprite(new Rect(new Vector2D(20, 100), new Vector2D(200, 1)), new Color(0.5, 1, 1, 1));
                layer.Add(sprite, 10);
            }
            {
                foreach (var chara in data.playerCharacters)
                {
                    PlayerCards.Add( chara);
                }
                PlayerCards.SetupCardPos();
            }
            SetupEnemysCard();

            {
                PlayerCards.OnSelected += (selectCards) =>
                {
                    switch (uiStep)
                    {
                        case UIStep.None:
                            {
                                var i = (List<UI.PlayerCard.PlayerCard>)selectCards;
                                if (i[0].character.HP.Now==0)
                                {
                                    // 戦闘不能
                                }
                                else
                                {
                                    SkillCards.SetStep(UI.SkillCard.SkillCards.Step.ShowPlayerCharacterSkills, i[0].character);
                                    var soundPlayer = new Emugen.Sound.SoundPlayer(MainConfig.SoundEffect.UISelect01, 0.10f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                                    uiStep = UIStep.SkillSelect;
                                    SelectPlayerCard = selectCards[0];

                                }
                            }
                            break;
                        case UIStep.SkillSelect:
                            break;
                        case UIStep.SkillTargetSelect:
                            if (!isInputEventNowFrame)
                            {
                                isInputEventNowFrame = true;
                                new Emugen.Sound.SoundPlayer(MainConfig.SoundEffect.UISelect01, 0.10f, false, Emugen.Sound.SoundPlayer.SoundType.SE);

                                uiStep = UIStep.None;
                                EnemyCards.uITargetSelectType = UI.EnemyCard.EnemyCards.UITargetSelectType.None;
                                PlayerCards.uITargetSelectType = UI.PlayerCard.PlayerCards.UITargetSelectType.None;

                                var i = (List<UI.PlayerCard.PlayerCard>)selectCards;
                                //var i = (List<FafTk.UI.EnemyCard.EnemyCard>)selectCards;
                                var targetCharacters = new List<Data.Character>();
                                foreach( var j in i)
                                {
                                    targetCharacters.Add(j.character);
                                }

                                SelectPlayerCard.character.Action(SelectSkillCard.actionSkill, targetCharacters);
                            }
                            break;
                    }

                };
            }
            {
                SkillCards.OnSelected += (selectCard) =>
                {
                    switch (uiStep)
                    {
                        case UIStep.None:
                            break;
                        case UIStep.SkillSelect:
                            if (!isInputEventNowFrame)
                            {
                                isInputEventNowFrame = true;
                                var soundPlayer = new Emugen.Sound.SoundPlayer(MainConfig.SoundEffect.UISelect01, 0.10f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                                uiStep = UIStep.SkillTargetSelect;
                                SelectSkillCard = selectCard;

                                if (SelectSkillCard.actionSkill.Main.TargetWord.IndexOf("横列")==0)
                                {
                                    var command = SelectSkillCard.actionSkill.Main.TargetWord.Split(' ');
                                    
                                    EnemyCards.uITargetSelectType = UI.EnemyCard.EnemyCards.UITargetSelectType.LineHorizontal;
                                    EnemyCards.uITargetSelectTypePalams = new int[] { int.Parse(command[1]), int.Parse(command[2]), int.Parse(command[3]) };
                                    PlayerCards.uITargetSelectType = UI.PlayerCard.PlayerCards.UITargetSelectType.LineHorizontal;
                                    PlayerCards.uITargetSelectTypePalams = new int[] { int.Parse(command[1]), int.Parse(command[2]), int.Parse(command[3]) };

                                }
                                else
                                {
                                    var command = SelectSkillCard.actionSkill.Main.TargetWord.Split(' ');
                                    
                                    EnemyCards.uITargetSelectType = UI.EnemyCard.EnemyCards.UITargetSelectType.One;
                                    EnemyCards.uITargetSelectTypePalams = new int[] { int.Parse(command[1]) };
                                    PlayerCards.uITargetSelectType = UI.PlayerCard.PlayerCards.UITargetSelectType.One;
                                    PlayerCards.uITargetSelectTypePalams = new int[] { int.Parse(command[1]) };
                                }
                            }
                            break;
                        case UIStep.SkillTargetSelect:
                            break;
                    }

                };

                SkillCards.OnClose += () =>
                {
                    switch (uiStep)
                    {
                        case UIStep.None:
                            break;
                        case UIStep.SkillSelect:
                            if (!isInputEventNowFrame)
                            {
                                isInputEventNowFrame = true;
                                var soundPlayer = new Emugen.Sound.SoundPlayer(MainConfig.SoundEffect.UICancel01, 0.15f, false, Emugen.Sound.SoundPlayer.SoundType.SE);

                                uiStep = UIStep.None;
                                EnemyCards.uITargetSelectType = UI.EnemyCard.EnemyCards.UITargetSelectType.None;
                                PlayerCards.uITargetSelectType = UI.PlayerCard.PlayerCards.UITargetSelectType.None;
                            }
                            break;
                        case UIStep.SkillTargetSelect:
                            break;
                    }

                };
            }
            {
                EnemyCards.OnSelected += (selectCards) =>
                {

                    switch (uiStep)
                    {
                        case UIStep.None:
                            break;
                        case UIStep.SkillSelect:
                            break;
                        case UIStep.SkillTargetSelect:
                            if (!isInputEventNowFrame)
                            {
                                isInputEventNowFrame = true;

                                uiStep = UIStep.None;
                                EnemyCards.uITargetSelectType = UI.EnemyCard.EnemyCards.UITargetSelectType.None;
                                PlayerCards.uITargetSelectType = UI.PlayerCard.PlayerCards.UITargetSelectType.None;

                                //var result = false;

                                var targetCharacters = new List<Character>();
                                var i = (List<UI.EnemyCard.EnemyCard>) selectCards;
                                foreach( var j in i)
                                {
                                    targetCharacters.Add(j.character);
                                }
                                var result = SelectPlayerCard.character.Action(SelectSkillCard.actionSkill, targetCharacters);

                                if (result) {
                                    new Emugen.Sound.SoundPlayer(MainConfig.SoundEffect.UISelect01, 0.10f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                                }
                                else
                                {
                                    new Emugen.Sound.SoundPlayer(MainConfig.SoundEffect.UICancel01, 0.10f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                                }
                            }

                            break;
                    }

                };
            }
        }

        private void SetupEnemys()
        {
            var data = Data.DataCreater.Instance;

            foreach (var command in scriptAPI.__enemyCommands)
            {
                var enemeyCreator = new Data.EnemyCreator(command);

                var chara = new Data.Character();
                chara.ViewName = enemeyCreator.scriptAPI.Name;
                chara.imagePath = enemeyCreator.scriptAPI.ImagePath;
                chara.imageScale = enemeyCreator.scriptAPI.ImageScale;
                chara.imageSideMargin = enemeyCreator.scriptAPI.ImageSideMargin;
                chara.HP.Now = chara.HP.Base = chara.HP.Max = enemeyCreator.scriptAPI.HP;

                foreach (var i in enemeyCreator.scriptAPI.BattleStatusBaseParams)
                {
                    foreach( var j in chara.BattleStatusParams)
                    {
                        if (i.Key==j.Key)
                        {
                            j.Value.Base = j.Value.Now = i.Value;
                            break;
                        }
                    }
                }

                foreach (var ad in enemeyCreator.scriptAPI.ActionSkillDatas)
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

                foreach (var p in enemeyCreator.scriptAPI.ElementLingPart)
                {
                    chara.ElementLing.Bodys.Add(p);
                }

                data.enemyCharacters.Add(chara);
            }
        }

        private void SetupEnemysCard()
        {
            var data = Data.DataCreater.Instance;
            foreach (var chara in data.enemyCharacters)
            {
                EnemyCards.Add(chara);
            }
            PlayerCards.SetupCardPos(); // todo : ミス ?

        }

        private void SetupPlayers()
        {
            var data = Data.DataCreater.Instance;

            foreach (var q in scriptAPI.__pcCommands)
            {

                var creator = new Data.PlayerCreator(q);

                var chara = new Data.Character();
                chara.ViewName = creator.scriptAPI.Name;
                chara.imagePath = creator.scriptAPI.ImagePath;
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

        public override void Update()
        {
            isInputEventNowFrame = false;
            //uiStep
            switch (uiStep)
            {
                case UIStep.None:
                    break;
                case UIStep.SkillSelect:
                    break;
                case UIStep.SkillTargetSelect:
                    {
                        if ((InputCore.Instance.GetKeyEventType(InputCore.KeyEventCode.MouseRightButton) == InputCore.KeyEventType.Up)) { 
                            if (!isInputEventNowFrame)
                            {
                                isInputEventNowFrame = true;
                                uiStep = UIStep.None;
                                new Emugen.Sound.SoundPlayer(MainConfig.SoundEffect.UICancel01, 0.10f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                            }
                        }
                    }
                    break;
            }

            Data.DataCreater.Instance.Update();
            SkillCards.Update();
            PlayerCards.Update();
            EnemyCards.Update();


            switch (battleStep)
            {
                case BattleStep.Main:
                    if (actionTimer==0) {
                        Action action = null;
                        Data.Character character = null;
                        foreach (var i in DataCreater.Instance.playerCharacters)
                        {
                            if (i.ActionStock != null)
                            {
                                action = i.ActionStock;
                                character = i;
                            }
                        }
                        foreach (var i in DataCreater.Instance.enemyCharacters)
                        {
                            if (i.ActionStock != null)
                            {
                                action = i.ActionStock;
                                character = i;
                            }
                        }
                        if (action != null)
                        {
                            action();
                            character.ActionStock = null;
                        }
                        actionTimer = actionTimerMax;

                    }
                    else
                    {
                        actionTimer--;
                    }

                    if( EnemyCards.Count ==0 )
                    {
                        battleStep = BattleStep.End;

                        var bgm = new Emugen.Sound.SoundPlayer(Config.MainConfig.BattleScene.BGMStageClear, 0.20f, true, Emugen.Sound.SoundPlayer.SoundType.BGM);

                    }
                    else if (DataCreater.Instance.IsPlayerCharacterAllDead())
                    {
                        battleStep = BattleStep.LostClickWait;

                        var choice = new UI.Talk.Choice( new string[]{ "もう一度戦う", "タイトルに戻る" } );
                        layer.Add(choice, 200);

                        LostClickWaitChoice = choice;
                        var bgm = new Emugen.Sound.SoundPlayer(Config.MainConfig.BattleScene.BGMGameOver, 0.20f, true, Emugen.Sound.SoundPlayer.SoundType.BGM);
                    }
                    else
                    {
                        var data = Data.DataCreater.Instance;

                        var tmp = new List<Data.Character>();

                        foreach(var i in data.enemyCharacters.Where(x => x.HP.Now <= 0) )
                        {
                            tmp.Add(i);
                        }
                        
                        foreach( var i in tmp )
                        {
                            EnemyCards.Del(i.enemyCard);
                            data.enemyCharacters.Remove(i);
                            
                        }
                    }
                    break;
                case BattleStep.LostClickWait:
                    {
                        LostClickWaitChoice.Update();

                        var input = Emugen.Input.InputCore.Instance;
                        if ((input.GetKeyEventType(Emugen.Input.InputCore.KeyEventCode.MouseLeftButton) == Emugen.Input.InputCore.KeyEventType.Up))
                        {
                            var tmp = LostClickWaitChoice.GetChoiseText();

                            switch (tmp)
                            {
                                case "もう一度戦う":
                                    {
                                        WindowManager.nextScene = new Scene.BattleScene(this.script.Path);
                                        //}
                                    }
                                    break;
                                case "タイトルに戻る":
                                    {
                                        WindowManager.nextScene = new Scene.TitleScene();
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case BattleStep.End:
                    {
                        var input = Emugen.Input.InputCore.Instance;
                        if ((input.GetKeyEventType(Emugen.Input.InputCore.KeyEventCode.MouseLeftButton) == Emugen.Input.InputCore.KeyEventType.Up))
                        {
                            WindowManager.nextScene = new Scene.TalkScene(scriptAPI.__nextTalkScriptPath);
                        }
                    }
                    break;
            }
        }

        public override void Draw()
        {
            layer.Draw();
        }


        public class ScriptAPI
        {
            public string __backGroundImagePath;
            public string __bgmPath;
            public string __nextTalkScriptPath;
            public List<string> __enemyCommands = new List<string>();
            public List<string> __pcCommands = new List<string>();


            public void AddEnemy(string command)
            {
                __enemyCommands.Add(command);
            }
            public void AddPlayerCharacter(string command)
            {
                __pcCommands.Add(command);
            }

            public void SetBackGroundImagePath( string backGroundImagePath)
            {
                __backGroundImagePath = backGroundImagePath;
            }

            public void SetBGM( string bgmPath )
            {
                __bgmPath = bgmPath;
            }

            public void NextTalkScene( string nextTalkScriptPath)
            {
                __nextTalkScriptPath = nextTalkScriptPath;
            }
        }

    }
}
