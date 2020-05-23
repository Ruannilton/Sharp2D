using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using SkiaSharp;
using System;

namespace LibNet.Sharp2D
{
    public class RenderText : RenderOption
    {
        private string _Text;
        private float _FontSize;
        private int TextureID;
        private IntPtr BitmapPtr = IntPtr.Zero;
        private int BytesPerRow;
        public int BitmapWidth { get; private set; }
        public int BitmapHeight { get; private set; }
        private SKSurface surface;
        private SKCanvas canvas;
        private SKTypeface typeFace;
        private SKColor ForeGround, Background;
        private bool Altered = false;
        public Color TextColor
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
        public Color BackgroundColor
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
        public string Text { get { return _Text; } set { _Text = value; Altered = true; } }
        public float FontSize { get { return _FontSize; } set { _FontSize = value; Altered = true; } }

        public RenderText(string text, uint fontSize = 14, SKTypeface face = null)
        {
            this.Text = text;
            this.FontSize = fontSize;
            this.TextColor = Color.Black;
            this.BackgroundColor = Color.Transparent;
            this.typeFace = face != null ? face : SKTypeface.Default;
            this.GenImage();
        }
        private void GenImage()
        {
            Altered = false;
            if (BitmapPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(BitmapPtr);
                BitmapPtr = IntPtr.Zero;
            }

            var paint = new SKPaint();
            paint.TextSize = _FontSize;
            paint.IsAntialias = true;
            paint.Color = ForeGround;
            paint.IsStroke = false;
            paint.TextAlign = SKTextAlign.Center;
            paint.Typeface = typeFace;
            BitmapWidth = (int)paint.MeasureText(Text);
            BitmapHeight = (int)FontSize;


            this.BitmapPtr = Marshal.AllocHGlobal(BitmapWidth * BitmapHeight * 4);
            this.BytesPerRow = BitmapWidth * 4;

            var surfaceInfo = new SKImageInfo(BitmapWidth, BitmapHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
            this.surface = SKSurface.Create(surfaceInfo, BitmapPtr, BytesPerRow);


            if (surface != null)
            {
                canvas = surface.Canvas;
            }

            canvas.DrawColor(Background);
            canvas.DrawText(_Text, BitmapWidth / 2, _FontSize - _FontSize / 8, paint);
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
        ~RenderText()
        {
            Marshal.FreeHGlobal(BitmapPtr);
            BitmapPtr = IntPtr.Zero;
        }

        public Vector2 MeasureSize()
        {
            if (Altered) this.GenImage();
            return new Vector2(BitmapWidth, BitmapHeight);
        }
        public override void Use()
        {
            if (Altered) this.GenImage();
            GL.UseProgram(Renderer.shaderTextureID);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }



    }
}