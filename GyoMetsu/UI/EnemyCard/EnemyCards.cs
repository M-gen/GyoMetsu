using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;

namespace GyoMetsu.UI.EnemyCard
{
    public class EnemyCards : ISprite
    {
        LayerSprite layer = new LayerSprite();
        List<EnemyCard> cards = new List<EnemyCard>();
        List<EnemyCard> outCards = new List<EnemyCard>();

        public Action<List<EnemyCard>> OnSelected;
        public EnemyCard SelectedEnemyCard;

        public enum UITargetSelectType
        {
            None,
            One,
            LineHorizontal,
        }
        public UITargetSelectType uITargetSelectType = UITargetSelectType.None;
        public int[] uITargetSelectTypePalams;

        EnemyCard lastMouseOnTargetCard;

        public void Add( Data.Character character)
        {
            var num = cards.Count;
            var wMargin = 200;
            var y = 600;
            var x = 50;

            var bmp = new System.Drawing.Bitmap(character.imagePath);
            var sprite = new EnemyCard(bmp, new Vector2D(x + wMargin * num, y), character.Name, character);
            layer.Add(sprite, 10);
            cards.Add(sprite);
            character.enemyCard = sprite;
            SetupCardPos();
        }

        public void Del(EnemyCard card )
        {
            cards.Remove(card);
            outCards.Add(card);

            card.damageTimer = 0;
            card.step = EnemyCard.Step.Lost;
            //layer.Del(card);
        }

        public void SetupCardPos()
        {
            // Todo : 敵の表示位置の調整がうまくいかない
            var t = Window.Instance.GameScreenSize;
            var num = cards.Count;
            var wMargin = 20;
            //var cardWidth = 180;
            var y1 = 600;

            var allWidth = (num - 1) * wMargin;
            for (var i = 0; i < num; i++)
            {
                var card = cards[i];
                //allWidth += (int)(card.cardImage.Width * card.character.imageScale);
                allWidth += (int)(card.cardImage.Width * card.character.imageScale * card.character.imageSideMargin);
            }

            var x = (t.X - allWidth) / 2;
            //var x = x1;
            for (var i = 0; i < num; i++)
            {
                var card = cards[i];
                var y = y1 - (card.cardImage.Height * card.character.imageScale);
                var x2 = x - card.cardImage.Width * card.character.imageScale * card.character.imageSideMargin / 2;
                card.SetPosition(new Vector2D(x2 , y));

                x += (int)(card.cardImage.Width * card.character.imageScale * card.character.imageSideMargin) + wMargin;
            }
        }

        public int Count { get { return cards.Count; } }

        public void Update()
        {
            {
                var tmp = new List<EnemyCard>();
                foreach (var card in outCards)
                {
                    card.Update();
                    if(card.IsLost())
                    {
                        layer.Del(card);
                    }
                    else
                    {
                        tmp.Add(card);
                    }
                }
                outCards = tmp;
            }

            switch (uITargetSelectType)
            {
                case UITargetSelectType.None:
                    {
                        var mousePosition = Emugen.Input.InputCore.Instance.mousePosition;
                        var tmp = default(EnemyCard);
                        var tmpRange = -1.0;

                        foreach (var card in cards)
                        {
                            card.isMouseOnUITarget = false;
                            card.mouseOnTarget.IsDraw = false;
                            if (card.isMouseOn)
                            {
                                var cr = card.Rect;
                                var range = Math.Sqrt(Math.Pow(mousePosition.X - (cr.Position.X + cr.Size.X / 2), 2) + Math.Pow(mousePosition.Y - (cr.Position.Y + cr.Size.Y / 2), 2));
                                if ((tmp == null) || (range < tmpRange))
                                {
                                    tmp = card;
                                    tmpRange = range;
                                }
                            }
                        }
                        if (tmp != null)
                        {
                            tmp.isMouseOnUITarget = true;
                            tmp.mouseOnTarget.IsDraw = true;
                        }
                    }
                    break;
                case UITargetSelectType.One:
                    {
                        var mousePosition = Emugen.Input.InputCore.Instance.mousePosition;
                        var tmp = default(EnemyCard);
                        var tmpRange = -1.0;

                        foreach (var card in cards)
                        {
                            card.isMouseOnUITarget = false;
                            card.mouseOnTarget.IsDraw = false;
                            if (card.isMouseOn)
                            {
                                var cr = card.Rect;
                                var range = Math.Sqrt(Math.Pow(mousePosition.X - (cr.Position.X + cr.Size.X / 2), 2) + Math.Pow(mousePosition.Y - (cr.Position.Y + cr.Size.Y / 2), 2));
                                if ((tmp == null) || (range < tmpRange))
                                {
                                    tmp = card;
                                    tmpRange = range;
                                }
                            }
                        }
                        if (tmp != null)
                        {
                            tmp.isMouseOnUITarget = true;
                            tmp.mouseOnTarget.IsDraw = true;
                        }
                    }
                    break;
                case UITargetSelectType.LineHorizontal:
                    {
                        var mousePosition = Emugen.Input.InputCore.Instance.mousePosition;
                        var tmp = default(EnemyCard);
                        var tmpRange = -1.0;

                        foreach (var card in cards)
                        {
                            card.isMouseOnUITarget = false;
                            card.mouseOnTarget.IsDraw = false;
                            if (card.isMouseOn)
                            {
                                var cr = card.Rect;
                                var range = Math.Sqrt(Math.Pow(mousePosition.X - (cr.Position.X + cr.Size.X / 2), 2) + Math.Pow(mousePosition.Y - (cr.Position.Y + cr.Size.Y / 2), 2));
                                if ((tmp == null) || (range < tmpRange))
                                {
                                    tmp = card;
                                    tmpRange = range;
                                }
                            }
                        }
                        if (tmp != null)
                        {
                            tmp.isMouseOnUITarget = true;
                            tmp.mouseOnTarget.IsDraw = true;

                            var linePosition = tmp.character.linePosition;
                            foreach (var card in cards)
                            {
                                if (card.character.linePosition==linePosition)
                                {
                                    card.isMouseOnUITarget = true;
                                    card.mouseOnTarget.IsDraw = true;
                                }
                            }
                        }

                        if ( (lastMouseOnTargetCard!= tmp) && (tmp!=null) )
                        {
                            var linePosition = tmp.character.linePosition;
                            foreach (var card in cards)
                            {
                                if (card.character.linePosition == linePosition)
                                {
                                    card.mouseOnTargetFadeTimer = 0;
                                }
                            }
                        }
                        lastMouseOnTargetCard = tmp;

                    }
                    break;
            }

            var selectCard = default(UI.EnemyCard.EnemyCard);
            foreach (var card in cards)
            {
                card.Update();
                if (card.isMouseOnUITarget)
                {
                    selectCard = card;
                }
            }

            {
                var input = Emugen.Input.InputCore.Instance;
                if (input.GetKeyEventType(Emugen.Input.InputCore.KeyEventCode.MouseLeftButton) == Emugen.Input.InputCore.KeyEventType.Up)
                {
                    if (selectCard != null)
                    {
                        var selectCards = new List<EnemyCard>();
                        foreach ( var card in cards )
                        {
                            if (card.isMouseOnUITarget) selectCards.Add(card);
                        }
                        if (OnSelected != null) OnSelected(selectCards);
                    }
                }
            }

        }

        public override void Draw()
        {
            layer.Draw();
        }
    }
}
