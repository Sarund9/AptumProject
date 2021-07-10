using System;

namespace AptumEngine.Core
{
    
    public struct Rect
    {
        public float x, y, w, h;

        public Rect(Vector2 position, Vector2 size) : this()
        {
            Position = position;
            Size = size;
        }
        public Rect(float x, float y, float w, float h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public Vector2 Position
        {
            get => new Vector2(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }
        public Vector2 Size
        {
            get => new Vector2(w, h);
            set
            {
                w = value.x;
                h = value.y;
            }
        }

        public Vector2 TL => new Vector2(x, y);
        public Vector2 TR => new Vector2(x + w, y);
        public Vector2 BL => new Vector2(x, y + h);
        public Vector2 BR => new Vector2(x + w, y + h);

        //public bool IsInside(Vector2 point)
        //{

        //}
    }

    //TODO: IRect class (Rect with Int-s)
    //TODO: Area class (Rect with Rang01-s)
}
