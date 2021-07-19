using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.SPIRV;
using Veldrid.StartupUtilities;
using System.Runtime.InteropServices;

namespace AptumEngine.Core
{
    public class SpriteBatcher : AObject
    {
        private const string VertexCode = @"
#version 450
layout(location = 0) in vec2 Position;
layout(location = 1) in vec4 Color;
layout(location = 0) out vec4 fsin_Color;
void main()
{
    gl_Position = vec4(Position, 0, 1);
    fsin_Color = Color;
}";

        private const string FragmentCode = @"
#version 450
layout(location = 0) in vec4 fsin_Color;
layout(location = 0) out vec4 fsout_Color;
void main()
{
    fsout_Color = fsin_Color;
}";


        public GraphicsDevice m_GraphicsDevice;
        public ResourceFactory m_Factory;

        public CommandList m_CommandList;
        public Pipeline m_Pipeline;
        //public Pipeline m_TexturePipeline;

        private DeviceBuffer m_VertexBuffer;
        private DeviceBuffer m_IndexBuffer;

#if DEBUG
        private bool m_Submiting;
#endif
        private const int InitialBatchSize = 256;
        SpriteBatchItem[] m_Batch;
        private int _currentBatchCount;
        private SpriteVertex[] m_Verts;

        public SpriteBatcher(AWindow window) // <- this is an SLD2Window Wrapper
        {
            m_GraphicsDevice = window.GraphicsDevice;
            m_Factory = window.GraphicsDevice.ResourceFactory;

            m_VertexBuffer = m_Factory.CreateBuffer(
                new BufferDescription(SpriteVertex.VertexDescriptor.Stride * 4, BufferUsage.VertexBuffer | BufferUsage.Dynamic)
                    );

            const uint SIZE_IN_BYTES = sizeof(ushort) * 6;
            m_IndexBuffer = m_Factory.CreateBuffer(
                new BufferDescription(SIZE_IN_BYTES, BufferUsage.IndexBuffer)
                );


            m_Batch = new SpriteBatchItem[InitialBatchSize];
            m_Verts = new SpriteVertex[4];

            VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2)//,
                //new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4)
                    );

            var pipelineDescription = new GraphicsPipelineDescription
            {
                BlendState = BlendStateDescription.SingleOverrideBlend,
                DepthStencilState = new DepthStencilStateDescription(
                    depthTestEnabled: true,
                    depthWriteEnabled: true,
                    comparisonKind: ComparisonKind.LessEqual
                    ),
                RasterizerState = new RasterizerStateDescription(
                    cullMode: FaceCullMode.Back,
                    fillMode: PolygonFillMode.Solid,
                    frontFace: FrontFace.Clockwise,
                    depthClipEnabled: true,
                    scissorTestEnabled: false
                    ),
                PrimitiveTopology = PrimitiveTopology.TriangleStrip,
                ResourceLayouts = Array.Empty<ResourceLayout>(),
                ShaderSet = new ShaderSetDescription(
                    vertexLayouts: new VertexLayoutDescription[] { vertexLayout },
                    shaders: GetShaders()
                    ),
                ResourceBindingModel = ResourceBindingModel.Improved,
                Outputs = m_GraphicsDevice.SwapchainFramebuffer.OutputDescription,
            };

