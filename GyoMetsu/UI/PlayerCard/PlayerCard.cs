using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Resource;

using GyoMetsu.UI.Effect;

namespace GyoMetsu.UI.PlayerCard
{

    public class PlayerCard : GyoMetsu.UI.CharacterCard
    {
        System.Drawing.Bitmap cardImage;

        class MySprites
        {
            public ImageSprite face;
            public PlaneLineSprite mouseOn;
            public PlaneSprite bar;

            public TextSprite viewName;
            public TextSprite ViewStatus;

            public PlaneSprite elementTimerBar;
            public PlaneLineSprite elementTimerBarFrame;

            public List<ImageSprite> element = new List<ImageSprite>();
        }
        MySprites mySprites = new MySprites();

        AutoDisposer autoDisposer = new AutoDisposer();
        ActionEffectUpdate actionEffectUpdate;

        public ImageSprite mouseOnTarget;
        int mouseOnTargetMargin = 20;
        public int mouseOnTargetFadeTimer = 0;

        public PlayerCard( Vector2D position, Data.Character character)
        {
            //var cardImage = default(System.Drawing.Bitmap);
            this.character = character;
            this.cardImage = new System.Drawing.Bitmap(character.imagePath);
            this.position = position;

            character.playerCard = this;

            {
                var bmp = cardImage;
                var texture = new Texture(bmp);
                var w = 160;
                var h = (int)((double)bmp.Height / (double)bmp.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(position.X, position.Y), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite,10);

                size = new Vector2D(w, h);
                mySprites.face = sprite;
            }

            {
                var x = 0;
                var y = 0;
                var w = 60;
                var h = 6;
                var sprite = new PlaneSprite(new Rect(new Vector2D(x, y), new Vector2D(w, h)), new Color(0.75, 0, 1, 0.5));
                layer.Add(sprite, 10);
                mySprites.elementTimerBar = sprite;
            }
            {
                var x = 0;
                var y = 0;
                var w = 60+2;
                var h = 6+2;
                var sprite = new PlaneLineSprite(new Rect(new Vector2D(x, y), new Vector2D(w, h)), new Color(0.5, 0, 0, 0));
                layer.Add(sprite, 10);
                mySprites.elementTimerBarFrame = sprite;
            }

            {
                var x = position.X - 5;
                var y = position.Y + 200;

                var font = new Font(Config.MainConfig.MainFontPath, 16, new Color(1, 1, 1, 1), new Font.FontFrame[] {
                                new Font.FontFrame(2, new Color(1,0,0,0)),
                                }, 0);
                var ts = new TextSprite(character.ViewName, font, new Vector2D(x, y));

                layer.Add(ts, 20);
                mySprites.viewName = ts;
            }
            {
                var x = position.X ;
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
                var ts = new TextSprite($"{character.HP.Now}/{character.HP.Max}", font, new Vector2D(x, y));
                layer.Add(ts, 20);
                mySprites.ViewStatus = ts;
            }

            {
                var sprite = new PlaneLineSprite(new Rect(position, size), new Color(0.0, 1, 1, 1));
                layer.Add(sprite, 30);
                mySprites.mouseOn = sprite;
            }


            {
                var margin = mouseOnTargetMargin;
                var filePath = character.imagePath + ".sel.png";
                var bmp2 = default(System.Drawing.Bitmap);
                if ( System.IO.File.Exists(filePath) )
                {
                    bmp2 = new System.Drawing.Bitmap(filePath);
                }
                else
                {
                    bmp2 = DrawUtility.DrawFrame.Draw(cardImage, margin, 8.0, 2.0, new Color(1, 0, 0.2, 1));
                    bmp2.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                var texture = new Texture(bmp2);
                var w = 160 + margin * 2;
                var h = (int)((double)bmp2.Height / (double)bmp2.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(10, 10), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite, 5);

                mouseOnTarget = sprite;
            }

            foreach ( var element in character.Elements)
            {
                AddElement(element);
            }

        }

        public void Update()
        {

            var mouse = Emugen.Input.InputCore.Instance.mousePosition;
            if ( ( (position.X <= mouse.X) && ( mouse.X < (position.X + size.X) ) ) &&
                 ( (position.Y <= mouse.Y) && ( mouse.Y < (position.Y + size.Y))) )
            {
                isMouseOn = true;
                mySprites.mouseOn.IsDraw = true;
            }
            else
            {
                isMouseOn = false;
                mySprites.mouseOn.IsDraw = false;
            }

            // エレメントゲージ
            {
                var val = character.elementTimer / character.elementTimerMax;
                mySprites.elementTimerBar.Rect.Size.X = 60 * val;
            }

            if (damageTimer != -1)
            {
                damageTimer++;
                if (damageTimerMax < damageTimer) damageTimer = -1;
                SetPosition(position); // Todo : 毎アップデート時に呼び出さないといけない？
            }

            if (actionEffectUpdate != null)
            {
                SetPosition(position);
            }
            autoDisposer.Update();

            if(mouseOnTarget.IsDraw)
            {
                var max = 30.0;
                mouseOnTarget.Color.A = Math.Sin((double)mouseOnTargetFadeTimer * 360 / max * Math.PI / 180) * 0.5 + 0.5;

                mouseOnTargetFadeTimer++;
                if (mouseOnTargetFadeTimer > max) mouseOnTargetFadeTimer = 0;
            }
        }

        public override void Draw()
        {
            //if (isMouseOn)
            layer.Draw();
        }

        public void SetPosition( Vector2D position )
        {
            this.position = position;

            var offsetX = 0.0;
            var offsetY = 0.0;
            if (actionEffectUpdate != null)
            {
                actionEffectUpdate.Update();
                offsetX += actionEffectUpdate.script.api.Position.X;
                offsetY += actionEffectUpdate.script.api.Position.Y;
            }


            if (damageTimer == -1)
            {
                mySprites.face.Rect.Position = new Vector2D(position.X + offsetX, position.Y+ offsetY);
                mouseOnTarget.Rect.Position = new Vector2D(position.X + offsetX- mouseOnTargetMargin , position.Y + offsetY- mouseOnTargetMargin );
            }
            else
            {
                var oofsetX = Math.Cos(damageTimer * 0.7) * 10 * ((damageTimerMax - damageTimer) / (damageTimerMax));
                mySprites.face.Rect.Position = new Vector2D(position.X + offsetX + oofsetX, position.Y+ offsetY);
                mouseOnTarget.Rect.Position = new Vector2D(position.X + offsetX + oofsetX - mouseOnTargetMargin, position.Y + offsetY - mouseOnTargetMargin);
            }

            mySprites.mouseOn.Rect.Position = new Vector2D(position.X + offsetX, position.Y + offsetY);
            {
                var x = position.X + 98 + offsetX;
                var y = position.Y + 232 + offsetY;
                mySprites.elementTimerBar.Rect.Position = new Vector2D(x, y);
                mySprites.elementTimerBarFrame.Rect.Position = new Vector2D(x-1, y-1);
            }
            {
                var x = position.X - 5 + offsetX;
                var y = position.Y + 200 + offsetY;
                mySprites.viewName.Rect.Position = new Vector2D(x,y);
            }
            {
                var x = position.X + offsetX;
                var y = position.Y + 227 + offsetY;
                mySprites.bar.Rect.Position = new Vector2D(x, y);
            }
            {
                var x = position.X - 5 + offsetX;
                var y = position.Y + 225 + offsetY;
                //var ts = new TextSprite("100/100", "data/font/rounded-mgenplus-1cp-medium.ttf", 12, new Vector2D(x, y), new Color(1, 1, 1, 1));
                //layer.Add(ts, 20);
                mySprites.ViewStatus.Rect.Position = new Vector2D(x, y);
            }
            {
                var i = 0;
                foreach( var element in mySprites.element)
                {
                    var x = position.X + 0 + i * 32 + offsetX;
                    var y = position.Y + 225+22+ offsetY;
                    element.Rect.Position = new Vector2D(x, y);
                    i++;
                }
            }
        }

        public override void AddElement( Data.Element element )
        {
            var elementNameToFilePath = new Dictionary<string, string>();
            foreach (var i in Config.MainConfig.Elements)
            {
                elementNameToFilePath.Add(i.Word, i.ImagePath);
            }

            var bmp = new System.Drawing.Bitmap(elementNameToFilePath[element.Name]);
            var texture = new Texture(bmp);
            var w = 30;
            var h = (int)((double)bmp.Height / (double)bmp.Width * w);
            var sprite = new ImageSprite(texture, new Rect(new Vector2D(0, 0), new Vector2D(w, h)), new Color(1, 1, 1, 1));
            layer.Add(sprite, 10);
            mySprites.element.Add(sprite);

            SetPosition(this.position);
        }

        public override void RemoveElement( int index )
        {
            {
                var e = mySprites.element[index];
                layer.Del(e);
                mySprites.element.RemoveAt(index);
            }

            SetPosition(this.position);
        }

        public override void RemoveElementAll()
        {
            foreach( var e in mySprites.element)
            {
                layer.Del(e);
            }
            mySprites.element.Clear();

        }

        public override void RefreshViewHP()
        {
            {
                var font = new Font( Config.MainConfig.MainFontPath, 12, new Color(1, 1, 1, 1), new Font.FontFrame[] {
                                new Font.FontFrame(2, new Color(1,0,0,0)),
                                }, 0);
                var ts = new TextSprite($"{character.HP.Now}/{character.HP.Max}", font, new Vector2D(0, 0));
                //var ts = new TextSprite( $"{character.HP.Now}/{character.HP.Max}", "data/font/ -mgenplus-1cp-medium.ttf", 12, new Vector2D(0, 0), new Color(1, 1, 1, 1));
                layer.Add(ts, 20);

                if (mySprites.ViewStatus != null)
                {
                    layer.Del(mySprites.ViewStatus);
                }
                mySprites.ViewStatus = ts;
            }

            if(character.HP.Now==0)
            {
                mySprites.face.Color = new Color(1.0, 0.5, 0.5, 0.5);
            }
            else
            {
                mySprites.face.Color = new Color(1.0, 1, 1, 1);
            }

            SetPosition(this.position);
        }

        public override void ShowDamage(int value)
        {
            var damage = new Effect.Damage(value, new Vector2D(this.position.X + mySprites.face.Rect.Size.X / 2, this.position.Y + mySprites.face.Rect.Size.Y / 2), autoDisposer);
            layer.Add(damage, 50);
        }
        public override void ShowMiss()
        {
            var e = new Effect.Miss( new Vector2D(this.position.X + mySprites.face.Rect.Size.X / 2, this.position.Y + mySprites.face.Rect.Size.Y / 2), autoDisposer);
            layer.Add(e, 50);
        }

        public override void ShowHealing(int value)
        {
            var damage = new Effect.Healing(value, new Vector2D(this.position.X + mySprites.face.Rect.Size.X / 2, this.position.Y + mySprites.face.Rect.Size.Y / 2), autoDisposer);
            layer.Add(damage, 50);
        }

        public override void ActionEffect(Action action, Data.ActionSkill actionSkill)
        {
            SetPosition(this.position);
            if( actionSkill.Main.EffectWord.IndexOf("HPダメージ") == 0 ) {
                actionEffectUpdate = new ActionEffectUpdate("data/script/action_pc_攻撃.cs", action, autoDisposer);
            }
            else if (actionSkill.Main.EffectWord.IndexOf("HP回復") == 0)
            {
                actionEffectUpdate = new ActionEffectUpdate("data/script/action_pc_回復.cs", action, autoDisposer);
            }
        }
    }
}
