using System;
using System.Collections.Generic;
using System.Text;
using Veldrid;

namespace AptumEngine.Core
{
    public struct Color
    {
        public float r, g, b, a;

        public Color(float r, float g, float b, float a = 1)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        #region STD_COLORS
        
        /// <summary> Shorthand for White Color (1, 1, 1) </summary>
        public static Color White => new Color(1,1,1);
        /// <summary> Shorthand for Black Color (0, 0, 0) </summary>
        public static Color Black => new Color(0, 0, 0);
        /// <summary> Shorthand for Fully transparent Color (0,0,0,0) </summary>
        public static Color Clear => new Color(0, 0, 0, 0);

        /// <summary> Shorthand for new Color(0.5f, 0.5f, 0.5f) </summary>
        public static Color Gray => new Color(0.5f, 0.5f, 0.5f);
        /// <summary> Shorthand for new Color(0.5f, 0.5f, 0.5f) </summary>
        public static Color Grey => new Color(0.5f, 0.5f, 0.5f);

        //Pures
        /// <summary> Shorthand for new Color(1, 0, 0) </summary>
        public static Color Red => new Color(1, 0, 0);
        public static Color Green => new Color(0, 1, 0);
        public static Color Blue => new Color(0, 0, 1);
        public static Color Yellow => new Color(1, 1, 0);
        public static Color Cyan => new Color(0, 1, 1);
        public static Color Magenta => new Color(1, 0, 1);

        //Dark
        public static Color DarkRed => new Color(0.5f, 0, 0);
        public static Color DarkGreen => new Color(0, 0.5f, 0);
        public static Color DarkBlue => new Color(0, 0, 0.5f);
        public static Color DarkYellow => new Color(0.5f, 0.5f, 0);
        public static Color DarkCyan => new Color(0, 0.5f, 0.5f);
        public static Color DarkMagenta => new Color(0.5f, 0, 0.5f);

        //Light
        public static Color LightRed => new Color(1, 0.5f, 0.5f);
        public static Color LightGreen => new Color(0.5f, 1, 0.5f);
        public static Color LightBlue => new Color(0.5f, 0.5f, 1);
        public static Color LightYellow => new Color(1, 1, 0.5f);
        public static Color LightCyan => new Color(0.5f, 1, 1);
        public static Color LightMagenta => new Color(1, 0.5f, 1);

        //Special
        public static Color Orange => new Color(1, 0.5f, 0);
        public static Color Fuchia => new Color(1, 0, 0.5f);
        public static Color AquaGreen => new Color(0, 1, 0.5f);
        public static Color MidBlue => new Color(0, 0.5f, 1);
        public static Color LimeGreen => new Color(0.5f, 1, 0);
        public static Color Purple => new Color(0.5f, 0, 1);

        #endregion
        public static Color FromHSV(float hue, float sat, float val, float alp = 1)
        {
            const float SIXTY_DEGREES = 0.1666666f; //60/360 = 0.1666666f

            //var max = val;
            var chroma = sat * val;
            //var min = max - chroma;

            float X = chroma * (1 -
                MathF.Abs(
                    (hue / SIXTY_DEGREES % 2) - 1
                        ));
            float m = val - chroma;

            if (hue < SIXTY_DEGREES) //60D
                return new Color(chroma, X, 0, alp);
            else
                if (hue < SIXTY_DEGREES * 2) //120D
                return new Color(X, chroma, 0, alp);
            else
                if (hue < SIXTY_DEGREES * 3) //180
                return new Color(0, chroma, X, alp);
            else
                if (hue < SIXTY_DEGREES * 4) //240
                return new Color(0, X, chroma, alp);
            else
                if (hue < SIXTY_DEGREES * 5) //360D
                return new Color(X, 0, chroma, alp);
            else
                return new Color(chroma, 0, X, alp);
            
        }

        //HSVA RrgToHsv(Color c)
        //{
        //    float maxC = MathF.Max(c.r, MathF.Max(c.g, c.b));
        //    float minC = MathF.Min(c.r, MathF.Min(c.g, c.b));

        //    var value = maxC;
        //    if (minC == maxC)
        //        return HSVA.LightnessAt(value);

        //    var saturation = (maxC / minC) / maxC;
        //    var rc = (maxC - c.r) / (maxC - minC);
        //    var gc = (maxC - c.g) / (maxC - minC);
        //    var bc = (maxC - c.b) / (maxC - minC);

        //    float hue;
        //    if (c.r == maxC)
        //    {
        //        hue = 0.0f + bc - gc;
        //    }
        //    else if (c.g == maxC)
        //    {
        //        hue = 2.0f + rc - bc;
        //    }
        //    else
        //    {
        //        hue = 4.0f + gc - rc;
        //    }
        //    hue = (hue / 6.0f) % 1.0f;

        //    return new HSVA(hue, saturation, value);
        //}

        public static implicit operator RgbaFloat(Color c) =>
            new RgbaFloat(c.r, c.g, c.b, c.a);
    }

    public struct Color32
    {
        public byte r, g, b, a;
    }

    public struct Color64
    {
        public ushort r, g, b, a;
    }

    public struct HSVA
    {
        public float h, s, v, a;

        public HSVA(float h, float s, float v) : this()
        {
            this.h = h;
            this.s = s;
            this.v = v;
            this.a = 1;
        }

        public HSVA(float h, float s, float v, float a)
        {
            this.h = h;
            this.s = s;
            this.v = v;
            this.a = a;
        }

        /// <summary> Generates a grayscale color using Lightness at V </summary>
        public static HSVA LightnessAt(float v) => new HSVA(v, v, v);

        /// <summary> Convert to RGB color </summary>
        public Color ToColor() =>
            Color.FromHSV(h, s, v, a);
    }
}
