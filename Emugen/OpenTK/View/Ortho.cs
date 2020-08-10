using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Emugen.Image.Primitive;

namespace Emugen.OpenTK.View
{
    public class Ortho : IView
    {

        public Vector2D ScreenSize;

        public Ortho( Vector2D size )
        {
            ScreenSize = size;
        }
            

        public override void Bind()
        {
            GL.Viewport(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y);

            var projection = Matrix4.CreateOrthographic((float)ScreenSize.X, (float)ScreenSize.Y, (float)0.1f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
    }
}
