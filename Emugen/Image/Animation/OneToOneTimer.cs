using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emugen.Image.Animation
{
    public class OneToOneTimer
    {
        public double timer = 0;
        public double timerMax;
        public double start;
        public double end;

        public OneToOneTimer( double start, double end, double timerMax )
        {
            this.start = start;
            this.end = end;
            this.timerMax = timerMax;
        }

        public void Update()
        {
            timer++;
            if (timer >= timerMax)
            {
                timer = timerMax;
                isEnd = true;
            }
        }

        public double Value
        {
            get
            {
                return ( start * (timerMax - timer) / timerMax )  + ( end * timer / timerMax );
            }
        }

        bool isEnd = false;
        public bool IsEnd { get { return isEnd; } }
         
    }
}
