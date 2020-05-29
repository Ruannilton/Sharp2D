using OpenTK.Graphics.OpenGL;

namespace LibNet.Sharp2D
{
    /// <summary>
    /// RenderComponent that draw an image in a square
    /// </summary>
    public class RenderImage : RenderComponent
    {
        /// <summary>
        /// ID of the texture
        /// </summary>
        public int textureID;

        /// <summary>
        /// If enabled this component will use the alpha channel of the image to render transparent images
        /// </summary>
        public bool enableAlpha;

        /// <summary>
        /// Create an instance of RenderImage
        /// </summary>
        /// <param name="texture">ID of the texture</param>
        /// <param name="enableAlpha">If enabled this component will use the alpha channel of the image to render transparent images</param>
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