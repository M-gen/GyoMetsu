using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Resource;

namespace GyoMetsu.UI.Common
{
    public class Button : ISprite, IDisposable
    {
        LayerSprite layer = new LayerSprite();
        TextSprite textSpriteNewGame;
        TextSprite textSpriteNewGameEffect;
        double textSpriteNewGameEffectTimer = 0;

        Action click;

        public Button( string text, Font font, Vector2D position, Action click )
        {
            this.click = click;

            {
                var ts = new TextSprite( text, font, new Vector2D(0, 0), TextSprite.Type.Separation, null);
                layer.Add(ts, 20);

                var size = ts.GetRect().Size;
                ts.Rect.Position = new Vector2D((1600 - size.X) / 2, position.Y);

                textSpriteNewGame = ts;
            }
            {
                var font2 = new Font(
                    font.fontPath,
                    font.fontSize,
                    new Color(1, 1, 1, 1),
                    new Font.FontFrame[] {
                        new Font.FontFrame(4, new Color(1.0, 1.0, 1.0, 1.0)),
                        new Font.FontFrame(1, new Color(0.5, 1.0, 1.0, 1.0))},
                    font.lineTextMargin);
                var x = position.X;
                var y = position.Y;
                var ts = new TextSprite(text, font2, new Vector2D(0, 0), TextSprite.Type.Separation, null);
                layer.Add(ts, 30);

                var size = ts.GetRect().Size;
                ts.Rect.Position = new Vector2D((1600 - size.X) / 2, position.Y);

                textSpriteNewGameEffect = ts;
                textSpriteNewGameEffect.IsDraw = false;
            }

        }

        public override void Update()
        {
            var position = textSpriteNewGame.Rect.Position;
            var size = textSpriteNewGame.GetRect().Size;
            var mouse = Emugen.Input.InputCore.Instance.mousePosition;
            if (((position.X <= mouse.X) && (mouse.X < (position.X + size.X))) &&
                 ((position.Y <= mouse.Y) && (mouse.Y < (position.Y + size.Y))))
            {
                var input = Emugen.Input.InputCore.Instance;
                if (input.GetKeyEventType(Emugen.Input.InputCore.KeyEventCode.MouseLeftButton) == Emugen.Input.InputCore.KeyEventType.Up)
                {
                    if (click != null) click();
                }
                textSpriteNewGameEffect.IsDraw = true;

                {
                    textSpriteNewGameEffectTimer += 0.10;

                    var tmp = ((Math.Sin(textSpriteNewGameEffectTimer) + 1.0) / 2.0) * 0.5 + 0.2;
                    if (textSpriteNewGameEffectTimer >= (Math.PI * 2))
                    {
                        textSpriteNewGameEffectTimer = 0;
                    }
                    textSpriteNewGameEffect.Color.A = tmp;
                }
            }
            else
            {
                textSpriteNewGameEffect.IsDraw = false;
                textSpriteNewGameEffectTimer = 0;
            }
        }

        public override void Draw()
        {
            layer.Draw();
        }

        public void Dispose()
        {
        }
    }
}
