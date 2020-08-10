using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;

namespace Emugen.Image.Animation
{
    // Bスプライン曲線を柔軟にX座標からY座標を取得できるように仲介するクラス
    // Xに対し1つのYが決まること
    // 厳密には正しい値ではない
    public class BSplineXtoY
    {
        BSplineFragment bSplineFragment;
        int detail;
        Vector2D[] points;
        double startX;
        double endX;

        public BSplineXtoY( Vector2D[] points, int detail)
        {
            bSplineFragment = new BSplineFragment(points);
            this.detail = detail;

            this.points = new Vector2D[detail+1];

            for( var i=0; i<= detail; i++)
            {
                double i2 = (double)i / detail;
                this.points[i] = bSplineFragment.Value(i2);

                startX = System.Math.Min(startX, this.points[i].X);
                endX   = System.Math.Max(startX, this.points[i].X);
            }

        }

        public double ValueXtoY( double x)
        {
            if ( x==startX)
            {
                return points[0].Y;
            }
            if ( x==endX)
            {
                return points[detail].Y;
            }


            for(var i=0;i< detail; i++)
            {
                var point1 = points[i];
                var point2 = points[i+1];
                if ( (point1.X <= x) && ( x < point2.X) )
                {
                    var range = point2.X - point1.X;
                    var point2_par = (x - point1.X) / range;
                    var point1_par = 1.0 - point2_par;
                    return point1.Y * point1_par + point2.Y * point2_par;
                    //return point1.Y;
                }
            }

            return 0;
        }

    }
}