            m_CommandList = m_Factory.CreateCommandList();
            m_Pipeline = m_Factory.CreateGraphicsPipeline(pipelineDescription); //Null reference exeption

        }

        private ref SpriteBatchItem CreateBatchItem()
        {
            if (_currentBatchCount >= m_Batch.Length)
            {
                Array.Resize(ref m_Batch, m_Batch.Length * 2);
            }

            return ref m_Batch[_currentBatchCount++];
        }

        private Shader[] GetShaders()
        {
            ShaderDescription vertexShaderDesc = new ShaderDescription(
                ShaderStages.Vertex,
                Encoding.UTF8.GetBytes(VertexCode),
                "main");
            ShaderDescription fragmentShaderDesc = new ShaderDescription(
                ShaderStages.Fragment,
                Encoding.UTF8.GetBytes(FragmentCode),
                "main");

            return m_Factory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);
        }


        public void SumbitBegin(/*CommandList commandList*/)
        {
#if DEBUG
            if (m_Submiting)
            {
                return; //TODO: throw EX
            }
            m_Submiting = true;
#endif

            //m_CommandList = commandList;
            //m_CommandList.SetPipeline(m_Pipeline);
            m_CommandList.Begin();
            m_CommandList.SetFramebuffer(m_GraphicsDevice.SwapchainFramebuffer);
            m_CommandList.SetPipeline(m_Pipeline);

            m_CommandList.ClearColorTarget(0, RgbaFloat.Black);
        }

        public void DrawRect(
            in Rect rect,
            in RgbaFloat color
            )
        {
#if DEBUG
            if (!m_Submiting)
            {
                return; //TODO: throw EX
            }
#endif

            ref var batchItem = ref CreateBatchItem();
            
            batchItem.VertexTL = new SpriteVertex
            {
                Position = rect.TL,
                Color = color,
                //UV = new Vector2(0, 1),
            };
            batchItem.VertexTR = new SpriteVertex
            {
                Position = rect.TR,
                Color = color,
                //UV = new Vector2(1, 1),
            };
            batchItem.VertexBL = new SpriteVertex
            {
                Position = rect.BL,
                Color = color,
                //UV = new Vector2(1, 0),
            };
            batchItem.VertexBR = new SpriteVertex
            {
                Position = rect.BR,
                Color = color,
                //UV = new Vector2(0, 0),
            };

            //Log($"Submiting: {batchItem}", ConsoleColor.DarkGreen);
        }
        

        public void SubmitEnd()
        {
#if DEBUG
            if (!m_Submiting)
            {
                return; //TODO: throw EX
            }
            m_Submiting = false;
#endif

            for (int i = 0; i < _currentBatchCount; i++)
            {
                ref var batchItem = ref m_Batch[i];

                m_Verts[0] = batchItem.VertexTL;
                m_Verts[1] = batchItem.VertexTR;
                m_Verts[2] = batchItem.VertexBL;
                m_Verts[3] = batchItem.VertexBR;

                

                m_CommandList.UpdateBuffer(m_VertexBuffer, 0, m_Verts);
                var indexes = new ushort[] { 0, 2, 3, 0, 3, 1 }; //TODO: Make static, set once, or smthng
                m_CommandList.UpdateBuffer(m_IndexBuffer, 0, indexes);

                Log($"Drawing | {batchItem}");

                m_CommandList.SetVertexBuffer(0, m_VertexBuffer);
                m_CommandList.SetIndexBuffer(m_IndexBuffer, IndexFormat.UInt16);

                m_CommandList.DrawIndexed(4);
            }

            m_CommandList.End();
            m_GraphicsDevice.SubmitCommands(m_CommandList);
            m_GraphicsDevice.SwapBuffers();
            _currentBatchCount = 0;
        }

        static void Log(string msg, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private struct SpriteBatchItem
        {
            public Texture texture;
            public SpriteVertex VertexTL;
            public SpriteVertex VertexTR;
            public SpriteVertex VertexBL;
            public SpriteVertex VertexBR;

            public override string ToString()
            {
                return $"Sprite:[{VertexTL.Position}|{VertexTR.Position}|{VertexBL.Position}|{VertexBR.Position}]";
            }
        }
    }

    //struct VertexPositionColor
    //{
    //    public const uint SizeInBytes = 24;
    //    public Vector2 Position;
    //    public RgbaFloat Color;
    //    public VertexPositionColor(Vector2 position, RgbaFloat color)
    //    {
    //        Position = position;
    //        Color = color;
    //    }
    //}
    
    public struct SpriteVertex
    {
        //public const uint BYTES_SIZE = sizeof(float) * 8;

        public Vector2 Position; //8 bytes
        public RgbaFloat Color;  //
        //public Vector2 UV;       //

        public static readonly VertexLayoutDescription VertexDescriptor = new VertexLayoutDescription(
            new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4)//,
            //new VertexElementDescription("TextCoord", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2)
            );

        public override string ToString()
        {
            return $"({Position}||{Color})";
        }
    }
}
