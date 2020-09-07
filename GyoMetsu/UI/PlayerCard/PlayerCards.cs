using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;

namespace GyoMetsu.UI.PlayerCard
{
    public class PlayerCards : ISprite
    {

        LayerSprite layer = new LayerSprite();
        List<PlayerCard> cards = new List<PlayerCard>();

        public Action<List<PlayerCard>> OnSelected;

        public enum UITargetSelectType
        {
            None,
            One,
            LineHorizontal,
        }
        public UITargetSelectType uITargetSelectType = UITargetSelectType.None;
        public int[] uITargetSelectTypePalams;

        PlayerCard lastMouseOnTargetCard;

        public PlayerCards()
        {
        }

        public void Add(  Data.Character character )
        {
            var num = cards.Count;
            var wMargin = 200;
            var y = 600;
            var x = 50;
            
            var sprite = new PlayerCard( new Vector2D(x + wMargin * num, y), character);
            layer.Add(sprite, 10);
            cards.Add(sprite);
        }

        public void SetupCardPos()
        {
            var t = Window.Instance.GameScreenSize;
            var num = cards.Count;
            var wMargin = 20;
            var cardWidth = 180;
            var y = 600;
            

            var allWidth = num * 180 + (num - 1) * wMargin;
            var x = (t.X - allWidth) / 2;

            for ( var i=0;i<num;i++ )
            {
                var card = cards[i];
                card.SetPosition( new Vector2D(x + (wMargin+ cardWidth )* i,y) );
            }
        }

        public void Update()
        {
            UI.PlayerCard.PlayerCard selectCard = null;
            foreach (var card in cards)
            {
                card.Update();
                if (card.isMouseOn)
                {
                    selectCard = card;
                }
            }

            switch (uITargetSelectType)
            {
                case UITargetSelectType.None:
                    foreach (var card in cards)
                    {
                        if (card == selectCard)
                        {
                            card.isMouseOnUITarget = true;
                            card.mouseOnTarget.IsDraw = true;
                        }
                        else
                        {
                            card.isMouseOnUITarget = false;
                            card.mouseOnTarget.IsDraw = false;
                        }
                    }
                    break;
                case UITargetSelectType.One:
                    foreach (var card in cards)
                    {
                        if (card == selectCard)
                        {
                            card.isMouseOnUITarget = true;
                            card.mouseOnTarget.IsDraw = true;
                        }
                        else
                        {
                            card.isMouseOnUITarget = false;
                            card.mouseOnTarget.IsDraw = false;
                        }
                    }
                    break;
                case UITargetSelectType.LineHorizontal:
                    {
                        // todo : 味方に列がないような作りになっている（いったんは、3人と人数が少ないので
                        if(selectCard!=null)
                        {
                            foreach (var card in cards)
                            {
                                card.isMouseOnUITarget = true;
                                card.mouseOnTarget.IsDraw = true;
                            }
                        }
                        else
                        {
                            foreach (var card in cards)
                            {
                                card.isMouseOnUITarget = false;
                                card.mouseOnTarget.IsDraw = false;
                            }
                        }

                        if ((lastMouseOnTargetCard != selectCard) && (selectCard != null))
                        {
                            var linePosition = selectCard.character.linePosition;
                            foreach (var card in cards)
                            {
                                if (card.character.linePosition == linePosition)
                                {
                                    card.mouseOnTargetFadeTimer = 0;
                                }
                            }
                        }
                        lastMouseOnTargetCard = selectCard;
                    }
                    break;
            }

            {
                var input = Emugen.Input.InputCore.Instance;
                if ((input.GetKeyEventType(Emugen.Input.InputCore.KeyEventCode.MouseLeftButton) == Emugen.Input.InputCore.KeyEventType.Up) && (selectCard != null))
                {
                    var selectCards = new List<PlayerCard>();
                    foreach (var card in cards)
                    {
                        if (card.isMouseOnUITarget) selectCards.Add(card);
                    }
                    if (OnSelected != null) OnSelected(selectCards);
                }
            }
        }

        public void PlayVoiceVictory()
        {

            var r = (new Random()).Next(cards.Count);
            var r2 = (new Random()).Next(2);
            var path = cards[r].character.voiceDirecotryPath + "/Victory_" + r2 + ".wav";
            var voice = new Emugen.Sound.SoundPlayer(path, 0.80f, false, Emugen.Sound.SoundPlayer.SoundType.Voice);

        }

        public override void Draw()
        {
            layer.Draw();
        }
    }
}


