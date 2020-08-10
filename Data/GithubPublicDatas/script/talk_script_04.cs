SetBackGroundImagePath("data/image/bg/背景.png");
SetBGM( "data/bgm/13_囁き[Whisper].mp3" );

// SetCharacterImage("data/image/Characters/聖職者女_h720.png");

switch( GetValue("倒し方") )
{
    case "ひっそり":
        {
            SetCharacterImage("data/image/Characters/侍男_h2.png");
            SetCharacterViewName( "ソレガシ" );
            Talk("よし、奥に進んだぞ。\nなにやら半魚人共が川でなにかしておるな？");
            WaitByEnter();

            ClearText();
            SetCharacterImage("data/image/enm/47_サハギン.png");
            SetCharacterViewName( "半魚人" );
            Talk("ぎょいっぎょいっ、ぎょいっぎょいっ");
            WaitByEnter();

            var isLoop = true;
            var isSelect1 = false;

            {
                var key = WaitChoise( new string[]{
                        "川辺の半魚人を退治してしまおう",
                        "何をしているのか観察しよう",
                    } );

                switch (key)
                {
                    case "川辺の半魚人を退治してしまおう":
                        ClearText();
                        SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                        SetCharacterViewName( "トレンリー" );
                        Talk("よし、いくぞ！");
                        WaitByEnter();

                        NextSceneBattle( "data/script/battle_02.cs" );

                        break;
                    case "何をしているのか観察しよう":

                        //PlaySound("data/se/SE_HitBlow_M.wav", 0.6f);

                        ClearText();
                        SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                        SetCharacterViewName( "レイチェル" );
                        Talk("どうやら、ゴミを掃除しているようですね？");
                        WaitByEnter();

                        //PlaySound("data/se/SE_KO.wav", 0.6f);
                        
                        ClearText();
                        SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                        SetCharacterViewName( "トレンリー" );
                        Talk("なんだ、特に害はなさそうだな？");
                        WaitByEnter();
                        
                        {
                            var key2 = WaitChoise( new string[]{
                                    "このまま帰って報告する",
                                    "とにかく倒してしまったほうがお金にならないか？",
                                    "一緒に手伝おう",
                                } );
                            switch( key2)
                            {
                                case "このまま帰って報告する":
                                    ClearText();
                                    SetCharacterImage("data/image/Characters/侍男_h2.png");
                                    SetCharacterViewName( "ソレガシ" );
                                    Talk("よし、ひきあげるぞ。");
                                    WaitByEnter();

                                    SetValue( "クマドラゴン戦わず", true);

                                    NextSceneTalk( "data/script/talk_script_06.cs" );
                                    break;
                                case "とにかく倒してしまったほうがお金にならないか？":
                                    NextSceneBattle( "data/script/battle_02.cs" );
                                    break;
                                case "一緒に手伝おう":
                                    ClearText();
                                    SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                                    SetCharacterViewName( "レイチェル" );
                                    Talk("でも、言葉が通じませんわ。\nここは見守りましょう。");
                                    WaitByEnter();

                                    SetBGM( "data/bgm/Battle_Boss/15_一歩踏み出す勇気[Courage to step one step].mp3" );
                                    
                                    // Todo : 見守った分のボーナス・メリットがほしい、クマドラゴンのHPが下がってるとか

                                    ClearText();
                                    SetCharacterImage("data/image/Characters/侍男_h2.png");
                                    SetCharacterViewName( "ソレガシ" );
                                    Talk("おい！\nクマドラゴンが現れて半魚人を襲っているぞ！");
                                    WaitByEnter();
                                    
                                    ClearText();
                                    SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                                    SetCharacterViewName( "レイチェル" );
                                    Talk("きゃ、こっちに来る！");
                                    WaitByEnter();
                                    
                                    NextSceneBattle( "data/script/battle_03.cs" );

                                    break;
                            }
                        }
                        // Todo : このあとどうしようか？
                        
                        break;
                }
            }
        }
        break;

    default:
        {
            SetCharacterImage("data/image/Characters/侍男_h2.png");
            SetCharacterViewName( "ソレガシ" );
            Talk("よし、奥に進んだぞ。\nなにやら半魚人共が川でなにかしておるな？");
            WaitByEnter();

            ClearText();
            SetCharacterImage("data/image/enm/47_サハギン.png");
            SetCharacterViewName( "半魚人" );
            Talk("ぎょいっぎょいっ、ぎょいっぎょいっ");
            WaitByEnter();

            var isLoop = true;
            var isSelect1 = false;

            {
                var key = WaitChoise( new string[]{
                        "川辺の半魚人を退治してしまおう",
                        "何をしているのか観察しよう",
                    } );

                switch (key)
                {
                    case "川辺の半魚人を退治してしまおう":
                        ClearText();
                        SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                        SetCharacterViewName( "トレンリー" );
                        Talk("よし、いくぞ！");
                        WaitByEnter();

                        NextSceneBattle( "data/script/battle_02.cs" );

                        break;
                    case "何をしているのか観察しよう":

                        ClearText();
                        SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                        SetCharacterViewName( "レイチェル" );
                        Talk("どうやら、ゴミを掃除しているようですね？");
                        WaitByEnter();

                        ClearText();
                        SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                        SetCharacterViewName( "トレンリー" );
                        Talk("なんだ、特に害はなさそうだな？");
                        WaitByEnter();
                        
                        {
                            var key2 = WaitChoise( new string[]{
                                    "このまま帰って報告する",
                                    "とにかく倒してしまったほうがお金にならないか？",
                                    "一緒に手伝おう",
                                } );
                            switch( key2)
                            {
                                case "このまま帰って報告する":
                                    ClearText();
                                    SetCharacterImage("data/image/Characters/侍男_h2.png");
                                    SetCharacterViewName( "ソレガシ" );
                                    Talk("よし、ひきあげるぞ。");
                                    WaitByEnter();

                                    SetValue( "クマドラゴン戦わず", true);

                                    NextSceneTalk( "data/script/talk_script_06.cs" );
                                    break;
                                case "とにかく倒してしまったほうがお金にならないか？":
                                    NextSceneBattle( "data/script/battle_02.cs" );
                                    break;
                                case "一緒に手伝おう":
                                    ClearText();
                                    SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                                    SetCharacterViewName( "トレンリー" );
                                    Talk("どうやら、さっき戦ってしまったので、警戒されているな");
                                    WaitByEnter();

                                    SetBGM( "data/bgm/Battle_Boss/15_一歩踏み出す勇気[Courage to step one step].mp3" );
                                    
                                    ClearText();
                                    SetCharacterImage("data/image/Characters/侍男_h2.png");
                                    SetCharacterViewName( "ソレガシ" );
                                    Talk("おい！\nクマドラゴンが現れて半魚人を襲っているぞ！");
                                    WaitByEnter();
                                    
                                    ClearText();
                                    SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                                    SetCharacterViewName( "レイチェル" );
                                    Talk("きゃ、こっちに来る！");
                                    WaitByEnter();
                                    
                                    NextSceneBattle( "data/script/battle_03.cs" );
                                    break;
                            }
                        }
                        // Todo : このあとどうしようか？
                        
                        break;
                }
            }
        }
        break;
}

Wait(1);
