using System;
using System.Collections.Generic;
using System.Text;

namespace Emugen.Image.Primitive
{
    public class Vector3D
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3D(double x = 0, double y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
