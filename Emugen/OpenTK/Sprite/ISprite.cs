using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emugen.OpenTK.Sprite
{
    public class ISprite : Emugen.Resource.UpdateAndAutoDispose
    {
        public bool IsDraw = true;
        public virtual void Draw() { }
    }
}
