using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Config
{
    public class MainConfig
    {
        public static string MainFontPath = @"data/font/rounded-mgenplus/rounded-mgenplus-1cp-bold.ttf";

        public class SoundEffect
        {
            public static string UISelect01 = @"data/se/FM_セレクト27(1秒).mp3";
            public static string UICancel01 = @"data/se/キャンセル音9(1秒)s.mp3";
        }

        public class TalkScene
        {
            public static string NextWaitIcon = @"data/image/会話Downアイコン_02.png";
        }

        public class BattleScene
        {
            public static string EnemyShadowImage       = @"data/image/エネミー影_01.png";
            public static string SoundEffectCharacterKO = @"data/se/SE_KO.wav";
            public static string BGMStageClear          = @"data/bgm/BattleClear/BGM_StageClear.wav";
            public static string BGMGameOver            = @"data/bgm/BGM_GameOver.wav";
        }

        public class TitleScene
        {
            public static string TitleBackgroundImage = @"data/image/title.png";
            public static string TitleBGM             = @"data\bgm\02_祈りの部屋[Prayer room].mp3";
            public static string SelectSE             = @"data\se\SE_ItemGem.wav";
            public static string NewGameStartScript   = @"data/script/talk_script_01.cs";
        }

        public class Card
        {
            public static string SkillDefault = @"data/image/スキルカード.png";
        }

        public class Element
        {
            public string Word;
            public string ImagePath;
            public Element( string word, string imagePath )
            {
                Word      = word;
                ImagePath = imagePath;
            }
        }

        public static List<Element> Elements;

        public static void Setup()
        {
            Elements = new List<Element>();
            Elements.Add(new Element("祈", "data/image/elem/祈_枠2.png"));
            Elements.Add(new Element("樹", "data/image/elem/樹_枠2.png"));
            Elements.Add(new Element("獣", "data/image/elem/獣_枠2.png"));
            Elements.Add(new Element("理", "data/image/elem/理_枠2.png"));
            Elements.Add(new Element("鉄", "data/image/elem/鉄_枠2.png"));

        }
    }
}
