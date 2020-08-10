using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;

namespace Emugen.OpenTK.Sprite
{
    public class TextSprite : ISprite, IDisposable
    {
        public Emugen.Image.Primitive.Rect Rect;
        //public Emugen.Image.Primitive.Vector2D Position;
        public Emugen.Image.Primitive.Color Color;
        public Emugen.OpenTK.View.IView view;

        string text;
        string fontPath;

        LayerSprite layer = new LayerSprite();

        class Item
        {
            public string      text;
            public Rect        defaultRect; // 基準となる矩形、素の状態で並べたときの情報
            public Rect        singleRect;  // 単体での矩形
            public ImageSprite imageSprite;
            public System.Drawing.Bitmap bmp;
            public Color color = new Color(1,1,1,1);
        }
        List<Item> items = new List<Item>();
        int margin = 3;
        int lineTextMargin = 0;

        double fadeTimer;
        double fadeTimerAdd;
        public Action OnShowEnd;

        //private Vector2D position;

        private class SubItem
        {
            public Rect Rect;
            public Rect SingleRect;
        }

        public enum Type {
            Separation,
            Lump,
        }
        Type type;

        public TextSprite( string text, Font efont, Emugen.Image.Primitive.Vector2D position, Type type = Type.Separation, Emugen.OpenTK.View.IView view = null, int fadeTimer = -1, int fadeTimerAdd = 100)
        {
            //this.position = position;
            this.Rect = new Rect(position, new Vector2D(0, 0));
            this.fadeTimer = fadeTimer;
            this.fadeTimerAdd = fadeTimerAdd;
            Color = new Color(1,1,1,1);

            switch (type)
            {
                case Type.Separation:
                    _TextSprite_Separation(text, efont, position, view, fadeTimer, fadeTimerAdd);
                    break;
                case Type.Lump:
                    break;
            }


        }

