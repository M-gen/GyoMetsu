using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Resource;

namespace GyoMetsu.UI.Effect
{
    class Miss : ISprite, IDisposable
    {
        LayerSprite layer = new LayerSprite();

        Vector2D position;
        TextSprite textSprite;
        int timer = 0;

        public Miss( Vector2D position, AutoDisposer autoDisposer)
        {
            this.SetupUpdateAndAutoDispose(autoDisposer);
            this.position = position;

            {
                var font = new Font(
                    Config.MainConfig.MainFontPath,
                    35,
                    new Color(1, 1, 1, 1),
                    new Font.FontFrame[] {
                        new Font.FontFrame(5, new Color(1, 1, 1, 1)),
                        new Font.FontFrame(5, new Color(1, 0, 0, 0)) },
                    0);
                var x = position.X;
                var y = position.Y;
                var ts = new TextSprite("MISS", font, new Vector2D(x, y));
                layer.Add(ts, 20);

                ts.Rect.Position.X -= ts.GetRect().Size.X / 2;

                textSprite = ts;
            }

        }

        public override void Update()
        {

            var m = 15;
            if (timer < m)
            {
            }
            else
            {
                textSprite.Color.A = 1.0 - ((timer - m) / 30.0);
                if (textSprite.Color.A < 0)
                {
                    textSprite.Color.A = 0;
                    WaitDispose();
                }
            }

            if (timer < 5)
            {
                textSprite.Rect.Position.Y -= 3;
            }
            else if (timer < 10)
            {
                textSprite.Rect.Position.Y -= 2;
            }
            else
            {
                textSprite.Rect.Position.Y -= 1;
            }

            timer++;
        }

        public override void Draw()
        {
            layer.Draw();
        }

        public void Dispose()
        {
            layer.Dispose();
        }

    }
}
