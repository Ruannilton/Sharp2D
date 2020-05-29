using System;
using OpenTK;
using System.IO;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;


namespace LibNet.Sharp2D
{
    /// <summary>
    /// Class tha allow load image and draw RenderComponents
    /// </summary>
    public class Renderer
    {
        private const int MAT4_SIZE = 16 * sizeof(float);
        private readonly int SHADER_VERTEX_MODEL_BUFFER;
        private readonly int PROJ_MATRIX_UNIFORM_LOCATION;
        private Matrix4 ProjectionMatrix;
        private Vector2 ViewPort;
        private List<int> imagesLoaded = new List<int>();

        internal static int rectID;
        internal static int shaderColorID;
        internal static int shaderTextureID;
        internal static int shaderTextID;

        /// <summary>
        /// Create an instance of Renderer
        /// Needed buffers, shaders and vaos are loaded
        /// </summary>
        /// <param name="width">widht of the view port</param>
        /// <param name="height">height of the view port</param>
        public Renderer(int width, int height)
        {
            ViewPort = new Vector2(width, height);
            SHADER_VERTEX_MODEL_BUFFER = GL.GenBuffer();
            PROJ_MATRIX_UNIFORM_LOCATION = GL.GenBuffer();

            LoadBuffers();
            LoadShaders();
            LoadVAOs();
        }

        /// <summary>
        /// Desconstruct this instance and delete the shaders and images loaded in the gpu memory
        /// </summary>
        ~Renderer()
        {
            GL.DeleteTextures(imagesLoaded.Count, imagesLoaded.ToArray());
            GL.DeleteProgram(shaderTextureID);
            GL.DeleteProgram(shaderColorID);
        }

        /// <summary>
        /// Set the size of the view port 
        /// </summary>
        /// <param name="width"> view port width </param>
        /// <param name="height">view port height</param>
        public void Resize(in int width, in int height)
        {
            ViewPort.X = width;
            ViewPort.Y = height;
            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, ViewPort.X, ViewPort.Y, 0, 100.0f, -100.0f);
            UpdateProjMatrixOnGPU();
        }

