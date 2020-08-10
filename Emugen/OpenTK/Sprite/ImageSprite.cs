using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Emugen.Image.Primitive;

namespace Emugen.OpenTK.Sprite
{
    public class ImageSprite : ISprite, IDisposable
    {
        public Emugen.Image.Primitive.Rect  Rect;
        public Emugen.Image.Primitive.Color Color;
        public Emugen.OpenTK.View.IView     view;
        public Emugen.OpenTK.Texture        texture;

        public bool AjustClipMode = true;

        public ImageSprite(Emugen.OpenTK.Texture texture, Emugen.Image.Primitive.Rect rect, Emugen.Image.Primitive.Color color, Emugen.OpenTK.View.IView view = null)
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
            this.texture = texture;
        }

        public override void Draw()
        {
            view.Bind();

            var modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.BlendFunc(BlendingFactor.Src1Alpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

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

            texture.Bind();

            //GL.Begin(BeginMode.Quads); // TriangleStrip
            GL.Begin(BeginMode.TriangleStrip); // 
            //GL.Begin(BeginMode.Lines);
            //GL.Vertex3(-w, h, z);
            //GL.Vertex3(-w, -h, z);
            //GL.Vertex3(w, -h, z);
            //GL.Vertex3(w, h, z);
            var u1 = 0.0f;
            var v1 = 0.0f;
            var u2 = 1.0f;
            var v2 = 1.0f;

            GL.TexCoord2(u1, v1); GL.Vertex3(+w, +h, z);
            GL.TexCoord2(u1, v2); GL.Vertex3(+w, -h, z);
            GL.TexCoord2(u2, v1); GL.Vertex3(-w, +h, z);
            GL.TexCoord2(u2, v2); GL.Vertex3(-w, -h, z);

            GL.End();
        }

        public void Dispose()
        {
            texture.Dispose();
        }

    }


}
