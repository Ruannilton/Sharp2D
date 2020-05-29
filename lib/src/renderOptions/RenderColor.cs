using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LibNet.Sharp2D
{
    /// <summary>
    /// RenderComponent that draw a colored square
    /// </summary>
    public class RenderColor : RenderComponent
    {
        /// <summary>
        /// Color that will be drawn
        /// </summary>
        public SColor color;
        /// <summary>
        /// Create an instance of RenderColor
        /// </summary>
        /// <param name="color">Color that will be drawn</param>
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