using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emugen.OpenTK
{
    public class Scene : IDisposable
    {
        public virtual void Update() { }
        public virtual void Draw() { }
        public virtual void Dispose() { }
    }
}
