using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emugen.OpenTK
{

    public class TextureResourceManager
    {
        static public TextureResourceManager Instance;

        class FileResource
        {
            public string path;
            public System.Drawing.Bitmap bitmap;
            public Texture texture;
            public int count = 0;

        }
        Dictionary<string, FileResource> fileResources = new Dictionary<string, FileResource>();
        object fileResourcesLock = new object();

        public TextureResourceManager()
        {
            Instance = this;
        }

        public Texture GetTexture( string path )
        {
            lock (fileResourcesLock)
            {
                if (fileResources.ContainsKey(path))
                {
                    return fileResources[path].texture;
                }
                else
                {
                    var i = new FileResource();
                    i.bitmap = new System.Drawing.Bitmap(path);
                    i.texture = new Texture(i.bitmap);
                    i.count = 1;

                    i.texture.disposeFunctions = () =>
                    {
                        i.count--;
                        if (i.count > 0) return false;
                        return true;
                    };

                    return i.texture;
                }
            }
        }


    }
}
