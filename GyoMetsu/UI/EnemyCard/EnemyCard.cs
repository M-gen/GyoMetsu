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

namespace GyoMetsu.UI.EnemyCard
{
    public class EnemyCard : GyoMetsu.UI.CharacterCard
    {
        public enum Step
        {
            None,
            Lost,
        }
        public Step step = Step.None;
        public int stepTimer = 0;

        public System.Drawing.Bitmap cardImage;


        bool isLost = false;
        public bool IsLost() { return isLost; }

        class MySprites
        {
            public ImageSprite face;
            public ImageSprite shadow;
            public PlaneSprite mouseOn;
            public PlaneSprite hpBar;
            public PlaneLineSprite hpBarFrame;
            public List<ImageSprite> element = new List<ImageSprite>();

        }
        MySprites mySprites = new MySprites();

        public ImageSprite mouseOnTarget;
        int mouseOnTargetMargin = 20;
        public int mouseOnTargetFadeTimer = 0;

        AutoDisposer autoDisposer = new AutoDisposer();

        public Rect Rect { get { return mySprites.mouseOn.Rect; } }

        ActionEffectUpdate actionEffectUpdate;

        public EnemyCard(System.Drawing.Bitmap cardImage, Vector2D position, string viewName, Data.Character character)
        {
            this.cardImage = cardImage;
            this.character = character;
            character.enemyCard = this;

            {
                var bmp = cardImage;
                var texture = new Texture(bmp);
                var w = (int)(bmp.Width * character.imageScale); // 160
                var h = (int)((double)bmp.Height / (double)bmp.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(position.X, position.Y), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite, 10);

                size = new Vector2D(w, h);

                mySprites.face = sprite;

            }

            {
                var bmp = new System.Drawing.Bitmap(Config.MainConfig.BattleScene.EnemyShadowImage); 
                var texture = new Texture(bmp);
                var w = 160;
                var h = (int)((double)bmp.Height / (double)bmp.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(position.X, position.Y), new Vector2D(w, h)), new Color(0.8, 1, 1, 1));
                layer.Add(sprite, 5);
                mySprites.shadow = sprite;
            }

            {
                var x = 0;
                var y = 0;
                var w = 60;
                var h = 6;
                var sprite = new PlaneSprite(new Rect(new Vector2D(x, y), new Vector2D(w, h)), new Color(0.75, 1, 0.2, 0.0));
                layer.Add(sprite, 10);
                mySprites.hpBar = sprite;
            }
            {
                var x = 0;
                var y = 0;
                var w = 60 + 2;
                var h = 6 + 2;
                var sprite = new PlaneLineSprite(new Rect(new Vector2D(x, y), new Vector2D(w, h)), new Color(0.5, 0, 0, 0));
                layer.Add(sprite, 10);
                mySprites.hpBarFrame = sprite;
            }

            {
                //var sprite = new PlaneLineSprite(new Rect(position, size), new Color(0.5, 1, 1, 1));
                var sprite = new PlaneSprite(new Rect(position, size), new Color(0.0, 1, 1, 1));
                layer.Add(sprite, 30);
                mySprites.mouseOn = sprite;
                mySprites.mouseOn.IsDraw = false;
            }

            {
                var margin = mouseOnTargetMargin;

                var filePath = character.imagePath + ".sel.png";
                var bmp2 = default(System.Drawing.Bitmap);
                if (System.IO.File.Exists(filePath))
                {
                    bmp2 = new System.Drawing.Bitmap(filePath);
                }
                else
                {
                    bmp2 = DrawUtility.DrawFrame.Draw(cardImage, margin, 8.0, 2.0, new Color(1, 0, 0.2, 1));
                    bmp2.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                var texture = new Texture(bmp2);
                var w = (int)(bmp2.Width * character.imageScale); // 160
                var h = (int)((double)bmp2.Height / (double)bmp2.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(10, 10), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite, 5);

                mouseOnTarget = sprite;
            }

        }

