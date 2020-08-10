using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Image.Primitive;
//using Emugen.Image.Drawing;
using Emugen.Resource;
using Emugen.Sound;

namespace GyoMetsu.Scene
{
    public class TitleScene : Emugen.OpenTK.Scene
    {
        LayerSprite layer = new LayerSprite();

        SoundPlayer bgm;

        class MySprites
        {
            public UI.Common.Button ButtonNewGame;
            public UI.Common.Button ButtonExit;
        }
        MySprites mySprites = new MySprites();

        public TitleScene()
        {
            {
                var bmp = new System.Drawing.Bitmap(Config.MainConfig.TitleScene.TitleBackgroundImage);
                var texture = new Texture(bmp);
                var w = 1600;
                var h = (int)((double)texture.Size.Y / (double)texture.Size.X * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(0, 0), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite, 5);
            }

            {
                bgm = new SoundPlayer(Config.MainConfig.TitleScene.TitleBGM, 0.5f, true, Emugen.Sound.SoundPlayer.SoundType.BGM);
            }

            {
                var position = new Vector2D(0, 0);
                var font = new Font(
                    Config.MainConfig.MainFontPath,
                    45,
                    new Color(1, 1, 1, 1),
                    new Font.FontFrame[] {
                        new Font.FontFrame(4, new Color(1, 0, 0.5, 1.0))},
                        5);
                var button = new UI.Common.Button("New Game", font, new Vector2D(0,600), ()=> {
                    new Emugen.Sound.SoundPlayer( Config.MainConfig.TitleScene.SelectSE, 0.5f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                    bgm.Stop();
                    WindowManager.nextScene = new Scene.TalkScene( Config.MainConfig.TitleScene.NewGameStartScript);
                });
                layer.Add(button, 20);
                mySprites.ButtonNewGame = button;
            }

            {
                var position = new Vector2D(0, 0);
                var font = new Font(
                    Config.MainConfig.MainFontPath,
                    45,
                    new Color(1, 1, 1, 1),
                    new Font.FontFrame[] {
                        new Font.FontFrame(4, new Color(1, 0, 0.5, 1.0))},
                        5);
                var button = new UI.Common.Button("Exit", font, new Vector2D(0, 600+100), () => {
                    new Emugen.Sound.SoundPlayer(Config.MainConfig.TitleScene.SelectSE, 0.5f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                    bgm.Stop();

                    Emugen.Thread.Sleep.Do(100);
                    Emugen.OpenTK.WindowManager.Close();

                });
                layer.Add(button, 20);
                mySprites.ButtonExit = button;
            }

        }
        public override void Update()
        {
            mySprites.ButtonNewGame.Update();
            mySprites.ButtonExit.Update();
        }

        public override void Draw()
        {
            layer.Draw();
        }

        public override void Dispose()
        {
        }

    }
}
