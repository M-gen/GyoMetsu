Name = "半魚人";
ImagePath = @"data/image/enm/53_竜の雛.png";
ImageScale = 0.8;
ImageSideMargin = 0.5;

HP = 70;
AddStatusBase("DMG", 3);
AddStatusBase("ACC", 6);
AddStatusBase("DOGE",6);
AddStatusBase("PROT",3);

//AddActionSkill( "なぐる", "1", 5, "攻撃" );
AddActionSkill( "重い一撃" );

AddElementLingPart("獣");
AddElementLingPart("祈");
AddElementLingPart("樹");
AddElementLingPart("鉄");
AddElementLingPart("理");
AddElementLingPart("祈");
