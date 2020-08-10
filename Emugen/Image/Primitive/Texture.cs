using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.PixelFormats;

////using Emugen.Resource;


//namespace Emugen.Image.Primitive
//{
//    public class Texture : IDisposable
//    {
//        const int NullTexture = -1;

//        public int glTexture = NullTexture;

//        public Vector2I TextureSize;


//        public Texture( SixLabors.ImageSharp.Image<Rgba32> image )
//        {

//            //lock (OpenTK.Manage.Instance.openTKLock)
//            OpenTK.Manage.Instance.openTKLock.Enter();
//            {
//                glTexture = OpenTK.Manage.Instance.GenTexture();
//                Bind();

//                TextureSize = new Vector2I(image.Width, image.Height);
//                float[,,] colors = new float[image.Height, image.Width, 4];

//                for (int x = 0; x < TextureSize.X; x++)
//                {
//                    for (int y = 0; y < TextureSize.Y; y++)
//                    {
//                        var p = image[x, y];
//                        colors[y, x, 0] = (float)p.R / 255.0f;
//                        colors[y, x, 1] = (float)p.G / 255.0f;
//                        colors[y, x, 2] = (float)p.B / 255.0f;
//                        colors[y, x, 3] = (float)p.A / 255.0f;
//                    }
//                }

//                OpenTK.Manage.Instance.TexImage2D( glTexture, image.Width, image.Height, colors);
//                //テクスチャの設定
//                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
//                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
//                //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.Float, colors);
//            }
//            OpenTK.Manage.Instance.openTKLock.Exit();
//        }

//        //public Image(  )
//        //{
//        //    SixLabors.ImageSharp.Image.Load()
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        public void UpdateTexture(string filePath)
//        {

//        }

//        public void Bind()
//        {
//            //if (glTexture!=NullTexture) {
//            //    GL.DeleteTexture(glTexture);
//            //}
//            GL.BindTexture(TextureTarget.Texture2D, glTexture);
//        }

//        private void DisposeGLTexture()
//        {
//            if (glTexture != NullTexture)
//            {
//                GL.DeleteTexture(glTexture);
//                glTexture = NullTexture;
//            }
//        }

//        public void Dispose()
//        {
//            DisposeGLTexture();
//        }

//    }
//}
