using System;
using System.Collections.Generic;
using System.Text;

namespace Emugen.Image.Primitive
{
    public class Rect
    {
        public Vector2D Position = new Vector2D();
        public Vector2D Size = new Vector2D();

        public Rect()
        {
            this.Position = new Vector2D();
            this.Size = new Vector2D();
        }
        public Rect( Rect rect )
        {
            this.Position = new Vector2D( rect.Position.X, rect.Position.Y);
            this.Size = new Vector2D(rect.Size.X, rect.Size.Y);
        }

        public Rect(Vector2D posision, Vector2D size )
        {
            this.Position = posision;
            this.Size = size;
        }

        public bool IsHit( Vector2D i )
        {

            if (((Position.X <= i.X) && (i.X < (Position.X + Size.X))) &&
                 ((Position.Y <= i.Y) && (i.Y < (Position.Y + Size.Y))))
            {
                return true;
            }
            return false;
        }
        public bool IsHit(Vector2I i)
        {

            if (((Position.X <= i.X) && (i.X < (Position.X + Size.X))) &&
                 ((Position.Y <= i.Y) && (i.Y < (Position.Y + Size.Y))))
            {
                return true;
            }
            return false;
        }
    }
}
