using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emugen.Random
{
    public class RundamIDManager
    {
        public List<string> ids = new List<string>();
        public System.Random random = new System.Random();

        int length;
        string idHead;

        public RundamIDManager( string idHead,  int length = 30 )
        {
            this.idHead = idHead;
            this.length = length;
        }

        public string GetNewID()
        {
            const string src = "abcdefghijklmnopqrstuvwxyz0123456789";
            var tmp = "";

            var isCreate = true;
            while (isCreate)
            {
                tmp = idHead;

                for (var i = 0; i < length; i++)
                {
                    var r = random.Next(0, src.Length);
                    tmp += src[r];
                }

                var isOK = true;
                foreach( var id in ids)
                {
                    if(id==tmp)
                    {
                        isOK = false;
                        break;
                    }
                }

                if (isOK)
                {
                    isCreate = false;
                }
            }


            ids.Add(tmp);
            return tmp;
        }

        public void ReleaseID( string id )
        {
            ids.Remove(id);
        }


    }
}
