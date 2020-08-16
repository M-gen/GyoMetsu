Name = "半魚人";
ImagePath = @"data/image/enm/53_竜の雛.png";
ImageScale = 1.0;
ImageSideMargin = 0.5;

HP = 500;

AddStatusBase("DMG", 5);
AddStatusBase("ACC", 5);
AddStatusBase("DOGE",4);
AddStatusBase("PROT",6);

AddActionSkill( "竜の爪" );
AddActionSkill( "鋼の竜の爪" );
AddActionSkill( "炎のブレス" );
AddActionSkill( "炎のブレス+" );

AddElementLingPart("獣");
AddElementLingPart("祈");
AddElementLingPart("樹");
AddElementLingPart("鉄");
AddElementLingPart("獣");
AddElementLingPart("祈");