        public void Update()
        {

            switch (step)
            {
                case Step.None:
                    {
                        var mouse = Emugen.Input.InputCore.Instance.mousePosition;
                        //if (((position.X <= mouse.X) && (mouse.X < (position.X + size.X))) &&
                        //     ((position.Y <= mouse.Y) && (mouse.Y < (position.Y + size.Y))))
                        if (mySprites.mouseOn.Rect.IsHit(mouse))
                        {
                            if (!isMouseOn)
                            {
                                isMouseOn = true;
                                mySprites.mouseOn.IsDraw = true;
                            }
                        }
                        else
                        {
                            if (isMouseOn)
                            {
                                isMouseOn = false;
                                mySprites.mouseOn.IsDraw = false;
                            }
                        }

                        if (damageTimer != -1)
                        {
                            damageTimer++;
                            if (damageTimerMax < damageTimer) damageTimer = -1;
                            SetPosition(position); // Todo : 毎アップデート時に呼び出さないといけない？
                        }

                        {
                            var val = (double)character.HP.Now / (double)character.HP.Max;
                            mySprites.hpBar.Rect.Size.X = 60 * val;
                        }
                    }
                    break;
                case Step.Lost:
                    {
                        {
                            var val = (double)character.HP.Now / (double)character.HP.Max;
                            mySprites.hpBar.Rect.Size.X = 60 * val;
                        }

                        mySprites.mouseOn.IsDraw = false;
                        stepTimer++;
                        if (damageTimer != -1)
                        {
                            damageTimer++;
                            if (damageTimerMax < damageTimer) damageTimer = -1;
                            SetPosition(position); // Todo : 毎アップデート時に呼び出さないといけない？
                        }

                        var soundTime = 10.0;
                        var EndWaitTime = 15.0;
                        if (stepTimer >= 10)
                        {
                            var v = 1.0 - (stepTimer - soundTime) / EndWaitTime;
                            mySprites.face.Color.A = v;
                            mySprites.shadow.Color.A = v;

                            mySprites.hpBarFrame.Color.A = v;
                            mySprites.hpBar.Color.A = v;
                        }

                        if (stepTimer == soundTime)
                        {
                            var s = new Emugen.Sound.SoundPlayer( Config.MainConfig.BattleScene.SoundEffectCharacterKO, 0.6f, false, Emugen.Sound.SoundPlayer.SoundType.SE);
                        }
                        else if (stepTimer >= (soundTime + EndWaitTime))
                        {
                            isLost = true;
                        }
                    }
                    break;
            }

            if (actionEffectUpdate != null)
            {
                SetPosition(position);
            }

            if (mouseOnTarget.IsDraw)
            {
                var max = 30.0;
                mouseOnTarget.Color.A = Math.Sin((double)mouseOnTargetFadeTimer * 360 / max * Math.PI / 180) * 0.5 + 0.5;

                mouseOnTargetFadeTimer++;
                if (mouseOnTargetFadeTimer > max) mouseOnTargetFadeTimer = 0;
            }

            autoDisposer.Update();
        }

        public void SetPosition(Vector2D position)
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
                mySprites.face.Rect.Position = new Vector2D(position.X+ offsetX, position.Y + offsetY);
                mySprites.shadow.Rect.Position = new Vector2D(position.X+ offsetX + cardImage.Width * character.imageScale / 2 - mySprites.shadow.Rect.Size.X / 2, position.Y + offsetY + size.Y - mySprites.shadow.Rect.Size.Y / 2 - 6);
                mouseOnTarget.Rect.Position = new Vector2D(position.X + offsetX - mouseOnTargetMargin, position.Y + offsetY - mouseOnTargetMargin);
            }
            else
            {
                var oofsetX = Math.Cos(damageTimer * 0.7) * 10 * ((damageTimerMax - damageTimer) / (damageTimerMax));
                mySprites.face.Rect.Position = new Vector2D(position.X + offsetX + oofsetX, position.Y + offsetY);
                mySprites.shadow.Rect.Position = new Vector2D(position.X + offsetX + cardImage.Width * character.imageScale / 2 - mySprites.shadow.Rect.Size.X / 2 + oofsetX, position.Y + offsetY + size.Y - mySprites.shadow.Rect.Size.Y / 2 - 6);
                mouseOnTarget.Rect.Position = new Vector2D(position.X + offsetX + oofsetX - mouseOnTargetMargin, position.Y + offsetY - mouseOnTargetMargin);
            }

            mySprites.mouseOn.Rect.Position = new Vector2D(position.X, position.Y);

            {
                var x = position.X + 98;
                var y = position.Y + 232 + offsetY;
                mySprites.hpBar.Rect.Position = new Vector2D(x + offsetX, y);
                mySprites.hpBarFrame.Rect.Position = new Vector2D(x + offsetX - 1, y - 1);
            }

            {
                var i = 0;
                foreach (var element in mySprites.element)
                {
                    var x = position.X + 98 + i * 32 + offsetX;
                    var y = position.Y + 225 + 22 + offsetY;
                    element.Rect.Position = new Vector2D(x, y);
                    i++;
                }
            }
        }

        public override void Draw()
        {
            layer.Draw();
        }

        public override void AddElement(Data.Element element)
        {
            var elementNameToFilePath = new Dictionary<string, string>();
            foreach( var i in Config.MainConfig.Elements)
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

        public override void RemoveElement(int index)
        {
            {
                var e = mySprites.element[index];
                layer.Del(e);
                mySprites.element.RemoveAt(index);
            }

            SetPosition(this.position);
        }

        public override void RefreshViewHP()
        {
        }

        public override void ShowDamage(int value)
        {
            var damage = new Effect.Damage(value, new Vector2D(this.position.X + mySprites.face.Rect.Size.X / 2, this.position.Y + mySprites.face.Rect.Size.Y / 2), autoDisposer);
            layer.Add(damage, 50);
        }
        public override void ShowMiss()
        {
            var e = new Effect.Miss(new Vector2D(this.position.X + mySprites.face.Rect.Size.X / 2, this.position.Y + mySprites.face.Rect.Size.Y / 2), autoDisposer);
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
            if (actionSkill.Main.EffectWord.IndexOf("HPダメージ") == 0)
            {
                actionEffectUpdate = new ActionEffectUpdate("data/script/action_01.cs", action, autoDisposer);
            }
            else if (actionSkill.Main.EffectWord.IndexOf("HP回復") == 0)
            {
                actionEffectUpdate = new ActionEffectUpdate("data/script/action_01.cs", action, autoDisposer);
            }
        }

    }
}
