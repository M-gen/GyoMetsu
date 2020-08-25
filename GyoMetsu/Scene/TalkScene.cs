using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Image.Primitive;

namespace GyoMetsu.Scene
{

    public class TalkScene : Emugen.OpenTK.Scene
    {
        LayerSprite layer = new LayerSprite();

        TextSprite[] textSprites = new TextSprite[3];
        int targetTextSpriteIndex = 0;

        ImageSprite ImageSpriteTalkCharacter;
        ImageSprite ImageSpriteBackGround;
        ImageSprite ImageSpriteWaitEnterIcon;

        int waitEnterIconTimer = 0;
        int waitEnterIconTimerMax = 10;
        int waitEnterIconTimerMove = 10;

        TextSprite CharacterViewNameTextSprite;

        Emugen.Image.Animation.OneToOneTimer fadeTimer = new Emugen.Image.Animation.OneToOneTimer(0, 1, 20);
        Emugen.Image.Animation.OneToOneTimer slideTimer = new Emugen.Image.Animation.OneToOneTimer(0, 20, 20);
        Vector2D characterPosition = new Vector2D(925, 0);

        Emugen.Image.Animation.BSplineXtoY fadeTimerSpline;
        int fadeTimerTime = 0;
        int fadeTimerTimeMax = 20;

        Emugen.Image.Animation.BSplineXtoY slideTimerSpline;
        int slideTimerTime = 0;
        int slideTimerTimeMax = 20;

        ScriptAPI scriptAPI;
        int textLineNum = 0;

        string talkScriptPath;

        UI.Talk.Choice choice;

        ImageSprite sceneStartFadeImageSprite;
        int sceneStartFadeTimer = 0;
        bool sceneStartFadeStart = false;

