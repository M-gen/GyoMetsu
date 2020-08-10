SetBackGroundImagePath("data/image/bg/背景.png");
SetBGM( "data/bgm/13_囁き[Whisper].mp3" );

SetValue( "半魚人討伐数", GetValueDefault<int>("半魚人討伐数", 0 )+4 );

ClearText();
SetCharacterImage("data/image/Characters/魔術師男_h720.png");
SetCharacterViewName( "トレンリー" );
Talk("よし、なんとか倒したな。");
WaitByEnter();

SetBGM( "data/bgm/Battle_Boss/15_一歩踏み出す勇気[Courage to step one step].mp3" );

ClearText();
SetCharacterImage("data/image/Characters/侍男_h2.png");
SetCharacterViewName( "ソレガシ" );
Talk("おい、クマドラゴンだ！\n半魚人につられてやってきたんじゃないか！");
WaitByEnter();

ClearText();
SetCharacterImage("data/image/Characters/聖職者女_h720.png");
SetCharacterViewName( "レイチェル" );
Talk("きゃ、こっちに来る！");
WaitByEnter();

NextSceneBattle( "data/script/battle_03.cs" );
Wait(1);