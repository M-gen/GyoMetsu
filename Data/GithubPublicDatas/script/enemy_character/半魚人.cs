Name = "半魚人";
ImagePath = @"data/image/enm/47_サハギン_scale0x3.png";
ImageScale = 1.0;
ImageSideMargin = 0.5;

HP = 70;

AddStatusBase("DMG", 3);
AddStatusBase("ACC", 6);
AddStatusBase("DOGE",6);
AddStatusBase("PROT",3);

//AddActionSkill( "なぐる", "1", 10, "攻撃" );
AddActionSkill( "重い一撃" );

AddElementLingPart("獣");
AddElementLingPart("祈");
AddElementLingPart("樹");
AddElementLingPart("鉄");
AddElementLingPart("理");
AddElementLingPart("祈");
