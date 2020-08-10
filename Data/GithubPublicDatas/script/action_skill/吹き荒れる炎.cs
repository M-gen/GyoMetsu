
ActionSkill.Name = "吹き荒れる炎";
//ActionSkill.Cost = "樹樹理";
ActionSkill.Cost = "1";

SetMain( "横列 0 0 1", "命中 4 2 6", "HPダメージ 6 4 6", "魔術 炎 風" );
AddSub(  "対象", "確率 30.00", "防御ペナルティ 3 1000", "魔術" );