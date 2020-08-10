using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Data
{
    public class ElementLing
    {
        public List<string> Bodys = new List<string>();
        public int pos = 0;

        public Element[] NextElements()
        {
            if (Bodys.Count == 0)
            {
                return null;
            }

            var a = Bodys[pos];
            pos++;
            if (pos >= Bodys.Count) pos = 0;

            var elements = new List<Element>();
            foreach (var i in a)
            {
                var e = new Element(i.ToString());
                elements.Add(e);
            }

            return elements.ToArray();
        }
    }
}
