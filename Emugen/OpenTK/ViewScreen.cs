using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.PixelFormats;

//using Emugen.GameCore;
//using Emugen.Resource;
using Emugen.Image.Primitive;

namespace Emugen.OpenTK
{
    public class ViewScreen // Todo 削除予定
    {
        public static ViewScreen Instance;

        public Vector2D ScreenSize;

        public ViewScreen()
        {
            Instance = this;
        }

        public void Reset()
        {
            GL.Viewport(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y);

            var wph = (double)ScreenSize.X / (double)ScreenSize.Y;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / 4, (float)wph, (float)1f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

        }
        public void ResetO()
        {
            GL.Viewport(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y);

            //var wph = (double)ScreenSize.X / (double)ScreenSize.Y;
            var projection = Matrix4.CreateOrthographic((float)ScreenSize.X, (float)ScreenSize.Y, (float)0.1f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            //// 正射影
            //GL.MatrixMode(MatrixMode.Projection);
            //float h = 4.0f, w = h * glControl.AspectRatio;
            //Matrix4 projection = Matrix4.CreateOrthographic(w, h, 0.1f, 2.0f);
            //GL.LoadMatrix(ref projection);

        }

    }
}
