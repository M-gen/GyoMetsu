using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Emugen.Image.Primitive
{

    public class Color
    {
        public double A;
        public double R;
        public double G;
        public double B;

        public Color(double a, double r, double g, double b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
            Validation();
        }

        private void Validation()
        {
            Emugen.Math.ValuChekcer.CheckMinMaxDouble(ref A, 0, 1);
            Emugen.Math.ValuChekcer.CheckMinMaxDouble(ref R, 0, 1);
            Emugen.Math.ValuChekcer.CheckMinMaxDouble(ref G, 0, 1);
            Emugen.Math.ValuChekcer.CheckMinMaxDouble(ref B, 0, 1);
        }

        public void SetGLColor()
        {
            GL.Color4(R, G, B, A);
        }

        public System.Drawing.Color ToSystemDrawingColor()
        {
            return System.Drawing.Color.FromArgb((int)(255 * A), (int)(255 * R), (int)(255 * G), (int)(255 * B));
        }

        //public Rgba32 ToRGBA32()
        //{
        //    //var res = new Rgba32((byte)(A * 255), (byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        //    var res = new Rgba32((byte)(R * 255), (byte)(G * 255), (byte)(B * 255), (byte)(A * 255));
        //    return res;
        //}
    }
}
