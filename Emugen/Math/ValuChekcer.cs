using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emugen.Math
{
    static public class ValuChekcer
    {
        static public void CheckMinMaxDouble(ref double t, double min, double max)
        {
            if (t < min) t = min;
            if (max < t) t = max;
        }
    }
}
