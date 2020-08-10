SetBackGroundImagePath("data/image/bg/酒場.png");
SetBGM( "data/bgm/12_きのこ狩り [Mushroom hunting].mp3" );

SetCharacterImage("data/image/Characters/魔術師男_h720.png");
SetCharacterViewName( "トレンリー" );

Talk("やぁ、今日の酒はマズい、どういうことだい？");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/侍男_h2.png");
SetCharacterViewName( "ソレガシ" );
Talk("わからん、それがしにはさっぱりわからん");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/聖職者女_h720.png");
SetCharacterViewName( "レイチェル" );
Talk("いや、わかります。\n薄まっています、薄まっています、\nそれはもうどうしようもなく、湖面に浮かぶ木の葉のように、薄まっているのです！");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk("なんだ、腐ってるなら小銭稼ぎの依頼があるぞ");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/魔術師男_h720.png");
SetCharacterViewName( "トレンリー" );
Talk("なんでしょう？");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk("北西の川辺で、半魚人の集団が槍持ってあらわれたってよ。\n討伐依頼だ");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/侍男_h2.png");
SetCharacterViewName( "ソレガシ" );
Talk("なるほど、それがしにとっては朝飯前の依頼、さっそく……");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/聖職者女_h720.png");
SetCharacterViewName( "レイチェル" );
Talk("まちなさい！\nいいですか、ご主人、この依頼、ゴールが明かになっていないじゃないですか");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/魔術師男_h720.png");
SetCharacterViewName( "トレンリー" );
Talk("そうだ、ゴール、つまり金だ、依頼料だ！");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/聖職者女_h720.png");
SetCharacterViewName( "レイチェル" );
Talk("ちがいます！\nいったい、何匹の半魚人を倒せばいいか、わかっておりません！");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk("おまえら、薄めた酒でよくそこまで酔えるな");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/魔術師男_h720.png");
SetCharacterViewName( "トレンリー" );
Talk("おうよ、こちとら酔うことにゃ人生かけてるからな！");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk("まぁいい、今回の依頼は調査もかねて、\n成功の線引は5匹の討伐と残りの数を確認してくることで100ゴルド。\n失敗時は一匹につき10ゴルドだ");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/聖職者女_h720.png");
SetCharacterViewName( "レイチェル" );
Talk("そうなると安心して挑めますね");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/侍男_h2.png");
SetCharacterViewName( "ソレガシ" );
Talk("どうでもいい、最悪、狩った魚肉さばいてうりゃいい。");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk("じゃぁ、どうするんだ？");
WaitByEnter();

{
    var key = WaitChoise( new string[]{"トレンリー「受けるに決まっている」","レイチェル「引き受けましょう」","ソレガシ「任せてもらおう」"} );
    
    if (key.IndexOf("トレンリー")>=0) {
        ClearText();
        SetCharacterImage("data/image/Characters/魔術師男_h720.png");
        SetCharacterViewName( "トレンリー" );
        Talk("受けるに決まっている");
        WaitByEnter();
    }
    else if (key.IndexOf("レイチェル")>=0) {
        ClearText();
        SetCharacterImage("data/image/Characters/聖職者女_h720.png");
        SetCharacterViewName( "レイチェル" );
        Talk("引き受けましょう");
        WaitByEnter();
    }
    else if (key.IndexOf("ソレガシ")>=0) {
        ClearText();
        SetCharacterImage("data/image/Characters/侍男_h2.png");
        SetCharacterViewName( "ソレガシ" );
        Talk("任せてもらおう");
        WaitByEnter();
    }
}

ClearText();
SetCharacterImage("data/image/Characters/k0084_1(noise_scale)(Level1)(x4.000000).png");
SetCharacterViewName( "酒場の店長" );
Talk("では、楽しみに待っておるぞ");
WaitByEnter();

{
    var key = WaitChoise( new string[]{"現場へ向かう","街で準備をする"} );
    
    if (key == "現場へ向かう") {
        NextSceneTalk( "data/script/talk_script_03.cs" );
    }
    else if (key == "街で準備をする" ) {
        NextSceneTalk( "data/script/talk_script_02.cs" );
    }
}



Wait(1);
