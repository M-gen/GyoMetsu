using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Image.Primitive;

namespace GyoMetsu.UI.SkillCard
{
    public class SkillCards : ISprite
    {
        LayerSprite layer = new LayerSprite();
        List<SkillCard> cards = new List<SkillCard>();

        public enum Step
        {
            None,
            ShowPlayerCharacterSkills,
        }
        Step step = Step.None;

        PlaneSprite BackGroundSprite;

        public Action<SkillCard> OnSelected;
        public Action OnClose;

        public SkillCards()
        {
            {
                var sprite = new PlaneSprite(new Rect(new Vector2D(0,0), new Vector2D(1600,900)), new Color(0.5,0,0,0));
                sprite.IsDraw = false;
                layer.Add(sprite, 0);
                BackGroundSprite = sprite;
            }
        }

        public void Add(System.Drawing.Bitmap bmp, Data.Character character, Data.ActionSkill actionSkill)
        {
            var texture = new Texture(bmp);
            var w = 180;
            //var w = 160;
            var h = (int)((double)bmp.Height / (double)bmp.Width * w);
            var x = 1;
            var y = 1;
            var sprite = new SkillCard(bmp, new Vector2D(x, y), actionSkill.Name, actionSkill.Cost, character, actionSkill);
            layer.Add(sprite, 10);
            cards.Add(sprite);
            sprite.Update();

            sprite.SetEnable(false);
        }

        public void SetupCardPos()
        {
            var t = Window.Instance.GameScreenSize;
            var num = cards.Count;
            var wMargin = 0;
            var cardWidth = 180;
            var cardHight = 300;
            var cardNum = 6;

            //var allWidth = num * cardWidth + (num - 1) * wMargin;
            var allWidth = cardNum * cardWidth + (cardNum - 1) * wMargin;
            var x = (t.X - allWidth) / 2;
            var y = 80;

            for (var i = 0; i < num; i++)
            {
                var card = cards[i];
                card.SetPosition(new Vector2D(x + (wMargin + cardWidth) * (i % cardNum), y + cardHight * (i / cardNum)));
            }
        }

        public void Update()
        {
            SetupCardPos();

            SkillCard selectCard = null;
            foreach (var card in cards)
            {
                card.Update();
                if (card.isMouseOn)
                {
                    selectCard = card;
                }
            }

            {
                var input = Emugen.Input.InputCore.Instance;
                if(input.GetKeyEventType(Emugen.Input.InputCore.KeyEventCode.MouseLeftButton) == Emugen.Input.InputCore.KeyEventType.Up)
                {
                    if (selectCard != null)
                    {
                        if (OnSelected!=null) OnSelected(selectCard);
                        Close();
                    }
                    else
                    {
                        if (OnClose != null) OnClose();
                        Close();
                    }
                }
            }
        }

        private void Close()
        {
            foreach (var skill in cards)
            {
                layer.Del(skill);
            }
            cards.Clear();
            BackGroundSprite.IsDraw = false;
            this.step = Step.None;
        }

        public override void Draw()
        {
            layer.Draw();
        }

        public void SetStep( Step step,  object obj )
        {
            switch(step)
            {
                case Step.ShowPlayerCharacterSkills:
                    {
                        var character = (Data.Character)obj;
                        foreach(var action in character.Actions )
                        {
                            //Add(new System.Drawing.Bitmap(Config.MainConfig.Card.SkillDefault), character, action);
                            Add(new System.Drawing.Bitmap(action.ImagePath), character, action);
                        }
                        BackGroundSprite.IsDraw = true;
                    }
                    break;
            }

            this.step = step;
        }
    }
}
