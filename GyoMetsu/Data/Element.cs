using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Data
{
    public class Element
    {
        public string Name;

        public Element( string name)
        {
            Name = name;
        }

        // 所持エレメントと、コストから支払えるかを算出する
        public static bool IsUsable( List<Element> haveElementsSrc, string costSrc )
        {
            var elementString = "祈樹獣理鉄";

            var cost = new Dictionary<string, int>();
            cost.Add("祈", 0);
            cost.Add("樹", 0);
            cost.Add("獣", 0);
            cost.Add("理", 0);
            cost.Add("鉄", 0);

            var have = new Dictionary<string, int>();
            have.Add("祈", 0);
            have.Add("樹", 0);
            have.Add("獣", 0);
            have.Add("理", 0);
            have.Add("鉄", 0);

            var costElementNum = 0;
            var haveElementNum = 0;
            var anyElementCost = 0;

            foreach( var i in costSrc)
            {
                if ("0123456789".IndexOf(i)>=0)
                {
                    // 数値である
                    anyElementCost = int.Parse(i.ToString());
                }
                else
                {
                    // エレメント
                    cost[i.ToString()]++;
                    costElementNum++;
                }
            }

            foreach (var i in haveElementsSrc)
            {
                have[i.Name]++;
                haveElementNum++;
            }

            var isOK = true;

            foreach( var i in elementString)
            {
                var es = i.ToString();
                if (cost[es] > have[es]) isOK = false; 
            }
            if (haveElementNum < costElementNum + anyElementCost) isOK = false;
            

            return isOK;
        }
    }

}
