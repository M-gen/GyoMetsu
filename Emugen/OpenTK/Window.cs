using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Emugen.OpenTK
{
    public class WindowManager
    {
        public static Scene nextScene;

        public static void Close()
        {
            Window.Instance.Close();
        }

        public static System.Drawing.Bitmap ScreenShot()
        {
            var window = Window.Instance;
            var bmp = new System.Drawing.Bitmap(window.ClientSize.Width, window.ClientSize.Height);
            var pos =  window.PointToScreen(new System.Drawing.Point(0, 0));

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(pos.X, pos.Y, 0, 0,
                  window.ClientSize, System.Drawing.CopyPixelOperation.SourceCopy);

                // ビット・ブロック転送方式の切り替え例：
                //g.FillRectangle(Brushes.LightPink,
                //  0, 0, rc.Width, rc.Height);
                //g.CopyFromScreen(rc.X, rc.Y, 0, 0,
                //  rc.Size, CopyPixelOperation.SourceAnd);
            }
            return bmp;
        }
    }

    public class Window : GameWindow
    {
        private static Window instance;
        public static Window Instance { get { return instance; } }

        public Emugen.OpenTK.View.IView defaultOrthoView;
        public Emugen.Image.Primitive.Vector2I GameScreenSize;

        public List<Scene> scenes = new List<Scene>();

        public Window(int width, int height, string title)
               : base(width, height, GraphicsMode.Default, title)
        {
            instance = this;
            GameScreenSize = new Image.Primitive.Vector2I(width, height);
            defaultOrthoView = new View.Ortho( new Image.Primitive.Vector2D(width, height));

            new TextureResourceManager();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //GL.Enable(EnableCap.DepthTest);

            //viewScreen.SetView(ClientRectangle.Width, ClientRectangle.Height);
            ////this.WindowBorder = WindowBorder.Hidden;


        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //GameCore.GameCore.Instance.Size = new Emugen.Image.Vector2D(ClientRectangle.Width, ClientRectangle.Height);

            //viewScreen.SetView(ClientRectangle.Width, ClientRectangle.Height);
            //Emugen.GameCore.Input.InputCore.Instance.AddEvent($"TopWindow-Resize {ClientRectangle.Width} {ClientRectangle.Height}");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Input.InputCore.Instance.Update();

            //gameCore.Update();
            //this.WindowState = new WindowState();

            if (WindowManager.nextScene != null)
            {
                scenes[0].Dispose();
                scenes.RemoveAt(0);
                scenes.Insert(0, WindowManager.nextScene);
                WindowManager.nextScene = null;
            }

            if (scenes.Count>0)
            {
                scenes[0].Update();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor((float)0.2, (float)0.2, (float)0.2, (float)1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Texture2D);


            if (scenes.Count > 0)
            {
                scenes[0].Draw();
            }

            this.SwapBuffers();
        }



        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            var i = Input.InputCore.Instance.mousePosition;
            i.X = e.X;
            i.Y = e.Y;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            var i = Input.InputCore.Instance.mousePosition;
            i.X = e.X;
            i.Y = e.Y;

            switch (e.Button)
            {
                case MouseButton.Left:
                    Input.InputCore.Instance.DoKeyEvent(Input.InputCore.KeyEventCode.MouseLeftButton, Input.InputCore.KeyEventType.DownUpdateBefor);
                    break;
                case MouseButton.Right:
                    Input.InputCore.Instance.DoKeyEvent(Input.InputCore.KeyEventCode.MouseRightButton, Input.InputCore.KeyEventType.DownUpdateBefor);
                    break;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            var i = Input.InputCore.Instance.mousePosition;
            i.X = e.X;
            i.Y = e.Y;

            switch(e.Button)
            {
                case MouseButton.Left:
                    Input.InputCore.Instance.DoKeyEvent(Input.InputCore.KeyEventCode.MouseLeftButton, Input.InputCore.KeyEventType.UpUpdateBefor);
                    break;
                case MouseButton.Right:
                    Input.InputCore.Instance.DoKeyEvent(Input.InputCore.KeyEventCode.MouseRightButton, Input.InputCore.KeyEventType.UpUpdateBefor);
                    break;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            //gameCore.OnMouseWheel(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            //gameCore.OnKeyDwon(e);

        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            //gameCore.OnKeyUp(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            foreach (var scene in scenes) scene.Dispose();
            scenes.Clear();


            //gameCore.OnClose();
            Sound.SoundPlayer.Clear();
            Sound.SoundPlayer.systemClose = true;

            GC.Collect();
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            //base.OnFocusedChanged(e);
            //if (this.Focused)
            //{
            //    Console.WriteLine("OnFocusedChanged True");
            //}
            //else
            //{
            //    Console.WriteLine("OnFocusedChanged False");
            //}
        }

        //public void NextScene( Scene scene )
        //{
        //    nextScene = scene;
        //}

    }

}
