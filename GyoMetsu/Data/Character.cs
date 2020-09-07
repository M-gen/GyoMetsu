using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Data
{
    public class BattleStatusResource
    {
        public string Key;
        public int Now;
        public int Max;
        public int Base;
        public int Bonus;

        public BattleStatusResource(string key, int value)
        {
            this.Key = key;
            Now = Max = Base = value;
        }
    }

    public class BattleStatusParam
    {
        public string Key;
        public int Now;
        public int Base;
        public int Bonus;

        public BattleStatusParam(string key)
        {
            this.Key = key;
        }
    }


    public class Character
    {
        public string ID = "";
        public string Name = "";
        public string ViewName = "";
        public List<ActionSkill> Actions = new List<ActionSkill>();
        public List<Element> Elements = new List<Element>();
        public ElementLing ElementLing = new ElementLing();

        public string imagePath       = "";
        public double imageScale      = 1.0;
        public double imageSideMargin = 1.0;
        public string voiceDirecotryPath = "";

        public double elementTimer = 0;
        public double elementTimerMax = 300;

        public int linePosition = 0;

        public UI.PlayerCard.PlayerCard playerCard;
        public UI.EnemyCard.EnemyCard   enemyCard;

        // リソース Now, MAX, Bounus, Base
        public BattleStatusResource HP = new BattleStatusResource("HP", 100);
        public Dictionary<string, BattleStatusParam> BattleStatusParams = new Dictionary<string, BattleStatusParam>();

        static private Random random = new Random(); // Todo : ゲーム用乱数を統一で保持したほうが良い？

        public double HPBarFrameWidthScale = 1.0;
        public Action ActionStock;


        public Character()
        {
            BattleStatusParams.Add("DMG",   new BattleStatusParam("DMG")  );  // ダメージ
            BattleStatusParams.Add("ACC",   new BattleStatusParam("ACC")  );  // 命中
            BattleStatusParams.Add("DOGE",  new BattleStatusParam("DOGE"));   // 回避
            BattleStatusParams.Add("PROT",  new BattleStatusParam("PROT")  ); // 防御力 
        }

        public void Update()
        {
            if (HP.Now == 0) return;

            elementTimer++;
            if (elementTimer >= elementTimerMax)
            {
                elementTimer = 0;


                var elements = ElementLing.NextElements();
                if (elements != null)
                {

                    if (true)
                    {
                        var soundPlayer = new Emugen.Sound.SoundPlayer("data/se/システム音4(1秒).mp3", 0.10f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                    }

                    foreach (var element in elements)
                    {
                        Elements.Add(element);
                        if (playerCard != null) playerCard.AddElement(element);
                        if (enemyCard != null) enemyCard.AddElement(element);
                    }

                    var elementOver = Elements.Count - 5;
                    if (elementOver > 0)
                    {
                        for (var i = 0; i < elementOver; i++)
                        {
                            Elements.RemoveAt(0);
                            if (playerCard != null) playerCard.RemoveElement(0);
                            if (enemyCard != null) enemyCard.RemoveElement(0);
                        }
                    }
                }

            }

            // 敵である場合のAI処理
            if( enemyCard!=null)
            {
                if ( (Elements.Count > 0) && (!DataCreater.Instance.IsPlayerCharacterAllDead()) )
                {
                    ActionSkill targetAction = null;
                    foreach(var action in Actions)
                    {
                        if (Element.IsUsable(Elements, action.Cost)) targetAction = action;
                    }

                    if(targetAction!=null)
                    {
                        if ( targetAction.Main.TargetWord.IndexOf("横列") == 0 )
                        {
                            // 範囲攻撃
                            var targetCharacters = new List<Character>();
                            foreach( var i in DataCreater.Instance.playerCharacters)
                            {
                                targetCharacters.Add(i);
                            }
                            Action(targetAction, targetCharacters);
                        }
                        else
                        {
                            // todo : パーティメンバーの前半を優先する…
                            var j = random.Next(DataCreater.Instance.playerCharacters.Count);
                            var targetCharacter = DataCreater.Instance.playerCharacters[j];
                            while (targetCharacter.HP.Now <= 0)
                            {
                                j = random.Next(DataCreater.Instance.playerCharacters.Count);
                                targetCharacter = DataCreater.Instance.playerCharacters[j];
                            }

                            var targetCharacters = new List<Character>();
                            targetCharacters.Add(targetCharacter);
                            Action(targetAction, targetCharacters);

                        }
                    }
                }
            }
        }

        public GyoMetsu.UI.CharacterCard GetCharacterCard()
        {
            var res = default(GyoMetsu.UI.CharacterCard);

            if (playerCard != null) res = playerCard;
            if (enemyCard != null) res = enemyCard;

            return res;
        }

        public bool Action(ActionSkill action, List<Character> targetCharacters)
        {
            if (!Element.IsUsable(Elements, action.Cost)) return false;

            ActionStock = () => { _Action(action, targetCharacters);  };

            return true;
        }

        public void _Action( ActionSkill actionSkill, List<Character> targetCharacters)
        {
            if (!Element.IsUsable(Elements, actionSkill.Cost)) return;


            Action a = () =>
            {
                foreach (var targetCharacter in targetCharacters)
                {
                    var targetCard = targetCharacter.GetCharacterCard();

                    if (actionSkill.Main.EffectWord.IndexOf("HPダメージ") == 0)
                    {
                        var isHit = false;
                        {
                            var command = actionSkill.Main.HitWord.Split(' ');
                            if (command[0] == "命中")
                            {
                                var value = int.Parse(command[1]) + this.BattleStatusParams["ACC"].Now;
                                var value_nd   = int.Parse(command[2]);
                                var value_dn   = int.Parse(command[3]);
                                for (var i = 0; i < value_nd; i++)
                                {
                                    value += random.Next(1, value_dn + 1);
                                }

                                var value_t =  targetCharacter.BattleStatusParams["DOGE"].Now;
                                var value_t_nd = 2;
                                var value_t_dn = 6;
                                for (var i = 0; i < value_t_nd; i++)
                                {
                                    value_t += random.Next(1, value_t_dn + 1);
                                }

                                if ( value > value_t)
                                {
                                    isHit = true;
                                }
                            }
                            else
                            {
                                isHit = true;
                            }

                        }

                        if (isHit)
                        {
                            var lastHP = targetCharacter.HP.Now;

                            var command = actionSkill.Main.EffectWord.Split(' ');
                            var value = int.Parse(command[1]) + this.BattleStatusParams["DMG"].Now - targetCharacter.BattleStatusParams["PROT"].Now;
                            var value_nd = int.Parse(command[2]);
                            var value_dn = int.Parse(command[3]);

                            for (var i = 0; i < value_nd; i++)
                            {
                                value += random.Next(1, value_dn + 1);
                            }
                            if (value < 0) value = 0;

                            targetCharacter.HP.Now -= value;
                            if (targetCharacter.HP.Now <= 0)
                            {
                                targetCharacter.HP.Now = 0;

                                if (lastHP != 0)
                                {
                                    targetCharacter.Elements.Clear();
                                    targetCard.RemoveElementAll();
                                }

                                if (targetCard.character.voiceDirecotryPath != "")
                                {
                                    var path = targetCard.character.voiceDirecotryPath + "/Unable_To_Fight.wav";
                                    var voice = new Emugen.Sound.SoundPlayer(path, 0.80f, false, Emugen.Sound.SoundPlayer.SoundType.Voice);
                                }
                            }
                            else
                            {
                                if (targetCard.character.voiceDirecotryPath != "")
                                {
                                    var r = random.Next(2);
                                    var path = "";
                                    switch (r)
                                    {
                                        case 0: path = targetCard.character.voiceDirecotryPath + "/Damage_1.wav"; break;
                                        case 1: path = targetCard.character.voiceDirecotryPath + "/Damage_2.wav"; break;
                                    }
                                    var voice = new Emugen.Sound.SoundPlayer(path, 0.80f, false, Emugen.Sound.SoundPlayer.SoundType.Voice);
                                }

                                targetCard.damageTimer = 0;
                            }
                            targetCard.ShowDamage(value);
                            targetCard.RefreshViewHP();

                        }
                        else
                        {
                            if (targetCard.character.voiceDirecotryPath != "")
                            {
                                var r = random.Next(2);
                                var path = "";
                                switch (r)
                                {
                                    case 0: path = targetCard.character.voiceDirecotryPath + "/No_Damage_1.wav"; break;
                                    case 1: path = targetCard.character.voiceDirecotryPath + "/No_Damage_2.wav"; break;
                                }
                                var voice = new Emugen.Sound.SoundPlayer(path, 0.80f, false, Emugen.Sound.SoundPlayer.SoundType.Voice);
                            }

                            targetCard.ShowMiss();

                        }
                    }
                    else if (actionSkill.Main.EffectWord.IndexOf("HP回復") == 0)
                    {
                        var command = actionSkill.Main.EffectWord.Split(' ');
                        var value = int.Parse(command[1]);
                        var value_nd = int.Parse(command[2]);
                        var value_dn = int.Parse(command[3]);

                        for (var i = 0; i < value_nd; i++)
                        {
                            value += random.Next(1, value_dn + 1);
                        }
                        if (value < 0) value = 0;

                        targetCharacter.HP.Now += value;
                        if (targetCharacter.HP.Now > targetCharacter.HP.Max)
                        {
                            targetCharacter.HP.Now = targetCharacter.HP.Max;
                        }
                        targetCard.ShowHealing(value);
                        targetCard.RefreshViewHP();
                    }
                }
            };

            this.GetCharacterCard().ActionEffect(a, actionSkill);
            RemoveElement(actionSkill.Cost);

        }

        public void RemoveElement(string actionCost)
        {
            var cost = new Dictionary<string, int>();
            cost.Add("祈", 0);
            cost.Add("樹", 0);
            cost.Add("獣", 0);
            cost.Add("理", 0);
            cost.Add("鉄", 0);

            var costElementNum = 0;
            //var haveElementNum = 0;
            var anyElementCost = 0;

            foreach (var i in actionCost)
            {
                if ("0123456789".IndexOf(i) >= 0)
                {
                    // 数値である
                    anyElementCost = int.Parse(i.ToString());
                }
                else
                {
                    // エレメント
                    cost[i.ToString()]++;
                    costElementNum++;
                }
            }

            foreach( var i in cost)
            {
                for( var j=0; j<i.Value; j++)
                {
                    RemoveElementByName(i.Key);
                }
            }
            for (var j = 0; j < anyElementCost; j++)
            {
                Elements.RemoveAt(0);
                //playerCard.RemoveElement(0);
                if (playerCard != null) playerCard.RemoveElement(0);
                if (enemyCard != null) enemyCard.RemoveElement(0);
            }
        }

        void RemoveElementByName( string name )
        {
            var j = 0;
            foreach( var i in Elements)
            {
                if (name==i.Name)
                {
                    break;
                }
                j++;
            }

            Elements.RemoveAt(j);
            if (playerCard != null) playerCard.RemoveElement(j);
            if (enemyCard != null) enemyCard.RemoveElement(j);
        }
    }
}
