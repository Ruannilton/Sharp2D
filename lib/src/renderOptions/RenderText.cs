using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using SkiaSharp;
using System;

namespace LibNet.Sharp2D
{
    /// <summary>
    /// RenderComponent that draw a text in a square
    /// </summary>
    public class RenderText : RenderComponent
    {
        private string _Text;
        private float _FontSize;
        private int TextureID;
        private IntPtr BitmapPtr = IntPtr.Zero;
        private int BytesPerRow;

        /// <summary>
        /// Widht of bitmap used to render the text
        /// </summary>
        public int BitmapWidth { get; private set; }
        /// <summary>
        /// Height of bitmap used to render the text
        /// </summary>
        public int BitmapHeight { get; private set; }
        private SKSurface surface;
        private SKCanvas canvas;
        private SKTypeface typeFace;
        private SKColor ForeGround, Background;
        private bool Altered = false;

        /// <summary>
        /// Color of the text
        /// </summary>
        public SColor TextColor
        {
            get
            {
                return TextColor;
            }
            set
            {
                this.ForeGround = ForeGround.WithRed(value.R);
                this.ForeGround = ForeGround.WithGreen(value.G);
                this.ForeGround = ForeGround.WithBlue(value.B);
                this.ForeGround = ForeGround.WithAlpha(value.A);
                Altered = true;
            }
        }

        /// <summary>
        /// Color of the square behind the text 
        /// </summary>
        public SColor BackgroundColor
        {
            get
            {
                return BackgroundColor;
            }
            set
            {
                this.Background = Background.WithRed(value.R);
                this.Background = Background.WithGreen(value.G);
                this.Background = Background.WithBlue(value.B);
                this.Background = Background.WithAlpha(value.A);
                Altered = true;
            }
        }

        /// <summary>
        /// Text to draw
        /// </summary>
        public string Text { get { return _Text; } set { _Text = value; Altered = true; } }

        /// <summary>
        /// Size of the text
        /// </summary>
        public float FontSize { get { return _FontSize; } set { _FontSize = value; Altered = true; } }

        /// <summary>
        /// Create an instance of RenderText
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="fontSize">Size of the text</param>
        /// <param name="face">SKTypeface object that define the font of the text</param>
        public RenderText(string text, uint fontSize = 14, SKTypeface face = null)
        {
            this.Text = text;
            this.FontSize = fontSize;
            this.TextColor = Color.Black;
            this.BackgroundColor = Color.Transparent;
            this.typeFace = face != null ? face : SKTypeface.Default;
            this.GenImage();
        }

        /// <summary>
        /// Create an instance of RenderText
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="fontFamilyName">Name of the font family</param>
        /// <param name="fontSize">Size of the text</param>
        public RenderText(string text, string fontFamilyName, uint fontSize = 14)
        {
            this.Text = text;
            this.FontSize = fontSize;
            this.TextColor = Color.Black;
            this.BackgroundColor = Color.Transparent;
            this.typeFace = SKTypeface.FromFamilyName(fontFamilyName);
            this.GenImage();
        }

        ///<summary>
        /// Destruct this instance and free the memory allocated to store the text data 
        ///<summary>
        ~RenderText()
        {
            Marshal.FreeHGlobal(BitmapPtr);
            BitmapPtr = IntPtr.Zero;
        }

        private void GenImage()
        {
            int max_widht = 0;
            var paint = new SKPaint();
            paint.TextSize = _FontSize;
            paint.IsAntialias = true;
            paint.Color = ForeGround;
            paint.IsStroke = false;
            paint.TextAlign = SKTextAlign.Center;
            paint.Typeface = typeFace;

            bool look_str(string s)
            {
                int l = (int)paint.MeasureText(s);
                if (max_widht < l) max_widht = l;
                return string.IsNullOrEmpty(s);
            }
            string[] textLines = Text.Split('\n').Where(s => !look_str(s)).ToArray();
            Altered = false;
            if (BitmapPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(BitmapPtr);
                BitmapPtr = IntPtr.Zero;
            }



            BitmapWidth = max_widht;
            BitmapHeight = (int)FontSize * textLines.Length;


            this.BitmapPtr = Marshal.AllocHGlobal(BitmapWidth * BitmapHeight * 4);
            this.BytesPerRow = BitmapWidth * 4;

            var surfaceInfo = new SKImageInfo(BitmapWidth, BitmapHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
            this.surface = SKSurface.Create(surfaceInfo, BitmapPtr, BytesPerRow);


            if (surface != null)
            {
                canvas = surface.Canvas;
            }

            canvas.DrawColor(Background);
            for (int i = 0; i < textLines.Length; i++)
            {
                canvas.DrawText(textLines[i], paint.MeasureText(textLines[i]) / 2, _FontSize * (i + 1), paint);
            }
            canvas.Flush();
            TextureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)PixelFormat.Rgba, BitmapWidth, BitmapHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, BitmapPtr);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureParameterName.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureParameterName.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);

        }

        /// <summary>
        /// Get the widht and height of the text
        /// </summary>
        public Vector2 MeasureSize()
        {
            if (Altered) this.GenImage();
            return new Vector2(BitmapWidth, BitmapHeight);
        }
        internal override void Use()
        {
            if (Altered) this.GenImage();
            GL.UseProgram(Renderer.shaderTextureID);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }



    }
}