        public TalkScene( string talkScriptPath )
        {
            this.talkScriptPath = talkScriptPath;

            // 前回（今現在）の画面を取得
            {
                //var bmp = new System.Drawing.Bitmap("data/image/bg/酒場.png");
                var bmp = WindowManager.ScreenShot();
                var texture = new Texture(bmp);
                var w = 1600;
                var h = (int)((double)bmp.Height / (double)bmp.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(0, 0), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite, 10000);

                sceneStartFadeImageSprite = sprite;
            }

            // 立ち絵のフェードで表示するための、イージングの数値を計算
            {
                fadeTimerSpline = new Emugen.Image.Animation.BSplineXtoY(
                    new Vector2D[] { new Vector2D(0, 0), new Vector2D(0, 1), new Vector2D(1, 1) },
                    100);
            }
            {
                slideTimerSpline = new Emugen.Image.Animation.BSplineXtoY(
                    new Vector2D[] { new Vector2D(0, 0), new Vector2D(0, 1), new Vector2D(1, 1) },
                    100);
            }

            // スクリプトの実行
            scriptAPI = new ScriptAPI();
            {
                var thread = new System.Threading.Thread( new System.Threading.ThreadStart(ThreadScriptRun));
                scriptAPI.thread = thread;
                thread.Start();
            }

            {
                var bmp = new System.Drawing.Bitmap(Config.MainConfig.TalkScene.NextWaitIcon);
                var texture = new Texture(bmp);
                var w = 30;
                var h = (int)((double)bmp.Height / (double)bmp.Width * w);
                var sprite = new ImageSprite(texture, new Rect(new Vector2D(1400+50, 600), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                layer.Add(sprite, 200);

                ImageSpriteWaitEnterIcon = sprite;
            }

            {
                var sprite = new PlaneSprite(new Rect(new Vector2D(100, 600), new Vector2D(300, 40)), new Color(0.5, 0, 0, 0));
                layer.Add(sprite, 150);
            }
            {
                var sprite = new PlaneSprite(new Rect(new Vector2D(100, 650), new Vector2D((1600 - 200), 40+50*3)), new Color(0.5, 0, 0, 0));
                layer.Add(sprite, 150);
            }

        }

        void ThreadScriptRun()
        {
            var script = new Emugen.Script.Script<ScriptAPI>( talkScriptPath, scriptAPI);
            script.Run();

        }

        public override void Update()
        {
            if (targetTextSpriteIndex < textSprites.Length)
            {
                if (textSprites[targetTextSpriteIndex] != null)
                {
                    textSprites[targetTextSpriteIndex].Update();
                }
            }

            if(scriptAPI.__waitEnter)
            {
                waitEnterIconTimer++;
                if (waitEnterIconTimer > waitEnterIconTimerMax) waitEnterIconTimer = 0;
                ImageSpriteWaitEnterIcon.Rect.Position.Y = 800 - 10 + ((double)waitEnterIconTimer / waitEnterIconTimerMax) * waitEnterIconTimerMove;
                //ImageSpriteWaitEnterIcon.IsDraw = true;
            }
            else
            {
                ImageSpriteWaitEnterIcon.IsDraw = false;
            }

            if (ImageSpriteTalkCharacter != null)
            {
                {
                    if (fadeTimerTime < fadeTimerTimeMax)
                    {
                        var x = (double)fadeTimerTime / fadeTimerTimeMax;
                        var y = fadeTimerSpline.ValueXtoY(x);

                        ImageSpriteTalkCharacter.Color.A = y;
                        fadeTimerTime++;
                    }
                }

                {
                    if (slideTimerTime < slideTimerTimeMax)
                    {
                        var x = (double)slideTimerTime / slideTimerTimeMax;
                        var y = slideTimerSpline.ValueXtoY(x);

                        var slide = 100;

                        ImageSpriteTalkCharacter.Rect.Position.X = characterPosition.X - slide * y + slide;
                        slideTimerTime++;
                    }
                }
            }

            if (scriptAPI.__waitEnter)
            {
                DoScript();

                var input = Emugen.Input.InputCore.Instance;
                if (input.GetKeyEventType(Emugen.Input.InputCore.KeyEventCode.MouseLeftButton) == Emugen.Input.InputCore.KeyEventType.Up) 
                {
                    if(choice!=null)
                    {
                        // 選択肢のクリック
                        var result = choice.GetChoiseText();
                        if (result!=null)
                        {
                            scriptAPI.__waitEnter = false;
                            scriptAPI.__choiseResult = choice.GetChoiseText();

                            layer.Del(choice);
                            choice = null;
                        }
                    }
                    else
                    {
                        // 通常の待機クリック
                        scriptAPI.__waitEnter = false;

                    }
                }
            }
            else if( scriptAPI.__wait > 0 )
            {
                scriptAPI.__wait--;
                DoScript();
            }

            if (sceneStartFadeStart && (sceneStartFadeTimer<30) )
            {
                sceneStartFadeTimer++;
                sceneStartFadeImageSprite.Color.A = 1.0 - sceneStartFadeTimer / 30.0;
            }

            if(choice!=null) choice.Update();
        }

        private void DoScript()
        {
            if (!sceneStartFadeStart) sceneStartFadeStart = true;

            if (scriptAPI.__isMainDo)
            {
                scriptAPI.__isMainDo = false;
                {
                    if (scriptAPI.__clearText)
                    {
                        for (var i = 0; i < textSprites.Length; i++)
                        {
                            if (textSprites[i] != null)
                            {
                                layer.Del(textSprites[i]);
                                textSprites[i].Dispose();
                                textSprites[i] = null;
                            }
                        }
                        textLineNum = 0;
                        targetTextSpriteIndex = 0;
                        scriptAPI.__clearText = false;
                    }

                    if (scriptAPI.__talkText != null)
                    {
                        lock (scriptAPI._lock__talkText)
                        {
                            var t = scriptAPI.__talkText;

                            var t2 = t.Split('\n');
                            foreach( var t3 in t2)
                            {
                                //if (t3 == "") continue;
                                var x = 100 + 5;
                                var y = 600 + 55 + 50 * textLineNum;
                                var font = new Font( Config.MainConfig.MainFontPath, 20, new Color(1, 1, 1, 1), new Font.FontFrame[] {
                                new Font.FontFrame(2, new Color(1,0,0,0)),
                                }, 0);
                                var ts = new TextSprite(t3, font, new Vector2D(x, y), TextSprite.Type.Separation, null, 0, 100);
                                layer.Add(ts, 200);
                                textSprites[textLineNum] = ts;
                                ts.OnShowEnd += () =>
                                {
                                    targetTextSpriteIndex++;
                                    if (textSprites.Count() <= targetTextSpriteIndex ) {
                                        ImageSpriteWaitEnterIcon.IsDraw = true;
                                    }
                                    else if ( textSprites[targetTextSpriteIndex] == null ) 
                                    {
                                        ImageSpriteWaitEnterIcon.IsDraw = true;
                                    }
                                };
                                ImageSpriteWaitEnterIcon.IsDraw = false;
                                textLineNum++;
                            }
                            scriptAPI.__talkText = null;
                        }
                    }

                    if (scriptAPI.__talkCharacterImagePath != null)
                    {
                        if (ImageSpriteTalkCharacter != null)
                        {
                            layer.Del(ImageSpriteTalkCharacter);
                            ImageSpriteTalkCharacter.Dispose();
                        }

                        var texture = TextureResourceManager.Instance.GetTexture(scriptAPI.__talkCharacterImagePath);
                        var w = texture.Size.X;// 1300;//1024;
                        var h = (int)((double)texture.Size.Y / (double)texture.Size.X * w);

                        var sprite = new ImageSprite(
                            texture,
                            new Rect(
                                new Vector2D(characterPosition.X, characterPosition.Y),
                                new Vector2D(w, h)),
                            new Color(1, 1, 1, 1)
                            );
                        layer.Add(sprite, 100);
                        ImageSpriteTalkCharacter = sprite;
                        ImageSpriteTalkCharacter.Color.A = 0;
                        fadeTimerTime = 0;
                        slideTimerTime = 0;

                        scriptAPI.__talkCharacterImagePath = null;
                    }

                    if (scriptAPI.__talkCharacterViewName != null)
                    {
                        if (CharacterViewNameTextSprite != null)
                        {
                            layer.Del(CharacterViewNameTextSprite);
                            CharacterViewNameTextSprite.Dispose();
                        }
                        var x = 100 + 5;
                        var y = 600;

                        var font = new Font( Config.MainConfig.MainFontPath, 20, new Color(1, 1, 1, 1), new Font.FontFrame[] {
                                new Font.FontFrame(2, new Color(1,0,0,0)),
                                }, 0);
                        var ts = new TextSprite(scriptAPI.__talkCharacterViewName, font, new Vector2D(x, y));
                        layer.Add(ts, 200);

                        CharacterViewNameTextSprite = ts;
                        scriptAPI.__talkCharacterViewName = null;
                    }

                    if( scriptAPI._nextBattleScript != null)
                    {
                        WindowManager.nextScene = new Scene.BattleScene(scriptAPI._nextBattleScript);

                        scriptAPI._nextBattleScript = null;
                    }

                    if (scriptAPI._nextTalkScript != null)
                    {
                        WindowManager.nextScene = new Scene.TalkScene(scriptAPI._nextTalkScript);

                        scriptAPI._nextTalkScript = null;
                    }

                    if (scriptAPI._nextTitle != null)
                    {
                        WindowManager.nextScene = new Scene.TitleScene();

                        scriptAPI._nextTitle = null;
                    }

                    if ( scriptAPI.__choiseText!=null ){

                        if(choice!=null) layer.Del(choice);

                        // todo : Choiceのなかで文字のDisposeがいるはず
                        choice = new UI.Talk.Choice( scriptAPI.__choiseText );
                        layer.Add(choice, 200);

                        scriptAPI.__choiseText = null;
                    }

                    if (scriptAPI.__backGroundImagePath != null)
                    {
                        // todo : 背景のリソースをクリーンにする処理が見当たらない

                        if (ImageSpriteBackGround!=null)
                        {
                            layer.Del(ImageSpriteBackGround);
                            ImageSpriteBackGround.Dispose();
                        }

                        var texture = TextureResourceManager.Instance.GetTexture(scriptAPI.__backGroundImagePath);

                        var w = 1600;
                        var h = (int)((double)texture.Size.Y / (double)texture.Size.X * w);
                        var sprite = new ImageSprite(texture, new Rect(new Vector2D(0, 0), new Vector2D(w, h)), new Color(1, 1, 1, 1));
                        layer.Add(sprite, 10);
                        ImageSpriteBackGround = sprite;

                        scriptAPI.__backGroundImagePath = null;
                    }

                    if (scriptAPI.__bgmPath != null)
                    {
                        var bgm = new Emugen.Sound.SoundPlayer(scriptAPI.__bgmPath, 0.20f, true, Emugen.Sound.SoundPlayer.SoundType.BGM);

                        scriptAPI.__bgmPath = null;
                    }

                    if (scriptAPI.__playSE != null)
                    {
                        var se = new Emugen.Sound.SoundPlayer(scriptAPI.__playSE, scriptAPI.__playSEVolume, false, Emugen.Sound.SoundPlayer.SoundType.SE);

                        scriptAPI.__playSE = null;
                    }
                }
            }
        }

        public override void Draw()
        {
            layer.Draw();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (scriptAPI!=null)
            {
                scriptAPI.thread.Abort();
            }

            layer.Dispose();
        }

        public class ScriptAPI
        {
            // todo : 多分こうしたほうがいい
            //public List<Action<object>> actions = new List<Action<object>>
            public System.Threading.Thread thread;

            public string __backGroundImagePath;
            public string __bgmPath;
            public string __talkText;
            public string __talkCharacterViewName;
            public string __talkCharacterImagePath;
            public string __playSE;
            public float __playSEVolume;

            public int  __wait = 0;
            public bool __isMainDo = false;
            public bool __clearText = false;
            public bool __waitEnter = false;

            public string[] __choiseText;
            public string __choiseResult;

            public object _lock__talkText = new object();

            public string _nextBattleScript;
            public string _nextTalkScript;
            public string _nextTitle;

            public void SetCharacterImage( string path )
            {
                __talkCharacterImagePath = path;
            }

            public void SetCharacterViewName(string name)
            {
                __talkCharacterViewName = name;
            }
            public void SetBackGroundImagePath(string backGroundImagePath)
            {
                __backGroundImagePath = backGroundImagePath;
            }

            public void SetBGM(string bgmPath)
            {
                __bgmPath = bgmPath;
            }

            public void PlaySound(string path, float volume = 0.20f)
            {
                __playSE = path;
                __playSEVolume = volume;
            }

            public void Talk( string text )
            {
                lock (_lock__talkText)
                {
                    __talkText = text;
                }
            }
            public void Wait( int time )
            {
                __isMainDo = true;
                __wait = time;
                while ( (__wait > 0) || (__isMainDo!=false) )
                {
                    Emugen.Thread.Sleep.Do(0);
                }
            }
            public void ClearText()
            {
                __clearText = true;
            }

            public string WaitChoise( string[] args )
            {
                __choiseText = args;

                __isMainDo = true;
                __waitEnter = true;
                while ((__waitEnter == true) || (__isMainDo != false))
                {
                    Emugen.Thread.Sleep.Do(0);
                }

                return __choiseResult;
            }

            public void WaitByEnter()
            {
                __isMainDo = true;
                __waitEnter = true;
                while ((__waitEnter == true) || (__isMainDo != false))
                {
                    Emugen.Thread.Sleep.Do(0);
                }
            }

            public void NextSceneBattle( string path )
            {
                _nextBattleScript = path;
            }
            public void NextSceneTalk(string path)
            {
                _nextTalkScript = path;
            }

            public void NextSceneTitle(string path = "")
            {
                _nextTitle = path;
            }

            public void SetValue(string key, object value)
            {
                Data.ScriptFlags.Instance.Set(key, value);
            }
            public object GetValue(string key)
            {
                return Data.ScriptFlags.Instance.Get(key);
            }

            public T GetValueDefault<T>(string key, T defaultValue)
            {
                var tmp = Data.ScriptFlags.Instance.Get(key);
                if( tmp==null )
                {
                    return defaultValue;
                }
                else
                {
                    return (T)tmp;
                }
            }

            public void DelValue(string key)
            {
                Data.ScriptFlags.Instance.Del(key);
            }

        }
    }
}
