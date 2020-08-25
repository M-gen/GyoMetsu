
SetBackGroundImagePath( "data/image/bg/背景.png" );
SetBGM( "data/bgm/Battle_Boss/15_一歩踏み出す勇気[Courage to step one step].mp3" );

AddEnemy("data/script/enemy_character/クマドラゴン.cs");

//NextTalkScene("data/script/talk_script.txt");
NextTalkScene( "data/script/talk_script_06.cs" ); // Todo : コマンドが NextTalkSceneと NectScneneTalkの2つある。。。