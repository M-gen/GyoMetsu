using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Data
{
    public class ScriptFlags
    {
        static public ScriptFlags Instance;

        Dictionary<string, object> items = new Dictionary<string, object>();

        public ScriptFlags()
        {
            Instance = this;
        }

        public object Get( string key )
        {
            if (items.ContainsKey(key))
            {
                return items[key];
            }
            else
            {
                return null;
            }
        }

        public void Set( string key, object value)
        {
            if (items.ContainsKey(key))
            {
                items[key] = value;
            }
            else
            {
                items.Add(key, value);
            }
        }

        public void Del( string key )
        {
            if (items.ContainsKey(key))
            {
                items.Remove(key);
            }

        }
    }
}
