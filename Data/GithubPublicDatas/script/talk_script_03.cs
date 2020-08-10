SetBackGroundImagePath("data/image/bg/背景.png");
SetBGM( "data/bgm/13_囁き[Whisper].mp3" );

// SetCharacterImage("data/image/Characters/聖職者女_h720.png");


SetCharacterImage("data/image/Characters/侍男_h2.png");
SetCharacterViewName( "ソレガシ" );
Talk("もうすぐ川辺だな、むむ、一匹、おるではないか！？\nこちらには気づいていないようだが");
WaitByEnter();


ClearText();
SetCharacterImage("data/image/enm/47_サハギン.png");
SetCharacterViewName( "半魚人" );
Talk("ぎょーーーーーぎょーーーーー");
WaitByEnter();

var isLoop = true;
var isSelect1 = false;

while( isLoop ){
    var key = "";
    if (!isSelect1) {
        key = WaitChoise( new string[]{
                "周囲の半魚人を探す",
                "ひっそり近づいて倒す",
                "どうどうと攻撃する",
                "会話を試みる"
            } );
    }
    else {
        key = WaitChoise( new string[]{
                "ひっそり近づいて倒す",
                "どうどうと攻撃する",
                "会話を試みる"
            } );
    }

    switch (key)
    {
        case "周囲の半魚人を探す":
            ClearText();
            SetCharacterImage("data/image/Characters/魔術師男_h720.png");
            SetCharacterViewName( "トレンリー" );
            Talk("他も同じような状態だ");
            WaitByEnter();
            
            isSelect1 = true;

            break;
        case "ひっそり近づいて倒す":

            PlaySound("data/se/SE_HitBlow_M.wav", 0.6f);

            ClearText();
            SetCharacterImage("data/image/Characters/聖職者女_h720.png");
            SetCharacterViewName( "レイチェル" );
            Talk("えいっ！");
            WaitByEnter();

            PlaySound("data/se/SE_KO.wav", 0.6f);
            
            ClearText();
            SetCharacterImage("data/image/Characters/侍男_h2.png");
            SetCharacterViewName( "ソレガシ" );
            Talk("今のは拙者の役目でござろう");
            WaitByEnter();
            
            NextSceneTalk( "data/script/talk_script_04.cs" );

            SetValue( "倒し方", "ひっそり");
            SetValue( "半魚人討伐数", GetValueDefault<int>("半魚人討伐数", 0 )+1 );
            
            break;
        case "どうどうと攻撃する":
            ClearText();
            SetCharacterImage("data/image/Characters/侍男_h2.png");
            SetCharacterViewName( "ソレガシ" );
            Talk("ようよう我こそはソレガシ、いざ尋常に勝負！");
            WaitByEnter();
            
            isLoop = false;
            
            SetValue( "倒し方", "どうどうと");
            SetValue( "半魚人討伐数", GetValueDefault<int>("半魚人討伐数", 0 )+1 );

            NextSceneBattle( "data/script/battle_01.cs" );

            break;
        case "会話を試みる":

            if ( GetValueDefault<bool>("購入『半魚人とおしゃべり辞典 序』", false ) ) {

                ClearText();
                SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                SetCharacterViewName( "トレンリー" );
                Talk("ギョギョーギョ（よう、こんなところで何してるんだ？）");
                WaitByEnter();
                            
                ClearText();
                SetCharacterImage("data/image/enm/47_サハギン.png");
                SetCharacterViewName( "半魚人" );
                Talk("ぎょっぎょーぎょーー（見張りだ）");
                WaitByEnter();

                ClearText();
                SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                SetCharacterViewName( "トレンリー" );
                Talk("ギョギョーギョ（奥で何をしている？）");
                WaitByEnter();
                            
                ClearText();
                SetCharacterImage("data/image/enm/47_サハギン.png");
                SetCharacterViewName( "半魚人" );
                Talk("ぎょっぎょーーぎょーぎょー（川の掃除だよ、おまえら人間がゴミを捨てていくからな）");
                WaitByEnter();
                

                ClearText();
                SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                SetCharacterViewName( "トレンリー" );
                Talk("ギョギョーギョ（そいつはすまねぇな。手伝ってもいいか？）");
                WaitByEnter();
                            
                ClearText();
                SetCharacterImage("data/image/enm/47_サハギン.png");
                SetCharacterViewName( "半魚人" );
                Talk("ぎょっ（もちろんだ！）");
                WaitByEnter();

                isLoop = false;
                
                SetValue( "倒し方", "会話");

                //NextSceneBattle( "data/script/battle_01_b.cs" );
                NextSceneTalk( "data/script/talk_script_05_b.cs" );

            }
            else {

                ClearText();
                SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                SetCharacterViewName( "トレンリー" );
                Talk("ギョ、ギョギョル、ギョギョギョル、ギョギョギョギョギョ");
                WaitByEnter();
                            
                ClearText();
                SetCharacterImage("data/image/enm/47_サハギン.png");
                SetCharacterViewName( "半魚人" );
                Talk("ぎょう！！！");
                WaitByEnter();

                ClearText();
                SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                SetCharacterViewName( "レイチェル" );
                Talk("ダメじゃない、襲ってきた！");
                WaitByEnter();

                isLoop = false;
                
                SetValue( "倒し方", "どうどうと");
                SetValue( "半魚人討伐数", GetValueDefault<int>("半魚人討伐数", 0 )+2 );

                NextSceneBattle( "data/script/battle_01_b.cs" );
            }
            break;
    }
}

Wait(1);
