using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LibNet.Sharp2D
{
    public class RenderColor : RenderOption
    {
        public SColor color;
        public RenderColor(SColor color)
        {
            this.color = color;
        }
        internal override void Use()
        {
            GL.UseProgram(Renderer.shaderColorID);
            GL.ProgramUniform4(Renderer.shaderColorID, 8, color);
        }
    }
}