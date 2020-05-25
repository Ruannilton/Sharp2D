using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LibNet.Sharp2D
{
    public class RenderColor : RenderOption
    {
        public Color4 color;
        public RenderColor(Color4 color)
        {
            this.color = color;
        }
        public override void Use()
        {
            GL.UseProgram(Renderer.shaderColorID);
            GL.ProgramUniform4(Renderer.shaderColorID, 8, color);
        }
    }
}