namespace LibNet.Sharp2D
{
    /// <summary>
    /// Class that tells to Renderer.Draw how to render something
    /// </summary>
    public abstract class RenderComponent
    {
        /// <summary>
        /// ID of the shader used by this component
        /// </summary>
        protected int shaderID;
        internal abstract void Use();
    }
}