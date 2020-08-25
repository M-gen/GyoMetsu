using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;


namespace GyoMetsu.UI.Talk
{
    public class Choice : ISprite
    {
        LayerSprite layer = new LayerSprite();

        class Item
        {
            public int Index;
            public string Text;
            public TextSprite TextSprite;
            public Rect Rect;
            public PlaneLineSprite PlaneLineSprite;
            public PlaneSprite BackPlaneSprite;

            public bool IsMouseOn;
        }
        List<Item> items = new List<Item>();

        public Choice( string[] texts )
        {
            var i = 0;
            foreach( var text in texts)
            {
                var item = new Item();
                {
                    var x = 500;
                    var y = 300 + i * 60;

                    var font = new Font( Config.MainConfig.MainFontPath, 26, new Color(1, 1, 1, 1), new Font.FontFrame[] {
                                new Font.FontFrame(2, new Color(1,0,0,0)),
                                }, 0);
                    var ts = new TextSprite(text, font, new Vector2D(x, y));


                    layer.Add(ts, 20);


                    item.TextSprite = ts;
                }
                {
                    var x = 500;
                    var y = 300 + i * 60;
                    var sprite = new PlaneLineSprite(new Rect(new Vector2D(x, y), new Vector2D(600, 50)), new Color(0.5, 1, 1, 1));
                    layer.Add(sprite, 20);
                    item.PlaneLineSprite = sprite;
                }
                {
                    var x = 500;
                    var y = 300 + i * 60;
                    var sprite = new PlaneSprite(new Rect(new Vector2D(x, y), new Vector2D(600, 50)), new Color(0.5, 0, 0, 0));
                    layer.Add(sprite, 5);
                    item.BackPlaneSprite = sprite;
                }
                item.Text = text;

                items.Add(item);

                i++;
            }
        }

        public void Update()
        {
            var mouse = Emugen.Input.InputCore.Instance.mousePosition;
            foreach ( var item in items)
            {
                var position = item.PlaneLineSprite.Rect.Position;
                var size = item.PlaneLineSprite.Rect.Size;
                if (((position.X <= mouse.X) && (mouse.X < (position.X + size.X))) &&
                     ((position.Y <= mouse.Y) && (mouse.Y < (position.Y + size.Y))))
                {
                    item.IsMouseOn = true;
                    item.PlaneLineSprite.IsDraw = true;
                    //mySprites.mouseOn.IsDraw = true;
                }
                else
                {
                    item.IsMouseOn = false;
                    item.PlaneLineSprite.IsDraw = false;
                    //mySprites.mouseOn.IsDraw = false;
                }

            }

        }

        public override void Draw()
        {
            layer.Draw();
        }

        public string GetChoiseText()
        {
            foreach (var item in items)
            {
                if(item.IsMouseOn)
                {
                    return item.Text;
                }
            }
            return null;
        }

    }
}
