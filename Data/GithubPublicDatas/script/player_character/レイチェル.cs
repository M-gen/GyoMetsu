Name = "レイチェル";
ImagePath = @"data/image/Characters/レイチェル.png";

HP = 45;

AddStatusBase("DMG",  2);
AddStatusBase("ACC",  4);
AddStatusBase("DOGE", 4);
AddStatusBase("PROT", 2);

AddActionSkill( "ひのきの杖" );
AddActionSkill( "癒やしの水" );
AddActionSkill( "癒やしの水+" );
AddActionSkill( "ちいさな天罰" );
AddActionSkill( "女神の輝き" );

DefalutStockElement = "";
AddElementLingPart("祈樹");
AddElementLingPart("鉄");
AddElementLingPart("樹");
AddElementLingPart("理");
AddElementLingPart("祈");
