using System;
using LibNet.Sharp2D;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace example
{

    public class SharpWindow : GameWindow
    {
        Renderer render;
        RenderText renderText;
        RenderColor renderColor;


        public SharpWindow() : base(800, 640, GraphicsMode.Default, "SharpWindow")
        {
            render = new Renderer(800, 640);
            GL.Viewport(0, 0, 800, 640);
        }
        public SharpWindow(int widht, int height, string title) : base(widht, height, GraphicsMode.Default, title)
        {
            render = new Renderer(widht, height);
            GL.Viewport(0, 0, Width, Height);

        }

        protected override void OnLoad(EventArgs e)
        {
            renderText = new RenderText("Hello World", 32);
            renderColor = new RenderColor(Color4.DarkOrange);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.CornflowerBlue);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            render.Draw(renderText, new RectPosition(new Vector3(50, 50, 0), renderText.MeasureSize()));
            render.Draw(renderColor, new RectPosition(new Vector3(200, 200, 0), new Vector2(100, 100)));

            Context.SwapBuffers();

        }

        protected override void OnResize(System.EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            render.Resize(Width, Height);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            SharpWindow window = new SharpWindow();
            window.Run();
        }
    }
}
