using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;

namespace GyoMetsu.DrawUtility
{
    public class DrawFrame
    {
        // シルエットから枠線用のエフェクトを作る
        // Todo : 処理が遅い
        static public System.Drawing.Bitmap Draw( System.Drawing.Bitmap src, int margin, double frameSizeA, double frameSizeB, Color color )
        {
            var iColor = new Emugen.Image.Drawing.Color((int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255), (int)(color.A * 255));
            var dst = new System.Drawing.Bitmap(src.Width + margin * 2, src.Height + margin * 2);
            using (var g = System.Drawing.Graphics.FromImage(dst))
            {
                g.DrawImage(src, new System.Drawing.Rectangle(margin, margin, src.Width, src.Height));
            }

            var image = new Emugen.Image.Drawing.EmugenImage(dst);

            var plane = new Emugen.Image.Drawing.EmugenPlaneImage((int)(frameSizeA * 2 + 1), (int)(frameSizeA * 2 + 1));
            plane.DrawCircle(frameSizeA, frameSizeA, frameSizeA, 1);
            //plane.Normalization();
            var image2 = image.Filter(plane, 255);
            image2.FillRGB(iColor);

            var plane2 = new Emugen.Image.Drawing.EmugenPlaneImage((int)(frameSizeB * 2 + 1), (int)(frameSizeB * 2 + 1));
            plane.DrawCircle(frameSizeB, frameSizeB, frameSizeB, 1);
            plane.Normalization();
            var image3 = image2.Filter(plane, 255);
            image3.FillRGB(iColor);

            return image3.ToBitmap();
        }
    }
}
