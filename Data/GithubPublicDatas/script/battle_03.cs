
SetBackGroundImagePath( "data/image/bg/背景.png" );
SetBGM( "data/bgm/Battle_Boss/15_一歩踏み出す勇気[Courage to step one step].mp3" );

AddPlayerCharacter("data/script/player_character/ソレガシ.cs");
AddPlayerCharacter("data/script/player_character/トレンリー.cs");
AddPlayerCharacter("data/script/player_character/レイチェル.cs");

AddEnemy("data/script/enemy_character/クマドラゴン.cs");

//NextTalkScene("data/script/talk_script.txt");
NextTalkScene( "data/script/talk_script_06.cs" ); // Todo : コマンドが NextTalkSceneと NectScneneTalkの2つある。。。