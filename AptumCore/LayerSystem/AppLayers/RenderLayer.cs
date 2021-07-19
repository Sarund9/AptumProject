using Veldrid;
using System.Text;
using Veldrid.SPIRV;
using System.Collections.Generic;
using Veldrid.StartupUtilities;
//using KeyEvent = Veldrid.KeyEvent;
//using MouseEvent = Veldrid.MouseEvent;

namespace AptumEngine.Core
{
    public class RenderLayer : IApplicationLayer
    {
        //        private const string VertexCode = @"
        //#version 450
        //layout(location = 0) in vec2 Position;
        //layout(location = 1) in vec4 Color;
        //layout(location = 0) out vec4 fsin_Color;
        //void main()
        //{
        //    gl_Position = vec4(Position, 0, 1);
        //    fsin_Color = Color;
        //}";

        //        private const string FragmentCode = @"
        //#version 450
        //layout(location = 0) in vec4 fsin_Color;
        //layout(location = 0) out vec4 fsout_Color;
        //void main()
        //{
        //    fsout_Color = fsin_Color;
        //}";

        ////Render Data
        //CommandList m_CommandList;
        //DeviceBuffer _vertexBuffer;
        //DeviceBuffer _indexBuffer;
        //Pipeline m_Pipeline;

        List<AWindow> m_Windows;

        public RenderLayer(params AWindow[] targets)
        {
            m_Windows = new List<AWindow>(targets.Length);
            for (int i = 0; i < targets.Length; i++)
            {
                m_Windows.Add(targets[i]);
            }


        }

        public void OnAttach()
        {
            //CreateResources();
        }

        public void OnDetach()
        {

        }

        public void OnEvent(Event e)
        {

        }

        public void OnUpdate()
        {
            while (m_Windows.Count > 0)
            {
                for (int i = 0; i < m_Windows.Count; i++)
                {
                    //m_Windows[i].OnUpdate();
                        
                    if (!m_Windows[i].Exists)
                    {
                        m_Windows.RemoveAt(i);
                        i--;
                    }
                }
            }
            
        }
        //private void Draw(Window window)
        //{
        //    m_CommandList.Begin();

        //    m_CommandList.SetFramebuffer(m_Target.GraphicsDevice.SwapchainFramebuffer);
        //    m_CommandList.ClearColorTarget(0, RgbaFloat.Black);

        //    m_CommandList.SetVertexBuffer(0, _vertexBuffer);
        //    m_CommandList.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
        //    m_CommandList.SetPipeline(m_Pipeline);

        //    m_CommandList.DrawIndexed(
        //        indexCount: 4,
        //        instanceCount: 1,
        //        indexStart: 0,
        //        vertexOffset: 0,
        //        instanceStart: 0);

        //    m_CommandList.End();
        //    m_Target.GraphicsDevice.SubmitCommands(m_CommandList);

        //    m_Target.GraphicsDevice.SwapBuffers();
        //}
        //private void CreateResources()
        //{
        //    ResourceFactory factory = m_Target.GraphicsDevice.ResourceFactory;

        //    #region CREATE_GPU_DATA
        //    //Create a Quad
        //    VertexPositionColor[] quadVertices =
        //    {
        //        new VertexPositionColor(new Vector2(-.75f, .75f), RgbaFloat.Red),
        //        new VertexPositionColor(new Vector2(.75f, .75f), RgbaFloat.Green),
        //        new VertexPositionColor(new Vector2(-.75f, -.75f), RgbaFloat.Blue),
        //        new VertexPositionColor(new Vector2(.75f, -.75f), RgbaFloat.Yellow)
        //    };

        //    //Buffer descrption ####
        //    BufferDescription vbDescription = new BufferDescription(
        //        4 * VertexPositionColor.SizeInBytes,
        //        BufferUsage.VertexBuffer);

        //    //Create a Buffer for the GPU containing the Quad
        //    _vertexBuffer = factory.CreateBuffer(vbDescription);

        //    //Send buffer to the GPU
        //    m_Target.GraphicsDevice.UpdateBuffer(_vertexBuffer, 0, quadVertices);


        //    ushort[] quadIndices = { 0, 1, 2, 3 };
        //    BufferDescription ibDescription = new BufferDescription(
        //        4 * sizeof(ushort),
        //        BufferUsage.IndexBuffer);
        //    _indexBuffer = factory.CreateBuffer(ibDescription);
        //    m_Target.GraphicsDevice.UpdateBuffer(_indexBuffer, 0, quadIndices);

        //    #endregion

        //    #region PREPARE_SHADERS
        //    //
        //    VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
        //        new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
        //        new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));

        //    //Create Shader Descriptions from Shader Code ####
        //    ShaderDescription vertexShaderDesc = new ShaderDescription(
        //        ShaderStages.Vertex,
        //        Encoding.UTF8.GetBytes(VertexCode),
        //        "main");
        //    ShaderDescription fragmentShaderDesc = new ShaderDescription(
        //        ShaderStages.Fragment,
        //        Encoding.UTF8.GetBytes(FragmentCode),
        //        "main");
        //    //Generate Shaders
        //    Shader[] shaders = factory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);

        //    #endregion

