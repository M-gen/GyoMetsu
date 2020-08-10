using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Emugen.OpenTK.Sprite
{

    public class LayerSprite : ISprite, IDisposable
    {
        public Action<LayerSprite> DrawStart;
        public Action<LayerSprite> DrawEnd;

        class Item
        {
            public ISprite sprite = null;
            public int drawPriority;
        }
        List<Item> sprites = new List<Item>();

        public override void Draw()
        {
            if (DrawStart != null) DrawStart(this);

            var tmp = new List<Item>();
            try
            {
                foreach (var i in sprites)
                {
                    if ( (i.sprite != null) && (i.sprite.IsDraw) )
                    {
                        GL.Clear(ClearBufferMask.DepthBufferBit);
                        try
                        {
                            i.sprite.Draw();
                            tmp.Add(i);
                        }
                        catch
                        {
                        }
                    }
                    else if ((i.sprite != null) && (!i.sprite.IsDraw))
                    {
                        tmp.Add(i);
                    }
                }
                sprites = tmp;
            }
            catch
            {
                // todo : sprites に追加中にDrawが呼び出されてエラーが出力される、ロックがかかってない
            }

            if (DrawEnd != null) DrawEnd(this);
        }

        public void Add(ISprite sprite, int drawPriority)
        {
            var item = new Item();
            item.sprite = sprite;
            item.drawPriority = drawPriority;
            _AddCore(item);
        }

        private void _AddCore(Item item)
        {
            var pos = -1;
            var index = 0;
            foreach (var i in sprites)
            {
                if (item.drawPriority < i.drawPriority)
                {
                    pos = index;
                    break;
                }
                index++;
            }

            if (pos == -1)
            {
                sprites.Add(item);
            }
            else
            {
                sprites.Insert(index, item);
            }
        }

        public void Del(ISprite sprite)
        {
            var pos = default(Item);
            foreach (var i in sprites)
            {
                if (i.sprite == sprite)
                {
                    pos = i;
                    break;
                }
            }
            if (pos != null)
            {
                sprites.Remove(pos);
            }
        }

        public void Dispose()
        {
            foreach( var i in sprites)
            {
                var d = i.sprite as IDisposable;
                d?.Dispose();
            }
        }
    }
}
