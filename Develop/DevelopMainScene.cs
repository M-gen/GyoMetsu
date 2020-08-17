using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Image.Primitive;



namespace Develop
{
    class DevelopMainScene : Scene
    {
        LayerSprite layer = new LayerSprite();

        [DllImport("EmugenDLL.dll")]
        static extern IntPtr Image_Create(uint width, uint height, IntPtr data);

        [DllImport("EmugenDLL.dll")]
        static extern void Image_ChangeFullRGB(IntPtr pImage, uint r, uint g, uint b);

        [DllImport("EmugenDLL.dll")]
        static extern void Image_Delete(IntPtr pImage);

        public DevelopMainScene()
        {
            var bitmap = new System.Drawing.Bitmap("data/image/enm/01_スライム.png");
            var height = bitmap.Height;
            var width  = bitmap.Width;
            //size = this.width * this.height;
            //this.colors = new Color[this.width * this.height];

            var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmap_data = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var stride = bitmap_data.Stride;
            var ptr = bitmap_data.Scan0;
            var pixels = new byte[bitmap_data.Stride * bitmap.Height];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);
            bitmap.UnlockBits(bitmap_data);
            bitmap.Dispose();

            var data = Marshal.AllocHGlobal(pixels.Length);
            Marshal.Copy(pixels, 0, data, pixels.Length);
            Marshal.FreeHGlobal(data);

            unsafe
            {

                var image = Image_Create((uint)width, (uint)height, data);
                Image_ChangeFullRGB(image, 255, 0, 0);
            }


            //var position = new Vector2D(100, 100);
            //{
            //    var font = new Font(

            //        "data/font/rounded-mgenplus-1cp-bold.ttf",
            //        16,
            //        new Color(1, 1, 1, 1),
            //        new Font.FontFrame[] { new Font.FontFrame(3, new Color(1, 1, 0, 0)), new Font.FontFrame(3, new Color(1, 0, 1, 0)) },
            //        100);
            //    var x = position.X;
            //    var y = position.Y;
            //    var ts = new TextSprite("abcd", font, new Vector2D(x, y), TextSprite.Type.Separation, null);
            //    layer.Add(ts, 20);
            //}

        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            layer.Draw();
        }
    }
}