        //    // Create pipeline description
        //    GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription
        //    {
        //        BlendState = BlendStateDescription.SingleOverrideBlend,
        //        DepthStencilState = new DepthStencilStateDescription(
        //            depthTestEnabled: true,
        //            depthWriteEnabled: true,
        //            comparisonKind: ComparisonKind.LessEqual
        //            ),
        //        RasterizerState = new RasterizerStateDescription(
        //            cullMode: FaceCullMode.Back,
        //            fillMode: PolygonFillMode.Solid,
        //            frontFace: FrontFace.Clockwise,
        //            depthClipEnabled: true,
        //            scissorTestEnabled: false
        //            ),
        //        PrimitiveTopology = PrimitiveTopology.TriangleStrip,
        //        ResourceLayouts = System.Array.Empty<ResourceLayout>(),
        //        ShaderSet = new ShaderSetDescription(
        //            vertexLayouts: new VertexLayoutDescription[] { vertexLayout },
        //            shaders: shaders
        //            ),
        //        Outputs = m_Target.GraphicsDevice.SwapchainFramebuffer.OutputDescription
        //    };

        //    m_Pipeline = factory.CreateGraphicsPipeline(pipelineDescription);

        //    m_CommandList = factory.CreateCommandList();
        //}

        //struct WindowInstance
        //{
        //    Window m_Target;
        //    CommandList m_CommandList;
        //    DeviceBuffer _vertexBuffer;
        //    DeviceBuffer _indexBuffer;
        //    Pipeline m_Pipeline;
        //    public WindowInstance(Window window) : this()
        //    {
        //        m_Target = window;
        //        CreateResources();
        //    }
        //    void CreateResources()
        //    {
        //        ResourceFactory factory = m_Target.GraphicsDevice.ResourceFactory;

        //        #region CREATE_GPU_DATA
        //        //Create a Quad
        //        VertexPositionColor[] quadVertices =
        //        {
        //        new VertexPositionColor(new Vector2(-.75f, .75f), RgbaFloat.Red),
        //        new VertexPositionColor(new Vector2(.75f, .75f), RgbaFloat.Green),
        //        new VertexPositionColor(new Vector2(-.75f, -.75f), RgbaFloat.Blue),
        //        new VertexPositionColor(new Vector2(.75f, -.75f), RgbaFloat.Yellow)
        //        };

        //        //Buffer descrption ####
        //        BufferDescription vbDescription = new BufferDescription(
        //            4 * VertexPositionColor.SizeInBytes,
        //            BufferUsage.VertexBuffer);

        //        //Create a Buffer for the GPU containing the Quad
        //        _vertexBuffer = factory.CreateBuffer(vbDescription);

        //        //Send buffer to the GPU
        //        m_Target.GraphicsDevice.UpdateBuffer(_vertexBuffer, 0, quadVertices);


        //        ushort[] quadIndices = { 0, 1, 2, 3 };
        //        BufferDescription ibDescription = new BufferDescription(
        //            4 * sizeof(ushort),
        //            BufferUsage.IndexBuffer);
        //        _indexBuffer = factory.CreateBuffer(ibDescription);
        //        m_Target.GraphicsDevice.UpdateBuffer(_indexBuffer, 0, quadIndices);

        //        #endregion

        //        #region PREPARE_SHADERS
        //        //
        //        VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
        //            new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
        //            new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));

        //        //Create Shader Descriptions from Shader Code ####
        //        ShaderDescription vertexShaderDesc = new ShaderDescription(
        //            ShaderStages.Vertex,
        //            Encoding.UTF8.GetBytes(VertexCode),
        //            "main");
        //        ShaderDescription fragmentShaderDesc = new ShaderDescription(
        //            ShaderStages.Fragment,
        //            Encoding.UTF8.GetBytes(FragmentCode),
        //            "main");
        //        //Generate Shaders
        //        Shader[] shaders = factory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);

        //        #endregion

        //        // Create pipeline description
        //        GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription
        //        {
        //            BlendState = BlendStateDescription.SingleOverrideBlend,
        //            DepthStencilState = new DepthStencilStateDescription(
        //                depthTestEnabled: true,
        //                depthWriteEnabled: true,
        //                comparisonKind: ComparisonKind.LessEqual
        //                ),
        //            RasterizerState = new RasterizerStateDescription(
        //                cullMode: FaceCullMode.Back,
        //                fillMode: PolygonFillMode.Solid,
        //                frontFace: FrontFace.Clockwise,
        //                depthClipEnabled: true,
        //                scissorTestEnabled: false
        //                ),
        //            PrimitiveTopology = PrimitiveTopology.TriangleStrip,
        //            ResourceLayouts = System.Array.Empty<ResourceLayout>(),
        //            ShaderSet = new ShaderSetDescription(
        //                vertexLayouts: new VertexLayoutDescription[] { vertexLayout },
        //                shaders: shaders
        //                ),
        //            Outputs = m_Target.GraphicsDevice.SwapchainFramebuffer.OutputDescription
        //        };

        //        m_Pipeline = factory.CreateGraphicsPipeline(pipelineDescription);

        //        m_CommandList = factory.CreateCommandList();
        //    }
        //}
    }
}
