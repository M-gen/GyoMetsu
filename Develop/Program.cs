using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Develop
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var runner = new Emugen.OpenTK.Runner("魚滅の槍");
            runner.Run(new DevelopMainScene());
        }
    }
}