        // Todo : こちらの文字描画に切り替えたいのだが...
        private void _TextSprite_Separation(string text, Font efont, Emugen.Image.Primitive.Vector2D position, Emugen.OpenTK.View.IView view = null, int fadeTimer = -1, int fadeTimerAdd = 100)
        {
            //this.position = position;
            //this.fadeTimer = fadeTimer;
            //this.fadeTimerAdd = fadeTimerAdd;
            this.lineTextMargin = efont.lineTextMargin;

            var frameSize = 0.0;
            var frameSizeMax = 0.0;
            var rfontFrames = new List<Font.FontFrame>();
            foreach (var frame in efont.fontFrames)
            {
                frameSizeMax += frame.size / 2;
                rfontFrames.Insert(0, frame);
            }
            frameSize = frameSizeMax;

            // Todo : フォントフレームの幅分だけマージンの対応が必要
            margin = (int)System.Math.Floor(frameSize);
            var saveMargin = 2;

            // Todo : Test用
            //lineTextMargin = 50;

            var k = 0;
            var tmp = "";

            var subItems = new List<SubItem>();
            foreach (var t in text)
            {
                tmp += t;
                var subItem = new SubItem();
                var a = Emugen.Image.Drawing.DrawText.Rect(t.ToString(), efont, new System.Drawing.PointF(0, 0));
                var b = Emugen.Image.Drawing.DrawText.Rect(tmp, efont, new System.Drawing.PointF(0, 0));
                var x2 = b.Position.X + b.Size.X;
                var y2 = b.Position.Y + b.Size.Y;
                subItem.Rect = new Rect(new Vector2D(x2 - a.Size.X, a.Position.Y), new Vector2D(a.Size.X, a.Size.Y));
                subItem.SingleRect = new Rect(a);
                subItems.Add(subItem);
            }

            k = 0;
            tmp = "";
            var tmpX = 0;
            foreach ( var t in text)
            {
                var subItem = subItems[k];
                tmp += t;
                var rectB = Emugen.Image.Drawing.DrawText.Rect(t.ToString(), efont, new System.Drawing.PointF(0, 0));

                var item = new Item();
                item.text = t.ToString();
                //item.bmp = new System.Drawing.Bitmap((int)(rectB.Position.X+rectB.Size.X + margin * 2), (int)(rectB.Position.Y+rectB.Size.Y + margin * 2));
                item.bmp = new System.Drawing.Bitmap((int)(subItem.Rect.Size.X + margin * 2 + saveMargin), (int)(subItem.Rect.Size.Y + margin * 2 + saveMargin));
                //item.bmp = new System.Drawing.Bitmap((int)(rectB.Position.X + rectB.Size.X), (int)(rectB.Position.Y + rectB.Size.Y));

                
                if (k == 0)
                {
                    var rectA = Emugen.Image.Drawing.DrawText.Rect(tmp, efont, new System.Drawing.PointF(0, 0));
                    //item.defaultRect = new Rect(new Vector2D(0, 0), new Vector2D(128, 128));
                    //item.defaultRect = new Rect(new Vector2D(rectA.Position.X, rectA.Position.Y), new Vector2D(rectA.Size.X, rectA.Size.Y));
                    item.defaultRect = new Rect(new Vector2D(subItem.Rect.Position.X,subItem.Rect.Position.Y), new Vector2D(item.bmp.Width, item.bmp.Height));
                }
                else
                {
                    var rectA = Emugen.Image.Drawing.DrawText.Rect(tmp, efont, new System.Drawing.PointF(0, 0));
                    item.defaultRect = new Rect(new Vector2D(subItem.Rect.Position.X, subItem.Rect.Position.Y), new Vector2D(item.bmp.Width, item.bmp.Height));
                    //item.defaultRect = new Rect(new Vector2D( margin + rectA.Position.X + rectA.Size.X - rectB.Size.X, margin), new Vector2D(item.bmp.Width, item.bmp.Height));
                    //item.defaultRect = new Rect(new Vector2D( margin, margin), new Vector2D(item.bmp.Width, item.bmp.Height));
                    //item.defaultRect = new Rect(new Vector2D(rectA.Position.X + rectA.Size.X - rectA.Size.X, 10), new Vector2D(128, 128));
                    //item.defaultRect = new Rect(new Vector2D(bB.X + bB.Width - bA.Width, bA.Y), new Vector2D(System.Math.Ceiling(bA.Width), System.Math.Ceiling(bA.Height)));
                    //item.defaultRect = new Rect(new Vector2D(rectB.Position.X + rectB.Size.X - rectA.Size.X, rectA.Position.Y), new Vector2D(rectA.Size.X, rectA.Size.Y));
                }
                //item.singleRect = new Rect(new Vector2D(0, 0), new Vector2D(128, 128));
                using (var g = System.Drawing.Graphics.FromImage(item.bmp))
                {
                    if (false) g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 255, 0, 0)), 0, 0, 1000, 1000);

                    Emugen.Image.Drawing.DrawText.Do(item.text, efont, g, new System.Drawing.PointF((float)(margin-subItem.SingleRect.Position.X), (float)(margin - subItem.SingleRect.Position.Y)));


