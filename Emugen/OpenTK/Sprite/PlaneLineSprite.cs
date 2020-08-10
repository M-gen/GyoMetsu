using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;



namespace Emugen.OpenTK.Sprite
{
    public class PlaneLineSprite : ISprite
    {
        public Emugen.Image.Primitive.Rect Rect;
        public Emugen.Image.Primitive.Color Color;
        public Emugen.OpenTK.View.IView view;

        public PlaneLineSprite(Emugen.Image.Primitive.Rect rect, Emugen.Image.Primitive.Color color, Emugen.OpenTK.View.IView view = null)
        {
            if (view == null)
            {
                this.view = Window.Instance.defaultOrthoView;
            }
            else
            {
                this.view = view;
            }
            this.Rect = rect;
            this.Color = color;
        }

        public override void Draw()
        {
            view.Bind();

            var modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Disable(EnableCap.Texture2D);

            {
                var screenSize = Window.Instance.GameScreenSize;
                var x = screenSize.X / 2 - Rect.Position.X - Rect.Size.X / 2;
                var y = screenSize.Y / 2 - Rect.Position.Y - Rect.Size.Y / 2;
                GL.Translate(x, y, 0);
            }


            var w = Rect.Size.X / 2;
            var h = Rect.Size.Y / 2;
            var z = 1;
            Color.SetGLColor();

            //GL.Begin(BeginMode.Quads);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(-w, h, z);
            GL.Vertex3(-w, -h+1, z);


            GL.Vertex3(w, -h, z);
            GL.Vertex3(w, h, z);
            GL.End();
        }

    }
}
