Name = "半魚人";
ImagePath = @"data/image/enm/47_サハギン_scale0x3.png";
ImageSideMargin = 0.5;

HP = 65;

AddStatusBase("DMG", 3);
AddStatusBase("ACC", 6);
AddStatusBase("DOGE",6);
AddStatusBase("PROT",3);

AddActionSkill( "重い一撃" );
AddActionSkill( "槍の一突き" );

{
    var type = Random.Next(3);
    switch(type) {
        case 0:
            DefalutStockElement = "";
            AddElementLingPart("獣");
            AddElementLingPart("祈");
            AddElementLingPart("樹");
            AddElementLingPart("鉄");
            AddElementLingPart("理");
            AddElementLingPart("祈");
            break;
        case 1:
            DefalutStockElement = "鉄";
            AddElementLingPart("樹");
            AddElementLingPart("獣");
            AddElementLingPart("獣樹");
            break;
        case 2:
            DefalutStockElement = "";
            AddElementLingPart("鉄");
            AddElementLingPart("獣");
            AddElementLingPart("樹");
            AddElementLingPart("祈");
            break;
        default:
            DefalutStockElement = "";
            AddElementLingPart("鉄");
            AddElementLingPart("鉄");
            AddElementLingPart("理");
            AddElementLingPart("祈");
            break;
    }
}
