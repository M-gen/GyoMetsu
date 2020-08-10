using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emugen.OpenTK
{
    public class Runner
    {
        Window window;
        Input.InputCore inputCore;

        public Runner( string title )
        {
            inputCore = new Input.InputCore();

            window = new Emugen.OpenTK.Window(1600, 900, title);
        }

        public void Run( Scene scene, double updateRate = 30)
        {
            window.scenes.Add(scene);
            window.Run(updateRate);
        }
    }
}
