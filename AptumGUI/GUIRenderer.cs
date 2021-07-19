using AptumEngine.Core;
using System;
using System.Text;
using Veldrid;
using Veldrid.SPIRV;

namespace AptumEngine.GUI
{
    public struct VertexColor
    {
        public Vector2 position;
        public Color color;

        public VertexColor(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public static implicit operator VertexColor(Vector2 v) =>
            new VertexColor
            {
                position = v,
                color = Color.White
            };
    }
    public struct DrawItemRect
    {
        public Vector2 v00; //TL
        public Vector2 v01; //BL
        public Vector2 v10; //TR
        public Vector2 v11; //BR
        public Color color;
    }

    
    public class GUIRenderer
    {
        const string SHADER_RECT_V = @"
#version 450

layout(location = 0) in vec2 Position;
layout(location = 1) in vec4 Color;
layout(location = 0) out vec4 fsin_Color;
void main()
{
    gl_Position = vec4(Position, 0, 1);
    fsin_Color = Color;
}";
        const string SHADER_RECT_F = @"
#version 450
layout(location = 0) in vec4 fsin_Color;
layout(location = 0) out vec4 fsout_Color;
void main()
{
    fsout_Color = fsin_Color;
}";


        const int INIT_ARRAY_SIZE = 256;

        private AWindow m_Window;
        
        private CommandList m_CommandList;
        private Pipeline m_Pipeline;
        private GraphicsDevice m_GraphicsDevice;
        private ResourceFactory Fact => m_GraphicsDevice.ResourceFactory;

        private DeviceBuffer m_VertexBuffer;
        private DeviceBuffer m_IndexBuffer;

        //Renderers
        DynamicArray<DrawItemRect> a_RectDraws;

        //Shader Data
        private VertexColor[] a_Verts; //Vec
        private readonly ushort[] a_Tris = new ushort[] { 0, 2, 3, 0, 3, 1 };

        //Other
        Vector2 v_InitWinSize;

        public GUIRenderer(AWindow window)
        {
            m_Window = window;
            m_GraphicsDevice = m_Window.GraphicsDevice;
            ResourceFactory factory = m_GraphicsDevice.ResourceFactory;

            v_InitWinSize = window.Size;

            #region BUFFERS_ARRAYS
            m_VertexBuffer = factory.CreateBuffer(
                new BufferDescription(24 * 4, BufferUsage.VertexBuffer | BufferUsage.Dynamic)        //Vec
                    );

            const uint SIZE_IN_BYTES = sizeof(ushort) * 6;
            m_IndexBuffer = factory.CreateBuffer(
                new BufferDescription(SIZE_IN_BYTES, BufferUsage.IndexBuffer)
                );

            a_RectDraws = new DynamicArray<DrawItemRect>(INIT_ARRAY_SIZE);
            a_Verts = new VertexColor[4]; //Vec
            #endregion

            #region SHADERS
            ShaderDescription vertexShaderDesc = new ShaderDescription(
                ShaderStages.Vertex,
                Encoding.UTF8.GetBytes(SHADER_RECT_V),
                "main");
            ShaderDescription fragmentShaderDesc = new ShaderDescription(
                ShaderStages.Fragment,
                Encoding.UTF8.GetBytes(SHADER_RECT_F),
                "main");

            Shader[] shaders = factory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);

            VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.Position, VertexElementFormat.Float2),
                new VertexElementDescription("Color", VertexElementSemantic.Color, VertexElementFormat.Float4));

            //VertexLayoutDescription fragmentLayout = new VertexLayoutDescription(
            //    new VertexElementDescription("Color", VertexElementSemantic.Color, VertexElementFormat.Float4)
            //    );
            #endregion

            //Log($"COMPILED {shaders.Length} SHADERS");

            #region PIPELINE
            var pipelineDescription = new GraphicsPipelineDescription
            {
                BlendState = BlendStateDescription.SingleOverrideBlend,
                DepthStencilState = new DepthStencilStateDescription(
                    depthTestEnabled: true,
                    depthWriteEnabled: true,
                    comparisonKind: ComparisonKind.LessEqual
                    ),
                RasterizerState = new RasterizerStateDescription(
                    cullMode: FaceCullMode.None,
                    fillMode: PolygonFillMode.Solid,
                    frontFace: FrontFace.Clockwise,
                    depthClipEnabled: true,
                    scissorTestEnabled: false
                    ),
                PrimitiveTopology = PrimitiveTopology.TriangleStrip,
                ResourceLayouts = Array.Empty<ResourceLayout>(),
                ShaderSet = new ShaderSetDescription(
                    vertexLayouts: new VertexLayoutDescription[] { vertexLayout },
                    shaders: shaders
                    ),
                ResourceBindingModel = ResourceBindingModel.Improved,
                Outputs = m_GraphicsDevice.SwapchainFramebuffer.OutputDescription,
            };

            m_Pipeline = factory.CreateGraphicsPipeline(ref pipelineDescription);
            #endregion

            m_CommandList = factory.CreateCommandList();
        }

        //void InitRender()
        //{

        //}

        public void Begin()
        {
            m_CommandList.Begin();
            m_CommandList.SetFramebuffer(m_GraphicsDevice.SwapchainFramebuffer);
            m_CommandList.SetPipeline(m_Pipeline);

            //TEMP: TODO: Clear Instruction
            //m_CommandList.ClearColorTarget(0, RgbaFloat.Black);
        }

        public void DrawRect(
            in Rect rect,
            in Color color
            )
        {
            ref var item = ref a_RectDraws.Get();

            Vector2 start = new Vector2
            {
                x = (rect.x / v_InitWinSize.x * 2) - 1,
                y = (rect.y / v_InitWinSize.y * -2) + 1,
            };
            Vector2 end = new Vector2
            {
                x = ((rect.x + rect.w) / v_InitWinSize.x * 2) - 1,
                y = ((rect.y + rect.h) / v_InitWinSize.y * -2) + 1,
            };

            item.v00 = start;                       //rect.Position;
            item.v01 = new Vector2(start.x, end.y); //rect.Position + Vector2.Up * rect.h;
            item.v10 = new Vector2(end.x, start.y); //rect.Position + Vector2.Right * rect.w;
            item.v11 = end;                         //rect.Position + rect.Size;
            item.color = color;
        }

        public void End()
        {

            for (int i = 0; i < a_RectDraws.Count; i++)
            {
                ref var batchItem = ref a_RectDraws[i];

                a_Verts[0] = new VertexColor(batchItem.v00, batchItem.color);
                a_Verts[1] = new VertexColor(batchItem.v01, batchItem.color);
                a_Verts[2] = new VertexColor(batchItem.v10, batchItem.color);
                a_Verts[3] = new VertexColor(batchItem.v11, batchItem.color);

                m_CommandList.UpdateBuffer(m_VertexBuffer, 0, a_Verts);

                m_CommandList.UpdateBuffer(m_IndexBuffer, 0, a_Tris);

                //Log($"Drawing | {batchItem}");

                m_CommandList.SetVertexBuffer(0, m_VertexBuffer);
                m_CommandList.SetIndexBuffer(m_IndexBuffer, IndexFormat.UInt16);
                m_CommandList.DrawIndexed(6, 1, 0, 0, 0);
            }

            m_CommandList.End();
            m_GraphicsDevice.SubmitCommands(m_CommandList);
            m_GraphicsDevice.SwapBuffers();
            a_RectDraws.Reset();
        }

        static void Log(string msg, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
