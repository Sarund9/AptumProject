using System;
using System.Runtime.CompilerServices;
using Veldrid;

namespace AptumEngine.Core
{
    public struct Vector2
    {
        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(float f)
        {
            x = f;
            y = f;
        }

        #region STATIC_EXPRESSIONS
        public static Vector2 Zero => new Vector2(0);
        public static Vector2 One => new Vector2(1);
        public static Vector2 Up => new Vector2(0, 1);
        public static Vector2 Down => new Vector2(0, -1);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Left => new Vector2(-1, 0);
        #endregion

        public float Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MathF.Sqrt(x * x + y * y); 
        }

        public Vector2 Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this / Magnitude;
        }

        public static Vector2 Angle(Vector2 a, Vector2 b)
        {
            throw new NotImplementedException("Vector2 Angle not Done"); //TODO
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector2 a, Vector2 b) =>
            (a.x * b.x) + (a.y * b.y);

        public override string ToString()
        {
            return $"[{x} - {y}]";
        }

        public static VertexLayoutDescription GetVertexLayout() => new VertexLayoutDescription(
            new VertexElementDescription("Position", VertexElementSemantic.Position, VertexElementFormat.Float2)
            );
        
        public static implicit operator Vector2(System.Numerics.Vector2 v) =>
            new Vector2
            {
                x = v.X,
                y = v.Y,
            };
        public static implicit operator System.Numerics.Vector2(Vector2 v) =>
            new System.Numerics.Vector2
            {
                X = v.x,
                Y = v.y,
            };

        #region MATH_OPERATORS
        public static Vector2 operator +(Vector2 a, Vector2 b) =>
            new Vector2
            {
                x = a.x + b.x,
                y = a.y + b.y,
            };
        public static Vector2 operator -(Vector2 a, Vector2 b) =>
           new Vector2
           {
               x = a.x - b.x,
               y = a.y - b.y,
           };
        public static Vector2 operator -(Vector2 a) =>
           new Vector2
           {
               x = -a.x,
               y = -a.y,
           };

        public static Vector2 operator *(Vector2 a, float f) =>
            new Vector2
            {
                x = a.x * f,
                y = a.y * f,
            };
        public static Vector2 operator /(Vector2 a, float f) =>
            new Vector2
            {
                x = a.x / f,
                y = a.y / f,
            };
        public static Vector2 operator %(Vector2 a, float f) =>
            new Vector2
            {
                x = a.x % f,
                y = a.y % f,
            };
        #endregion
    }

    
}
