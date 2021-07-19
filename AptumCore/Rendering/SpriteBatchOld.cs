using System.Collections.Generic;
using System.Text;
using Veldrid;
using Veldrid.SPIRV;
using AptumEngine.Core;

namespace AptumEngine
{
    [System.Obsolete]
    public class SpriteBatchOld
    {
        private const string VertexCode = @"
#version 450
layout(location = 0) in vec2 Position;
layout(location = 1) in vec2 TexCoords;
layout(location = 0) out vec2 fsin_texCoords;
void main()
{
    gl_Position = vec4(Position, 0, 1);;
    fsin_texCoords = TexCoords;
}";
        private const string FragmentCode = @"
#version 450
layout(location = 0) in vec2 fsin_texCoords;
layout(location = 0) out vec4 fsout_color;
layout(set = 1, binding = 1) uniform texture2D SurfaceTexture;
layout(set = 1, binding = 2) uniform sampler SurfaceSampler;
void main()
{
    fsout_color =  texture(sampler2D(SurfaceTexture, SurfaceSampler), fsin_texCoords);
}";


        struct DrawCall
        {
            public Rect pos;
            public Texture texture;
        }
        
        AWindow m_Widnow;
        List<DrawCall> m_DrawCalls = new List<DrawCall>();
        bool b_Begin;

        DeviceBuffer m_VertexUVBuffer;
        DeviceBuffer m_IndexBuffer;

        ResourceFactory m_Factory;
        Pipeline m_Pipeline;

        CommandList m_CL;

        public SpriteBatchOld(AWindow widnow)
        {
            m_Widnow = widnow;
            m_Factory = widnow.GraphicsDevice.ResourceFactory;

            BufferDescription vbDescription = new BufferDescription(
                VertexUV.SIZE_IN_BYTES * 4,
                BufferUsage.VertexBuffer | BufferUsage.Dynamic);

            m_VertexUVBuffer = m_Factory.CreateBuffer(vbDescription);

            ushort[] quadIndices = { 0, 1, 2, 3 };
            BufferDescription ibDescription = new BufferDescription(
                4 * sizeof(ushort),
                BufferUsage.IndexBuffer | BufferUsage.Dynamic);

            m_IndexBuffer = m_Factory.CreateBuffer(ibDescription);

            VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                new VertexElementDescription("TexCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2)
                );

            //Create Shader Descriptions from Shader Code ####
            ShaderDescription vertexShaderDesc = new ShaderDescription(
                ShaderStages.Vertex,
                Encoding.UTF8.GetBytes(VertexCode),
                "main");
            ShaderDescription fragmentShaderDesc = new ShaderDescription(
                ShaderStages.Fragment,
                Encoding.UTF8.GetBytes(FragmentCode),
                "main");
            //Generate Shaders
            Shader[] shaders = m_Factory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);

            //PIPELINE
            m_Pipeline = m_Factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.Default,
                PrimitiveTopology.TriangleList,
                new ShaderSetDescription(
                    new VertexLayoutDescription[] { vertexLayout },
                    shaders
                        ),
                new ResourceLayout[] { },
                new OutputDescription())
                    );

            m_CL = m_Factory.CreateCommandList();
        }

        public void W_Get()
        {

        }

        public void Begin()
        {
            if (b_Begin)
            {
                return;
            }
            b_Begin = true;
        }

        public void DrawTexture(Rect pos, Texture texture)
        {
            if (!b_Begin)
            {
                return;
            }

            //Prepare data to be Uploaded to the GPU
            m_DrawCalls.Add(new DrawCall { pos = pos, texture = texture });
        }

        public void SubmitEnd()
        {
            if (!b_Begin)
            {
                return;
            }
            b_Begin = false;

            List<VertexUV> Quads = new List<VertexUV>(m_DrawCalls.Count * 4);
            //Send data to GPU (Update Buffers)
            foreach (var drawCall in m_DrawCalls)
            {
                //Create Quad
                Quads.Add(new VertexUV(drawCall.pos.Position, new Vector2(0, 1)));                                       //Up Left
                Quads.Add(new VertexUV(drawCall.pos.Position + Vector2.Up * drawCall.pos.Size.y, new Vector2(0, 0)));    //Bot Left
                Quads.Add(new VertexUV(drawCall.pos.Position + drawCall.pos.Size, new Vector2(1, 0)));                   //Bot Right
                Quads.Add(new VertexUV(drawCall.pos.Position + Vector2.Right * drawCall.pos.Size.x, new Vector2(1, 1))); //Up Right

                //var quad = new VertexUV[]
                //{
                //    new VertexUV(drawCall.pos.Position, new Vector2(0, 1)),                        //Up Left
                //    new VertexUV(drawCall.pos.Position + Vector2.Up * drawCall.pos.Size.y, new Vector2(0, 0)),    //Bot Left
                //    new VertexUV(drawCall.pos.Position + drawCall.pos.Size, new Vector2(1, 0)),    //Bot Right
                //    new VertexUV(drawCall.pos.Position + Vector2.Right * drawCall.pos.Size.x, new Vector2(1, 1)),    //Up Right
                //};

            }

            BufferDescription vbDescription = new BufferDescription(
                (uint)Quads.Count * VertexUV.SIZE_IN_BYTES,
                BufferUsage.VertexBuffer | BufferUsage.Dynamic);

            m_Widnow.GraphicsDevice.UpdateBuffer(m_VertexUVBuffer, 0, VertexUV.SIZE_IN_BYTES * 4);
            m_Widnow.GraphicsDevice.UpdateBuffer(m_IndexBuffer, 0, sizeof(ushort) * 4);



            //m_VertexUVBuffer
        }
    }
    public struct VertexUV
    {
        public const uint SIZE_IN_BYTES = sizeof(float) * 4;
        public Vector2 pos;
        public Vector2 uv;

        public VertexUV(Vector2 pos, Vector2 uv)
        {
            this.pos = pos;
            this.uv = uv;
        }
    }
}
