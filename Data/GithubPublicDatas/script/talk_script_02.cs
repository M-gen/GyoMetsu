using System.Collections.Generic;

SetBackGroundImagePath("data/image/bg/街並み.png");
SetBGM( "data/bgm/12_きのこ狩り [Mushroom hunting].mp3" );

// SetCharacterImage("data/image/Characters/聖職者女_h720.png");

SetCharacterImage("data/image/Characters/魔術師男_h720.png");
SetCharacterViewName( "トレンリー" );

Talk("さて、まずはどうしようか？");
WaitByEnter();

var talkCount = 0;
var shopCount = 0;

while (true){

    //var key = "";

    var choiseList = new List<string>();

    if (talkCount<3) choiseList.Add("街人に話しかける");
    if (shopCount<2) choiseList.Add("道具屋で買い物をする");
    choiseList.Add("現場に向かう");

    var key = WaitChoise( choiseList.ToArray() );

    if (key == "街人に話しかける" ) {

        switch(talkCount)
        {
            case 0:
                ClearText();
                SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                SetCharacterViewName( "レイチェル" );
                Talk("すいません、近くの川辺で半魚人が出たそうなんですけど");
                WaitByEnter();

                ClearText();
                SetCharacterImage("data/image/Characters/k0452_3.png");
                SetCharacterViewName( "村人" );
                Talk("そうなんだよ、実はさ。\n明後日バーベキューしようと計画していたんだが、これじゃ難しいのかな。\n非常に残念だ。");
                WaitByEnter();
                break;

            case 1:
                ClearText();
                SetCharacterImage("data/image/Characters/侍男_h2.png");
                SetCharacterViewName( "ソレガシ" );
                Talk("すまぬが、最近変わったことはなかったか？");
                WaitByEnter();

                ClearText();
                SetCharacterImage("data/image/Characters/k0452_2.png");
                SetCharacterViewName( "村人" );
                Talk("そうだな、なんかクマドラゴンの遠吠えを聞いた話だ。おっかないね");
                WaitByEnter();
                break;

            case 2:
                ClearText();
                SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                SetCharacterViewName( "トレンリー" );
                Talk("そこのかた、お茶でも飲みませんか？");
                WaitByEnter();

                ClearText();
                SetCharacterImage("data/image/Characters/k0482_12.png");
                SetCharacterViewName( "村人" );
                Talk("なぜ、私の居場所がわかった、失礼！");
                WaitByEnter();
                break;
        }

        talkCount++;
    }
    else if (key == "道具屋で買い物をする" ) {
        
        switch(shopCount)
        {
            case 0:
                ClearText();
                SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                SetCharacterViewName( "レイチェル" );
                Talk("ごめんください、薬草はありませんか？");
                WaitByEnter();

                ClearText();
                SetCharacterImage("data/image/Characters/k0897_5.png");
                SetCharacterViewName( "商人" );
                Talk("いいのが入ったよ、お安くしとくよ！");
                WaitByEnter();
                
                ClearText();
                SetCharacterImage("data/image/Characters/聖職者女_h720.png");
                SetCharacterViewName( "レイチェル" );
                Talk("ありがとうございます");
                WaitByEnter();
                break;
            case 1:
                ClearText();
                SetCharacterImage("data/image/Characters/k0897_5.png");
                SetCharacterViewName( "商人" );
                Talk("お、そうだ『半魚人とおしゃべり辞典 序』が手に入ったよ？\n買っていくかい？");
                WaitByEnter();

                ClearText();
                SetCharacterImage("data/image/Characters/魔術師男_h720.png");
                SetCharacterViewName( "トレンリー" );
                Talk("ぜひとも買っておこう");
                WaitByEnter();

                SetValue( "購入『半魚人とおしゃべり辞典 序』", true);
                break;
        }

        shopCount++;
    }
    else if (key == "現場に向かう") {
        NextSceneTalk( "data/script/talk_script_03.cs" );
    }
}

Wait(1);
