namespace LibNet.Sharp2D
{
    internal enum ShaderLoc
    {
        ProjectionMatrix = 0,
        vPos = 1,
        vUV = 2,
        vColor = 3,
        vModel = 4,
        iColor = 8
    }
    internal static class ShaderSource
    {
        internal const string COLOR_SHADER_VERTEX =
        @"#version 430 core

        layout(std140, binding = 0) uniform Matrices
        {
            mat4 projection;
        };

        layout (location = 1) in vec3 vPos;
        layout (location = 2) in vec2 vUV; 
        layout (location = 3) in vec4 vColor; 
        layout (location = 4) in mat4 Model; 

        layout (location = 8) uniform vec4 Color;

        out vec4 fColor;

        void main(){
            gl_Position =   projection* Model* vec4(vPos,1.0f);
            fColor = vColor*Color;
        }";
        internal const string COLOR_SHADER_FRAGMENT =
        @"#version 430 core

        in vec4 fColor;

        out vec4 FragColor;

        void main(){
            FragColor = fColor;
        }";

        internal const string TEXTURE_SHADER_VERTEX =
        @"#version 430 core

        layout(std140, binding = 0) uniform Matrices
        {
            mat4 projection;
        };

        layout (location = 1) in vec3 vPos;
        layout (location = 2) in vec2 vUV; 
        layout (location = 3) in vec4 vColor; 
        layout (location = 4) in mat4 Model; 

        layout (location = 8) uniform vec4 Color;

        out vec2 fUV;

        void main(){
            gl_Position =   projection* Model* vec4(vPos,1.0f);
            fUV = vUV;
        }";

        internal const string TEXTURE_SHADER_FRAGMENT =
        @"#version 430 core

        in vec2 fUV;
        uniform sampler2D texture0;


        out vec4 fragColor;

        void main(){
        fragColor = texture(texture0,fUV);
        }";

        internal const string TEXT_SHADER_VERTEX =
                @"#version 430 core

        layout(std140, binding = 0) uniform Matrices
        {
        	mat4 projection;
        };
        
        layout (location = 1) in vec3 vPos;
        layout (location = 2) in vec2 vUV; 
        layout (location = 3) in vec4 vColor; 
        layout (location = 4) in mat4 Model; 
        
        layout (location = 8) uniform vec4 TextColor;
        layout (location = 9) uniform vec4 BackColor;
        
        layout (location = 10) uniform vec2 TextureOffset;
    

        out vec2 fUV;
        out vec4 textColor;
        out vec4 backColor;
        
        void main(){
            gl_Position =   projection* Model* vec4(vPos,1.0f);
            fUV = vUV;
            textColor = TextColor;
            backColor = BackColor;
        }";

        internal const string TEXT_SHADER_FRAGMENT =
        @"#version 430 core

        in vec2 fUV;
        uniform sampler2D texture0;

        out vec4 fragColor;

        in vec4 textColor;
        in vec4 backColor;
        void main(){
        
          vec4 color = texture(texture0,fUV); 
          if(color.r != 0) 
          fragColor = vec4(color.r,color.g,color.b,color.r); 
        }";


    }
}