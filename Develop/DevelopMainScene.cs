using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Image.Primitive;


namespace Develop
{
    class DevelopMainScene : Scene
    {
        LayerSprite layer = new LayerSprite();

        public DevelopMainScene()
        {
            var position = new Vector2D(100, 100);
            {
                var font = new Font(
                    
                    "data/font/rounded-mgenplus-1cp-bold.ttf",
                    16,
                    new Color(1, 1, 1, 1),
                    new Font.FontFrame[] { new Font.FontFrame(3, new Color(1, 1, 0, 0)), new Font.FontFrame(3, new Color(1, 0, 1, 0)) },
                    100);
                var x = position.X;
                var y = position.Y;
                var ts = new TextSprite("abcd", font, new Vector2D(x, y), TextSprite.Type.Separation, null);
                layer.Add(ts, 20);
            }

        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            layer.Draw();
        }
    }
}
