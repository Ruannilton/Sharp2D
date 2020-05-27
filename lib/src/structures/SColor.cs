using OpenTK;
using OpenTK.Graphics;

namespace LibNet.Sharp2D
{
    public struct SColor
    {
        public byte R, G, B, A;

        public byte this[byte index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.R;
                    case 1:
                        return this.G;
                    case 2:
                        return this.B;
                    case 3:
                        return this.A;
                    default:
                        throw new System.IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.R = value;
                        break;
                    case 1:
                        this.G = value;
                        break;
                    case 2:
                        this.B = value;
                        break;
                    case 3:
                        this.A = value;
                        break;
                    default:
                        throw new System.IndexOutOfRangeException();
                }
            }
        }


        public SColor(byte R, byte G, byte B, byte A)
        {

            this.R = R;
            this.B = B;
            this.G = G;
            this.A = A;
        }
        public SColor(int R, int G, int B, int A)
        {
            this.R = (byte)R;
            this.B = (byte)B;
            this.G = (byte)G;
            this.A = (byte)A;
        }
        public static implicit operator Color(SColor c) => new Color(c.R, c.G, c.B, c.A);
        public static implicit operator SColor(Color c) => new SColor(c.R, c.G, c.B, c.A);
        public static implicit operator SColor(Color4 c) => (Color)c;
        public static implicit operator Color4(SColor c) => (Color)c;

        public static bool operator ==(SColor left, SColor right) => (left.R, left.G, left.B, left.A) == (right.R, right.G, right.B, right.A);
        public static bool operator !=(SColor left, SColor right) => (left.R, left.G, left.B, left.A) != (right.R, right.G, right.B, right.A);
        public static SColor operator *(SColor left, SColor right) => new SColor(left.R * right.R, left.G * right.G, left.B * right.B, left.A * right.A);
        public static SColor operator +(SColor left, SColor right) => new SColor(left.R + right.R, left.G + right.G, left.B + right.B, left.A + right.A);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static SColor MediumAquamarine = Color4.MediumAquamarine;
        public static SColor MediumBlue = Color4.MediumBlue;
        public static SColor MediumOrchid = Color4.MediumOrchid;
        public static SColor MediumPurple = Color4.MediumPurple;
        public static SColor MediumSeaGreen = Color4.MediumSeaGreen;
        public static SColor MediumSlateBlue = Color4.MediumSlateBlue;
        public static SColor MediumSpringGreen = Color4.MediumSpringGreen;
        public static SColor MediumTurquoise = Color4.MediumTurquoise;
        public static SColor MintCream = Color4.MintCream;
        public static SColor MidnightBlue = Color4.MidnightBlue;
        public static SColor Maroon = Color4.Maroon;
        public static SColor MistyRose = Color4.MistyRose;
        public static SColor Moccasin = Color4.Moccasin;
        public static SColor NavajoWhite = Color4.NavajoWhite;
        public static SColor Navy = Color4.Navy;
        public static SColor OldLace = Color4.OldLace;
        public static SColor MediumVioletRed = Color4.MediumVioletRed;
        public static SColor Magenta = Color4.Magenta;
        public static SColor Lime = Color4.Lime;
        public static SColor LimeGreen = Color4.LimeGreen;
        public static SColor LavenderBlush = Color4.LavenderBlush;
        public static SColor LawnGreen = Color4.LawnGreen;
        public static SColor LemonChiffon = Color4.LemonChiffon;
        public static SColor LightBlue = Color4.LightBlue;
        public static SColor LightCoral = Color4.LightCoral;
        public static SColor LightCyan = Color4.LightCyan;
        public static SColor LightGoldenrodYellow = Color4.LightGoldenrodYellow;
        public static SColor LightGreen = Color4.LightGreen;
        public static SColor LightGray = Color4.LightGray;
        public static SColor LightPink = Color4.LightPink;
        public static SColor LightSalmon = Color4.LightSalmon;
        public static SColor LightSeaGreen = Color4.LightSeaGreen;
        public static SColor LightSkyBlue = Color4.LightSkyBlue;
        public static SColor LightSlateGray = Color4.LightSlateGray;
        public static SColor LightSteelBlue = Color4.LightSteelBlue;
        public static SColor LightYellow = Color4.LightYellow;
        public static SColor Olive = Color4.Olive;
        public static SColor Linen = Color4.Linen;
        public static SColor OliveDrab = Color4.OliveDrab;
        public static SColor Orchid = Color4.Orchid;
        public static SColor OrangeRed = Color4.OrangeRed;
        public static SColor Silver = Color4.Silver;
        public static SColor SkyBlue = Color4.SkyBlue;
        public static SColor SlateBlue = Color4.SlateBlue;
        public static SColor SlateGray = Color4.SlateGray;
        public static SColor Snow = Color4.Snow;
        public static SColor SpringGreen = Color4.SpringGreen;
        public static SColor SteelBlue = Color4.SteelBlue;
        public static SColor Sienna = Color4.Sienna;
        public static SColor Tan = Color4.Tan;
        public static SColor Thistle = Color4.Thistle;
        public static SColor Tomato = Color4.Tomato;
        public static SColor Turquoise = Color4.Turquoise;
        public static SColor Violet = Color4.Violet;
        public static SColor Wheat = Color4.Wheat;
        public static SColor White = Color4.White;
        public static SColor WhiteSmoke = Color4.WhiteSmoke;
        public static SColor Teal = Color4.Teal;
        public static SColor SeaShell = Color4.SeaShell;
        public static SColor SeaGreen = Color4.SeaGreen;
        public static SColor SandyBrown = Color4.SandyBrown;
        public static SColor Lavender = Color4.Lavender;
        public static SColor PaleGoldenrod = Color4.PaleGoldenrod;
        public static SColor PaleGreen = Color4.PaleGreen;
        public static SColor PaleTurquoise = Color4.PaleTurquoise;
        public static SColor PaleVioletRed = Color4.PaleVioletRed;
        public static SColor PapayaWhip = Color4.PapayaWhip;
        public static SColor PeachPuff = Color4.PeachPuff;
        public static SColor Peru = Color4.Peru;
        public static SColor Pink = Color4.Pink;
        public static SColor Plum = Color4.Plum;
        public static SColor PowderBlue = Color4.PowderBlue;
        public static SColor Purple = Color4.Purple;
        public static SColor Red = Color4.Red;
        public static SColor RosyBrown = Color4.RosyBrown;
        public static SColor RoyalBlue = Color4.RoyalBlue;
        public static SColor SaddleBrown = Color4.SaddleBrown;
        public static SColor Salmon = Color4.Salmon;
        public static SColor Orange = Color4.Orange;
        public static SColor Khaki = Color4.Khaki;
        public static SColor IndianRed = Color4.IndianRed;
        public static SColor Indigo = Color4.Indigo;
        public static SColor DarkGray = Color4.DarkGray;
        public static SColor DarkGoldenrod = Color4.DarkGoldenrod;
        public static SColor DarkCyan = Color4.DarkCyan;
        public static SColor DarkBlue = Color4.DarkBlue;
        public static SColor Cyan = Color4.Cyan;
        public static SColor Crimson = Color4.Crimson;
        public static SColor Cornsilk = Color4.Cornsilk;
        public static SColor CornflowerBlue = Color4.CornflowerBlue;
        public static SColor Ivory = Color4.Ivory;
        public static SColor Chocolate = Color4.Chocolate;
        public static SColor Chartreuse = Color4.Chartreuse;
        public static SColor CadetBlue = Color4.CadetBlue;
        public static SColor DarkGreen = Color4.DarkGreen;
        public static SColor BurlyWood = Color4.BurlyWood;
        public static SColor BlueViolet = Color4.BlueViolet;
        public static SColor Blue = Color4.Blue;
        public static SColor BlanchedAlmond = Color4.BlanchedAlmond;
        public static SColor Black = Color4.Black;
        public static SColor Bisque = Color4.Bisque;
        public static SColor Beige = Color4.Beige;
        public static SColor Azure = Color4.Azure;
        public static SColor Aquamarine = Color4.Aquamarine;
        public static SColor Aqua = Color4.Aqua;
        public static SColor AntiqueWhite = Color4.AntiqueWhite;
        public static SColor AliceBlue = Color4.AliceBlue;
        public static SColor Transparent = Color4.Transparent;
        public static SColor Brown = Color4.Brown;
        public static SColor DarkKhaki = Color4.DarkKhaki;
        public static SColor Coral = Color4.Coral;
        public static SColor DarkOliveGreen = Color4.DarkOliveGreen;
        public static SColor Yellow = Color4.Yellow;
        public static SColor HotPink = Color4.HotPink;
        public static SColor DarkMagenta = Color4.DarkMagenta;
        public static SColor GreenYellow = Color4.GreenYellow;
        public static SColor Green = Color4.Green;
        public static SColor Gray = Color4.Gray;
        public static SColor Goldenrod = Color4.Goldenrod;
        public static SColor Gold = Color4.Gold;
        public static SColor GhostWhite = Color4.GhostWhite;
        public static SColor Gainsboro = Color4.Gainsboro;
        public static SColor Fuchsia = Color4.Fuchsia;
        public static SColor ForestGreen = Color4.ForestGreen;
        public static SColor FloralWhite = Color4.FloralWhite;
        public static SColor Honeydew = Color4.Honeydew;
        public static SColor DodgerBlue = Color4.DodgerBlue;
        public static SColor Firebrick = Color4.Firebrick;
        public static SColor DarkOrange = Color4.DarkOrange;
        public static SColor DarkOrchid = Color4.DarkOrchid;
        public static SColor DarkRed = Color4.DarkRed;
        public static SColor DarkSalmon = Color4.DarkSalmon;
        public static SColor DarkSeaGreen = Color4.DarkSeaGreen;
        public static SColor YellowGreen = Color4.YellowGreen;
        public static SColor DarkSlateGray = Color4.DarkSlateGray;
        public static SColor DarkTurquoise = Color4.DarkTurquoise;
        public static SColor DarkViolet = Color4.DarkViolet;
        public static SColor DeepPink = Color4.DeepPink;
        public static SColor DeepSkyBlue = Color4.DeepSkyBlue;
        public static SColor DarkSlateBlue = Color4.DarkSlateBlue;
        public static SColor DimGray = Color4.DimGray;
    }
}