using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Image.Primitive;
//using Emugen.Image.Drawing;
using Emugen.Resource;

using GyoMetsu.Data;

namespace GyoMetsu.Scene
{

    public class TestScene : Emugen.OpenTK.Scene
    {
        LayerSprite layer = new LayerSprite();
        AutoDisposer autoDisposer = new AutoDisposer();

        public TestScene(string talkScriptPath)
        {
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            layer.Draw();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
