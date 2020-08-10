using System;
using System.Collections.Generic;
using System.Text;

namespace Emugen.Image.Primitive
{
    public class Vector2D
    {
        public double X;
        public double Y;

        public Vector2D(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }
        public Vector2D(Vector2D src)
        {
            X = src.X;
            Y = src.Y;
        }
    }
}
