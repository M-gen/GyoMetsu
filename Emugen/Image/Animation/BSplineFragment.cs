using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Emugen.OpenTK;
//using Emugen.OpenTK.Sprite;
using Emugen.Image.Primitive;

namespace Emugen.Image.Animation
{
    // B-スプライン曲線
    public class BSplineFragment
    {
        Vector2D[] points;
        int sectionNum;

        public BSplineFragment(Vector2D[] points)
        {
            this.points = points;
            sectionNum = (points.Length - 1) / 2;
        }

        public Vector2D Value( double t)
        {
            if ( t <= 0 )
            {
                var i = 0;
                return new Vector2D(points[i].X, points[i].Y);
            }
            else if ( t >= 1 )
            {
                var i = points.Length-1;
                return new Vector2D(points[i].X, points[i].Y);
            }
            else
            {
                var pos = t * sectionNum;
                for(var i=0; i<sectionNum; i++)
                {
                    //if ( i < pos)
                    if ( pos < i + 1)
                    {
                        var t2 = pos - i;
                        return GetVector2D(t2, i * 2);
                    }
                }
            }

            {
                var i = 0;
                return new Vector2D(points[i].X, points[i].Y);
            }
        }

        private Vector2D GetVector2D( double t, int offset)
        {

            var x = (1 - t) * (1 - t) * points[0 + offset].X + 2 * t * (1 - t) * points[1 + offset].X + t * t * points[2 + offset].X;
            var y = (1 - t) * (1 - t) * points[0 + offset].Y + 2 * t * (1 - t) * points[1 + offset].Y + t * t * points[2 + offset].Y;
            return new Vector2D(x, y);
        }
    }
}
