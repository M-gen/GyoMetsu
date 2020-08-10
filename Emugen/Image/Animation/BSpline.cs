using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;

namespace Emugen.Image.Animation
{
    // todo : 削除予定
    public class BSpline
    {
        BSplineFragment bSpline;
        List<Vector2D> points = new List<Vector2D>();
        double step = 0.05;
        public double minX = 999999;
        public double maxX = 0;

        public BSpline(BSplineFragment bSpline )
        {
            this.bSpline = bSpline;
            for( var i=0.0; i<1; i+=step )
            {
                var point = bSpline.Value(i);
                points.Add(point);

                minX = System.Math.Min(minX, point.X);
                maxX = System.Math.Max(maxX, point.X);
            }
        }

        // T(開始地点を0、終了地点を1とする)に従って、Yの値を返す（スプライン曲線はtを利用するための、このような取得は加工が必要になる
        public double GetYByT( double x )
        {
            if (x == points[0].X)
            {
                return points[0].Y;
            }
            if (x == points[points.Count-1].X)
            {
                return points[points.Count - 1].Y;
            }

            for (var i=0; i<(points.Count-1); i++)
            {
                if ( ( points[i].X <= x ) && ( x < points[i+1].X ) ) {
                    var addY = ( points[i + 1].Y - points[i].Y ) * ( x - points[i].X) / (points[i+1].X - points[i].X) ;
                    return addY + points[i].Y;
                }
            }

            return 0;
        }
    }
}
