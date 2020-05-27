using OpenTK.Graphics.OpenGL;

namespace LibNet.Sharp2D
{
    public class RenderImage : RenderOption
    {
        public int textureID;
        public bool enableAlpha;
        public RenderImage(int texture, bool enableAlpha = false)
        {
            textureID = texture;
            this.enableAlpha = enableAlpha;
        }
        internal override void Use()
        {
            GL.UseProgram(Renderer.shaderTextureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            if (enableAlpha)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            }
        }
    }
}