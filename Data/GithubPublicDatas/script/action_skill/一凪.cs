
ActionSkill.Name = "一凪";
ActionSkill.Cost = "樹";
ActionSkill.ImagePath = "data/image/skill/スキルカード_一凪.png";

SetMain( "単体 0", "命中 13 2 6", "HPダメージ 11 2 6", "武器" );
AddSub(  "自身", "自動成功", "命中ボーナス 1 1000", "武術" );