using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using OpenTK_Graphics = OpenTK.Graphics;

using Emugen.Image.Primitive;


namespace Emugen.OpenTK
{
    public class Texture : IDisposable
    {
        const int NullTexture = -1;
        public int glTexture = NullTexture;
        public Vector2I Size;

        public Func<bool> disposeFunctions;

        public Texture( Bitmap bmp )
        {
            Size = new Vector2I(bmp.Width, bmp.Height);

            glTexture = GL.GenTexture();
            Bind();

            //テクスチャの設定
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK_Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, glTexture);
        }

        private void DisposeGLTexture()
        {
            if (glTexture != NullTexture)
            {
                GL.DeleteTexture(glTexture);
                glTexture = NullTexture;
            }
        }

        public void Dispose()
        {
            if ((disposeFunctions != null) && (disposeFunctions() == false))
            {
                // TextureResourceManagerでテクスチャ―の管理をしているので、解放しきってはいけない
            }
            else
            {
                DisposeGLTexture();
            }
        }

    }
}
