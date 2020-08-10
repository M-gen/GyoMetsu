using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;

namespace GyoMetsu.UI.SkillCard
{
    public class SkillCard : ISprite
    {
        System.Drawing.Bitmap cardImage;
        Vector2D position;
        Vector2D size;

        LayerSprite layer = new LayerSprite();
        public bool isMouseOn = false;
        Data.Character ownerCharacter;

        class MySprites
        {
            public ImageSprite face;
            public PlaneLineSprite mouseOn;
            public PlaneSprite bar;

            public TextSprite viewName;
            public TextSprite ViewStatus;
        }
        MySprites mySprites = new MySprites();

        public string cost; // Todo : actionSkillで統一できる
        public Data.ActionSkill actionSkill;

        public SkillCard(System.Drawing.Bitmap cardImage, Vector2D position, string viewName, string cost, Data.Character ownerCharacter, Data.ActionSkill actionSkill)
        {
            this.cardImage = cardImage;
            this.position = position;
            this.ownerCharacter = ownerCharacter;
            this.cost = cost;
            this.actionSkill = actionSkill;

            {
                var bmp = cardImage;
                var texture = new Texture(bmp);
                var w = 160;
                var h = (int)((double)bmp.Height / (double)bmp.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(position.X, position.Y), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite, 10);

                size = new Vector2D(w, h);
                mySprites.face = sprite;
            }

            {
                var x = position.X - 5;
                var y = position.Y + 200;
                var font = new Font(Config.MainConfig.MainFontPath, 16, new Color(1, 1, 1, 1), new Font.FontFrame[] {
                                new Font.FontFrame(2, new Color(1,0,0,0)),
                                }, 0);
                var ts = new TextSprite( viewName, font, new Vector2D(x, y));

                layer.Add(ts, 20);
                mySprites.viewName = ts;
            }
            {
                var x = position.X;
                var y = position.Y + 227;
                var w = size.X;
                var h = 1;
                var sprite = new PlaneSprite(new Rect(new Vector2D(x, y), new Vector2D(w, h)), new Color(0.3, 1, 1, 1));
                layer.Add(sprite, 10);
                mySprites.bar = sprite;
            }
            {
                var x = position.X - 5;
                var y = position.Y + 225;
                var font = new Font(Config.MainConfig.MainFontPath, 12, new Color(1, 1, 1, 1), new Font.FontFrame[] {
                                new Font.FontFrame(2, new Color(1,0,0,0)),
                                }, 0);
                var ts = new TextSprite(cost, font, new Vector2D(x, y));
                //var ts = new TextSprite(cost, "data/font/rounded-mgenplus-1cp-medium.ttf", 12, new Vector2D(x, y), new Color(1, 1, 1, 1));
                layer.Add(ts, 20);
                mySprites.ViewStatus = ts;
            }

            {
                var sprite = new PlaneLineSprite(new Rect(position, size), new Color(0.5, 1, 1, 1));
                layer.Add(sprite, 30);
                mySprites.mouseOn = sprite;
            }

        }

        public void Update()
        {
            var mouse = Emugen.Input.InputCore.Instance.mousePosition;
            if (((position.X <= mouse.X) && (mouse.X < (position.X + size.X))) &&
                 ((position.Y <= mouse.Y) && (mouse.Y < (position.Y + size.Y))))
            {
                isMouseOn = true;
                mySprites.mouseOn.IsDraw = true;
            }
            else
            {
                isMouseOn = false;
                mySprites.mouseOn.IsDraw = false;
            }

            // エレメント、コストが足りているかを確認する
            var ok = Data.Element.IsUsable(ownerCharacter.Elements, cost);
            SetEnable(ok);

        }

        public override void Draw()
        {
            //if (isMouseOn)
            layer.Draw();
        }

        public void SetPosition(Vector2D position)
        {
            this.position = position;
            mySprites.face.Rect.Position = new Vector2D(position.X, position.Y);
            mySprites.mouseOn.Rect.Position = new Vector2D(position.X, position.Y);
            {
                var x = position.X - 5;
                var y = position.Y + 200;
                mySprites.viewName.Rect.Position = new Vector2D(x, y);
            }
            {
                var x = position.X;
                var y = position.Y + 227;
                mySprites.bar.Rect.Position = new Vector2D(x, y);
            }
            {
                var x = position.X - 5;
                var y = position.Y + 225;
                //var ts = new TextSprite("100/100", "data/font/rounded-mgenplus-1cp-medium.ttf", 12, new Vector2D(x, y), new Color(1, 1, 1, 1));
                //layer.Add(ts, 20);
                mySprites.ViewStatus.Rect.Position = new Vector2D(x, y);
            }
        }

        public void SetEnable( bool isEnable )
        {
            if( isEnable)
            {
                mySprites.face.Color = new Color(1,1,1,1);

            }
            else
            {
                var col = 0.3;
                mySprites.face.Color = new Color(1, col, col, col);

            }
        }
    }
}