        /// <summary>
        /// Draw a RenderComponent onto window
        /// </summary>
        /// <param name="renderData">Component to draw</param>
        /// <param name="position">Position of component</param>
        /// <param name="size">Size of component</param>
        public void Draw(in RenderComponent renderData, Vector3 position, Vector2 size)
        {
            Matrix4[] matrices = RectToMat(new RectPosition(position, size));
            renderData.Use();
            SetTransforms(matrices);
            GL.BindVertexArray(rectID);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (IntPtr)0, matrices.Length);
        }

        /// <summary>
        /// Draw a RenderComponent onto window
        /// </summary>
        /// <param name="renderData">Component to draw</param>
        /// <param name="position">Position of component</param>
        /// <param name="size">Size of component</param>
        public void Draw(in RenderComponent renderData, Vector2 position, Vector2 size)
        {
            Matrix4[] matrices = RectToMat(new RectPosition(position, size));
            renderData.Use();
            SetTransforms(matrices);
            GL.BindVertexArray(rectID);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (IntPtr)0, matrices.Length);
        }

        /// <summary>
        /// Draw a RenderComponent onto window
        /// </summary>
        /// <param name="renderData">Component to Draw</param>
        /// <param name="transforms">Array of RectPositions</param>        
        public void Draw(in RenderComponent renderData, params RectPosition[] transforms)
        {
            Matrix4[] matrices = RectToMat(transforms);
            renderData.Use();
            SetTransforms(matrices);
            GL.BindVertexArray(rectID);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (IntPtr)0, matrices.Length);
        }

        /// <summary>
        /// Draw a RenderComponent onto window
        /// </summary>
        /// <param name="renderData">Component to Draw</param>
        /// <param name="transforms">Array of Matrix4</param> 
        public void Draw(in RenderComponent renderData, params Matrix4[] transforms)
        {
            renderData.Use();
            SetTransforms(transforms);
            GL.BindVertexArray(rectID);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (IntPtr)0, transforms.Length);
        }

        /// <summary>
        /// Load an image in the gpu memory
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <returns>image id</returns>
        public static int LoadImage(string path)
        {

            using (var fs = new FileStream(path, FileMode.Open))
            {
                SKBitmap bitmap = SKBitmap.Decode(fs);
                Console.WriteLine(bitmap.ColorType);

                int textureID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureID);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)PixelFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, bitmap.Bytes);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                return textureID;
            }
        }

        private void SetTransforms(params Matrix4[] transforms)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, SHADER_VERTEX_MODEL_BUFFER);
            GL.BufferData(BufferTarget.ArrayBuffer, transforms.Length * MAT4_SIZE, transforms, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(4);
            GL.EnableVertexAttribArray(5);
            GL.EnableVertexAttribArray(6);
            GL.EnableVertexAttribArray(7);

            GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, MAT4_SIZE, (IntPtr)0);
            GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, MAT4_SIZE, (IntPtr)(MAT4_SIZE / 4));
            GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, MAT4_SIZE, (IntPtr)(2 * MAT4_SIZE / 4));
            GL.VertexAttribPointer(7, 4, VertexAttribPointerType.Float, false, MAT4_SIZE, (IntPtr)(3 * MAT4_SIZE / 4));

            GL.VertexAttribDivisor(4, 1);
            GL.VertexAttribDivisor(5, 1);
            GL.VertexAttribDivisor(6, 1);
            GL.VertexAttribDivisor(7, 1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        private void LoadBuffers()
        {
            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, ViewPort.X, ViewPort.Y, 0, 100.0f, -100.0f);
            AllocateProjMatrixOnGPU();
            UpdateProjMatrixOnGPU();
        }
        private void LoadShaders()
        {

            {//LOAD COLOR SHADER
                int vertexProg = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexProg, ShaderSource.COLOR_SHADER_VERTEX);
                GL.CompileShader(vertexProg);

                int fragmentProg = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentProg, ShaderSource.COLOR_SHADER_FRAGMENT);
                GL.CompileShader(fragmentProg);

                int program = GL.CreateProgram();
                GL.AttachShader(program, vertexProg);
                GL.AttachShader(program, fragmentProg);

                GL.LinkProgram(program);

                GL.DetachShader(program, vertexProg);
                GL.DetachShader(program, fragmentProg);
                GL.DeleteShader(vertexProg);
                GL.DeleteShader(fragmentProg);

                shaderColorID = program;
            }
            {//LOAD TEXTURE SHADER
                int vertexProg = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexProg, ShaderSource.TEXTURE_SHADER_VERTEX);
                GL.CompileShader(vertexProg);

                int fragmentProg = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentProg, ShaderSource.TEXTURE_SHADER_FRAGMENT);
                GL.CompileShader(fragmentProg);

                int program = GL.CreateProgram();
                GL.AttachShader(program, vertexProg);
                GL.AttachShader(program, fragmentProg);

                GL.LinkProgram(program);

                GL.DetachShader(program, vertexProg);
                GL.DetachShader(program, fragmentProg);
                GL.DeleteShader(vertexProg);
                GL.DeleteShader(fragmentProg);

                shaderTextureID = program;
            }

            {//LOAD TEXT SHADER
                int vertexProg = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexProg, ShaderSource.TEXT_SHADER_VERTEX);
                GL.CompileShader(vertexProg);

                int fragmentProg = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentProg, ShaderSource.TEXT_SHADER_FRAGMENT);
                GL.CompileShader(fragmentProg);

                int program = GL.CreateProgram();
                GL.AttachShader(program, vertexProg);
                GL.AttachShader(program, fragmentProg);

                GL.LinkProgram(program);

                GL.DetachShader(program, vertexProg);
                GL.DetachShader(program, fragmentProg);
                GL.DeleteShader(vertexProg);
                GL.DeleteShader(fragmentProg);

                shaderTextID = program;
            }
        }
        private void LoadVAOs()
        {
            float[] positions = new float[] { 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0 };
            float[] uvs = new float[] { 0, 0, 1, 0, 1, 1, 0, 1 };
            float[] colors = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] index = new int[] { 0, 1, 3, 1, 2, 3 };

            int PositionSize = positions.Length * 3 * sizeof(float);
            int UvSize = uvs.Length * 2 * sizeof(float);
            int ColorSize = colors.Length * 4 * sizeof(float);
            int IndexSize = index.Length * sizeof(int);
            int Size = PositionSize + UvSize + ColorSize;

            int id = GL.GenVertexArray();
            GL.BindVertexArray(id);

            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Size, (IntPtr)0, BufferUsageHint.StaticDraw);
            int posSize = PositionSize, uvSize = UvSize, colorSize = ColorSize;

            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, posSize, positions);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)posSize, uvSize, uvs);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(posSize + uvSize), colorSize, colors);

            GL.VertexAttribPointer((int)ShaderLoc.vPos, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.VertexAttribPointer((int)ShaderLoc.vUV, 2, VertexAttribPointerType.Float, true, 2 * sizeof(float), posSize);
            GL.VertexAttribPointer((int)ShaderLoc.vColor, 4, VertexAttribPointerType.Float, true, 4 * sizeof(float), posSize + uvSize);

            GL.EnableVertexAttribArray((int)ShaderLoc.vPos);
            GL.EnableVertexAttribArray((int)ShaderLoc.vUV);
            GL.EnableVertexAttribArray((int)ShaderLoc.vColor);

            int index_id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, index_id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, IndexSize, index, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);
            rectID = id;
        }
        private void AllocateProjMatrixOnGPU()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, PROJ_MATRIX_UNIFORM_LOCATION);
            GL.BufferData(BufferTarget.UniformBuffer, MAT4_SIZE, ref ProjectionMatrix, BufferUsageHint.DynamicDraw);
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, (int)ShaderLoc.ProjectionMatrix, PROJ_MATRIX_UNIFORM_LOCATION, (IntPtr)0, MAT4_SIZE);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }
        private void UpdateProjMatrixOnGPU()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, PROJ_MATRIX_UNIFORM_LOCATION);
            GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, MAT4_SIZE, ref ProjectionMatrix);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        private Matrix4[] RectToMat(params RectPosition[] transforms)
        {
            Matrix4[] matrices = new Matrix4[transforms.Length];
            for (int i = 0; i < transforms.Length; i++)
            {
                RectPosition t = transforms[i];
                matrices[i] = Matrix4.CreateScale(t.size.X, t.size.Y, 1) * Matrix4.CreateTranslation(t.position.X, t.position.Y, t.position.Z);
            }
            return matrices;
        }
    }
}