SetBackGroundImagePath("data/image/bg/酒場.png");
SetBGM( "data/bgm/12_きのこ狩り [Mushroom hunting].mp3" ); // Todo : BGM をエンディング用に変えたい

ClearText();
SetCharacterImage("data/image/Characters/侍男_h2.png");
SetCharacterViewName( "ソレガシ" );
Talk("ただいま戻ったぞ！");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk("おぉ、成果はどうだった？");
WaitByEnter();

var result = GetValueDefault<int>("半魚人討伐数", 0 );

ClearText();
SetCharacterImage("data/image/Characters/魔術師男_h720.png");
SetCharacterViewName( "トレンリー" );
Talk($"{result}匹倒したぞ。\nほら。");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk("どれどれ……");
WaitByEnter();

// Todo : 倒した数に応じてエンディングを分岐したらいい
// 成功の線引は5匹の討伐と残りの数を確認してくることで100ゴルド。\n失敗時は一匹につき10ゴルドだ");

var resultGold = 0;
if ( result >= 5 ) {
    resultGold = 100;
}
else {
    resultGold = 10 * result;
}

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk($"よし、それじゃ報酬の{resultGold}ゴルドだ");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/聖職者女_h720.png");
SetCharacterViewName( "レイチェル" );
Talk("ありがとうございます");
WaitByEnter();

if ( result >= 5 ) {
    ClearText();
    SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
    SetCharacterViewName( "酒場の店長" );
    Talk("なかなか、良かったじゃないか。");
    WaitByEnter();

    ClearText();
    SetCharacterImage("data/image/Characters/侍男_h2.png");
    SetCharacterViewName( "ソレガシ" );
    Talk("良くはなかった……クマドラゴンが出たでござる");
    WaitByEnter();

    ClearText();
    SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
    SetCharacterViewName( "酒場の店長" );
    Talk("そうか、大変だったな。");
    WaitByEnter();
}
else {

    if ( GetValueDefault<string>( "倒し方", "")=="会話" ) {
//                SetValue( "倒し方", "会話");
        ClearText();
        SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
        SetCharacterViewName( "酒場の店長" );
        Talk("少ないようだが、何かあったのか？");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/魔術師男_h720.png");
        SetCharacterViewName( "トレンリー" );
        Talk("半魚人は川を掃除しているだけだったのだが、クマドラゴンが出てきてな。");
        WaitByEnter();
        
        ClearText();
        SetCharacterImage("data/image/Characters/聖職者女_h720.png");
        SetCharacterViewName( "レイチェル" );
        Talk("半魚人さん達と一緒にクマドラゴンをやっつけたのよ。");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/侍男_h2.png");
        SetCharacterViewName( "ソレガシ" );
        Talk("クマドラゴン分の報酬はないでござるか？");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
        SetCharacterViewName( "酒場の店長" );
        Talk("無い。");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/侍男_h2.png");
        SetCharacterViewName( "ソレガシ" );
        Talk("なら、クマドラゴンの肉をさばいて売るか……");
        WaitByEnter();
    
    }
    else if ( !GetValueDefault<bool>( "クマドラゴン戦わず", false) ) {
        ClearText();
        SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
        SetCharacterViewName( "酒場の店長" );
        Talk("少ないようだが、何かあったのか？");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/魔術師男_h720.png");
        SetCharacterViewName( "トレンリー" );
        Talk("半魚人は川を掃除しているだけだったのだが、クマドラゴンが出てきてな。");
        WaitByEnter();
        
        ClearText();
        SetCharacterImage("data/image/Characters/聖職者女_h720.png");
        SetCharacterViewName( "レイチェル" );
        Talk("みんなクマドラゴンが食べちゃったの。");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/侍男_h2.png");
        SetCharacterViewName( "ソレガシ" );
        Talk("クマドラゴン分の報酬はないでござるか？");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
        SetCharacterViewName( "酒場の店長" );
        Talk("無い。");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/侍男_h2.png");
        SetCharacterViewName( "ソレガシ" );
        Talk("なら、クマドラゴンの肉をさばいて売るか……");
        WaitByEnter();
    }
    else {
        ClearText();
        SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
        SetCharacterViewName( "酒場の店長" );
        Talk("少ないようだが、何かあったのか？");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/魔術師男_h720.png");
        SetCharacterViewName( "トレンリー" );
        Talk("半魚人は川を掃除しているだけだったのだ。");
        WaitByEnter();
        
        ClearText();
        SetCharacterImage("data/image/Characters/聖職者女_h720.png");
        SetCharacterViewName( "レイチェル" );
        Talk("害は無かったみたいね。");
        WaitByEnter();

        ClearText();
        SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
        SetCharacterViewName( "酒場の店長" );
        Talk("なるほど、騒ぐ必要もなかったのかもしれんな。");
        WaitByEnter();

    }
}

NextSceneTitle();
Wait(1);