                    if (false) g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 255, 255, 255)),
                        (float)rectB.Position.X, (float)rectB.Position.Y, (float)(rectB.Position.X + rectB.Size.X), (float)(rectB.Position.Y + rectB.Size.Y));
                    //Emugen.Image.Drawing.DrawText.Do(item.text, efont, g, new System.Drawing.PointF((float)-rectB.Size.X+margin, (float)-rectB.Size.Y + margin));
                    //Emugen.Image.Drawing.DrawText.Do(item.text, efont, g, new System.Drawing.PointF((float)-rectB.Position.X, (float)-rectB.Position.Y));
                    //Emugen.Image.Drawing.DrawText.Do(item.text, efont, g, new System.Drawing.PointF((float)-rectB.Position.X, (float)-rectB.Position.Y));
                }
                var texture = new Texture(item.bmp);
                var w = item.bmp.Width;
                var h = item.bmp.Height;
                //var x = item.defaultRect.Position.X + position.X - margin + lineTextMargin * i;
                //var y = item.defaultRect.Position.Y + position.Y - margin;
                var x = item.defaultRect.Position.X + position.X - margin + lineTextMargin * k;
                var y = item.defaultRect.Position.Y + position.Y - margin;

                item.imageSprite = new ImageSprite(texture, new Rect(new Vector2D(x, y), new Vector2D(w, h)), new Color(1, 1, 1, 1));

                items.Add(item);
                layer.Add(item.imageSprite, 10);
                k++;
                //tmpX += item.bmp.Width - efont.lineTextMargin;
                tmpX += item.bmp.Width;
            }

            // Todo : あとで、文字送りするときだけするように変更する
            if (this.fadeTimer != -1)
            {
                foreach (var item in items)
                {
                    item.color.A = 0;
                }
            }
        }

        // テクスチャ1つの文字列
        public TextSprite(string text, Font efont, Emugen.Image.Primitive.Vector2D position, int dammy, int dammy2, Emugen.OpenTK.View.IView view = null, int fadeTimer = -1, int fadeTimerAdd = 100)
        {
            //this.position = position;
            this.fadeTimer = fadeTimer;
            this.fadeTimerAdd = fadeTimerAdd;
            this.lineTextMargin = efont.lineTextMargin;

            var frameSize = 0.0;
            var frameSizeMax = 0.0;
            var rfontFrames = new List<Font.FontFrame>();
            foreach (var frame in efont.fontFrames)
            {
                frameSizeMax += frame.size;
                rfontFrames.Insert(0, frame);
            }
            frameSize = frameSizeMax;

            // Todo : フォントフレームの幅分だけマージンの対応が必要
            margin = (int)System.Math.Floor(frameSize);

            // Todo : Test用
            //lineTextMargin = 50;

            var k = 0;
            var tmp = "";
            var tmpX = 0;

            var subItems = new List<SubItem>();
            foreach( var t in text)
            {
                tmp += t;
                var subItem = new SubItem();
                var a = Emugen.Image.Drawing.DrawText.Rect(t.ToString(), efont, new System.Drawing.PointF(0, 0));
                var b = Emugen.Image.Drawing.DrawText.Rect(tmp, efont, new System.Drawing.PointF(0, 0));
                var x2 = b.Position.X + b.Size.X;
                var y2 = b.Position.Y + b.Size.Y;
                subItem.Rect = new Rect( new Vector2D( x2 - a.Size.X, a.Position.Y), new Vector2D(a.Size.X, a.Size.Y));
                subItems.Add(subItem);
            }

            {
                var rectB = Emugen.Image.Drawing.DrawText.Rect(text.ToString(), efont, new System.Drawing.PointF(0, 0));

                var item = new Item();
                item.text = text;
                //item.bmp = new System.Drawing.Bitmap((int)(rectB.Position.X+rectB.Size.X + margin * 2), (int)(rectB.Position.Y+rectB.Size.Y + margin * 2));
                //item.bmp = new System.Drawing.Bitmap((int)(rectB.Position.X + rectB.Size.X + margin * 2), (int)(rectB.Position.Y + rectB.Size.Y + margin * 2));
                item.bmp = new System.Drawing.Bitmap((int)(rectB.Position.X + rectB.Size.X + margin * 2), (int)(rectB.Position.Y + rectB.Size.Y + margin * 2));
                //item.bmp = new System.Drawing.Bitmap((int)(rectB.Position.X + rectB.Size.X), (int)(rectB.Position.Y + rectB.Size.Y));


                //var rectA = Emugen.Image.Drawing.DrawText.Rect(tmp, efont, new System.Drawing.PointF(0, 0));
                item.defaultRect = new Rect(new Vector2D(margin, margin), new Vector2D(item.bmp.Width, item.bmp.Height));
     
                //item.singleRect = new Rect(new Vector2D(0, 0), new Vector2D(128, 128));
                using (var g = System.Drawing.Graphics.FromImage(item.bmp))
                {
                    if (false) g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 255, 0, 0)), 0, 0, 1000, 1000);

                    Emugen.Image.Drawing.DrawText.Do(item.text, efont, g, new System.Drawing.PointF(0, 0));


                    if (false) g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 255, 255, 255)),
                        (float)rectB.Position.X, (float)rectB.Position.Y, (float)(rectB.Position.X + rectB.Size.X), (float)(rectB.Position.Y + rectB.Size.Y));

                    var kk = 0;
                    foreach(var subItem in subItems)
                    {
                        //if (kk == 1)
                        {
                            g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 255, 255, 0)),
                                (float)subItem.Rect.Position.X, (float)subItem.Rect.Position.Y,
                                (float)subItem.Rect.Size.X, (float)subItem.Rect.Size.Y);
                        }

                        kk++;
                    }
                    //Emugen.Image.Drawing.DrawText.Do(item.text, efont, g, new System.Drawing.PointF((float)-rectB.Size.X+margin, (float)-rectB.Size.Y + margin));
                    //Emugen.Image.Drawing.DrawText.Do(item.text, efont, g, new System.Drawing.PointF((float)-rectB.Position.X, (float)-rectB.Position.Y));
                    //Emugen.Image.Drawing.DrawText.Do(item.text, efont, g, new System.Drawing.PointF((float)-rectB.Position.X, (float)-rectB.Position.Y));
                }
                var texture = new Texture(item.bmp);
                var w = item.bmp.Width;
                var h = item.bmp.Height;
                //var x = item.defaultRect.Position.X + position.X - margin + lineTextMargin * i;
                //var y = item.defaultRect.Position.Y + position.Y - margin;
                var x = item.defaultRect.Position.X + position.X - margin + lineTextMargin * k;
                var y = item.defaultRect.Position.Y + position.Y - margin;

                item.imageSprite = new ImageSprite(texture, new Rect(new Vector2D(x, y), new Vector2D(w, h)), new Color(1, 1, 1, 1));

                items.Add(item);
                layer.Add(item.imageSprite, 10);
                k++;
                //tmpX += item.bmp.Width - efont.lineTextMargin;
                tmpX += item.bmp.Width;

            }

            // Todo : あとで、文字送りするときだけするように変更する
            if (this.fadeTimer != -1)
            {
                foreach (var item in items)
                {
                    item.imageSprite.Color.A = 0;
                }
            }
        }


        private System.Drawing.Font CreateFont( string fontPath, double fontSize)
        {
            //var fontPath = "data/font/migmix-2p-bold.ttf";
            var pfc = new System.Drawing.Text.PrivateFontCollection();
            var id = pfc.Families.Length;
            pfc.AddFontFile(fontPath);
            var fontFamily = pfc.Families[id];
            var fontStyle = System.Drawing.FontStyle.Regular;

            if (fontFamily.IsStyleAvailable(System.Drawing.FontStyle.Regular))
            {
                fontStyle = System.Drawing.FontStyle.Regular;
            }
            else if (fontFamily.IsStyleAvailable(System.Drawing.FontStyle.Bold))
            {
                fontStyle = System.Drawing.FontStyle.Bold;
            }
            else if (fontFamily.IsStyleAvailable(System.Drawing.FontStyle.Italic))
            {
                fontStyle = System.Drawing.FontStyle.Italic;
            }
            else if (fontFamily.IsStyleAvailable(System.Drawing.FontStyle.Strikeout))
            {
                fontStyle = System.Drawing.FontStyle.Strikeout;
            }
            else if (fontFamily.IsStyleAvailable(System.Drawing.FontStyle.Underline))
            {
                fontStyle = System.Drawing.FontStyle.Strikeout;
            }

            var font = new System.Drawing.Font(fontFamily, (float)fontSize, fontStyle);
            return font;
        }

        private System.Drawing.Font CheckFont(System.Drawing.Font font, string fontPath, double fontSize)
        {
            // todo : font を作り直しているが、もしかすると利用中に破棄しているかもしれない？
            return CreateFont(fontPath, fontSize);
        }

        override public void Update()
        {
            base.Update();

            if (fadeTimer >= 0)
            {
                fadeTimer += fadeTimerAdd;
                var oneStringShowTime = 200;
                var fadeStringNum = 3;

                var i = 0;
                var j = 0;
                foreach (var item in items)
                {
                    if (i * oneStringShowTime <= fadeTimer)
                    {
                        j++;
                        //item.imageSprite.Color.A = 1;
                        item.color.A = 1;
                    }
                    else if (((i - fadeStringNum) * oneStringShowTime <= fadeTimer) && (fadeTimer < i * oneStringShowTime))
                    {
                        //item.imageSprite.Color.A = 0.5;
                        //item.imageSprite.Color.A = (double)(fadeTimer - (i - fadeStringNum) * oneStringShowTime) / (double)(oneStringShowTime * fadeStringNum);
                        item.color.A = (double)(fadeTimer - (i - fadeStringNum) * oneStringShowTime) / (double)(oneStringShowTime * fadeStringNum);
                    }
                    else
                    {
                        //item.imageSprite.Color.A = 0;
                        item.color.A = 0;
                    }
                    i++;
                }

                if (j == i)
                {
                    if (OnShowEnd != null) OnShowEnd();
                }
            }
            //else
            //{
            //    foreach (var item in items)
            //    {
            //        item.imageSprite.Color = Color; // Todo 
            //    }
            //}
        }

        public override void Draw()
        {
            // 座標の設定
            {
                var i = 0;
                foreach (var item in items)
                {
                    //if (item.texture!=null)
                    //{
                    //    item.texture = new Texture(item.bmp);
                    //}
                    //var texture = new Texture(item.bmp);
                    var w = item.bmp.Width;
                    var h = item.bmp.Height;
                    var x = item.defaultRect.Position.X + Rect.Position.X - margin + lineTextMargin * i;
                    var y = item.defaultRect.Position.Y + Rect.Position.Y - margin;
                    item.imageSprite.Rect.Position = new Vector2D(x, y);
                    item.imageSprite.Rect.Size = new Vector2D(w, h);

                    item.imageSprite.Color = new Color( Color.A * item.color.A, Color.R * item.color.R, Color.G * item.color.G, Color.B * item.color.B); // Todo 
                    //item.imageSprite.Color = new Color( Color.A *  )

                    i++;
                }
            }

            layer.Draw();
        }

        // todo : Rectで直接取得できないのでまどこっこしい
        // todo : このitemsを収集してサイズを割り出すのは汎用化できそう
        public Rect GetRect()
        {
            // 文字の大きさを確認する
            {
                var startX = items[0].imageSprite.Rect.Position.X;
                var endX = items[0].imageSprite.Rect.Position.X + items[0].imageSprite.Rect.Size.X;
                var startY = items[0].imageSprite.Rect.Position.Y;
                var endY = items[0].imageSprite.Rect.Position.Y + items[0].imageSprite.Rect.Size.Y;
                foreach (var item in items)
                {
                    startX = System.Math.Min(startX, item.imageSprite.Rect.Position.X);
                    startY = System.Math.Min(startY, item.imageSprite.Rect.Position.Y);
                    endX = System.Math.Max(endX, item.imageSprite.Rect.Position.X + item.imageSprite.Rect.Size.X);
                    endY = System.Math.Max(endY, item.imageSprite.Rect.Position.Y + item.imageSprite.Rect.Size.Y);
                }
                return new Rect( new Vector2D( startX, startY), new Vector2D( endX - startX, endY - startY ) );
            }
        }

        public void Dispose()
        {
            foreach (var item in items)
            {
                item.bmp.Dispose();
                item.imageSprite.Dispose();
            }
        }
    }
}
