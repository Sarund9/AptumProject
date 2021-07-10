using System;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using System.Text;
using Veldrid.SPIRV;

namespace AptumEngine.Core
{
    public struct WindowConfig
    {
        public Vector2 initSize;
        public Vector2 initPos;
        public string title;
        public bool borderless;
        public bool resizable;

        public static WindowConfig Default => new WindowConfig
        {
            initPos = new Vector2(100),
            initSize = new Vector2(960, 540),
            title = "Window",
            resizable = true,
            borderless = false,
        };
    }
    public class AWindow : AObject
    {
        Sdl2Window m_Window;

        GraphicsDevice m_GraphicsDevice;
        

        //Event Info
        //bool b_CapsLock = false;
        struct EventCapture
        {
            public ModifierKeys lastModifiers;
            //public bool capsLock;
            public Vector2 mousePosition;
        }
        EventCapture eventCapture;


        public AWindow() : this(WindowConfig.Default) {}
        public AWindow(in WindowConfig config)
        {
            WindowCreateInfo windowCI = new WindowCreateInfo()
            {
                X = (int)config.initPos.x,
                Y = (int)config.initPos.y,
                WindowWidth = (int)config.initSize.x,
                WindowHeight = (int)config.initSize.y,
                WindowTitle = config.title,
            };
            m_Window = VeldridStartup.CreateWindow(ref windowCI);
            m_Window.BorderVisible = !config.borderless;
            m_Window.Resizable = config.resizable;

            GraphicsDeviceOptions options = new GraphicsDeviceOptions
            {
                PreferStandardClipSpaceYDirection = true,
                PreferDepthRangeZeroToOne = true
            };

            m_GraphicsDevice = VeldridStartup.CreateGraphicsDevice(m_Window, options, GraphicsBackend.Vulkan);

        }

        #region WIN_PROP

        public bool Exists => m_Window.Exists;

        public float Opacity
        {
            get => m_Window.Opacity;
            set => m_Window.Opacity = value;
        }
        public bool Borderless
        {
            get => !m_Window.BorderVisible;
            set => m_Window.BorderVisible = !value;
        }
        public bool Resizable
        {
            get => m_Window.Resizable;
            set => m_Window.Resizable = value;
        }

        public Vector2 Size
        {
            get => m_Window.Bounds.Size;
        }

        #endregion

        public GraphicsDevice GraphicsDevice => m_GraphicsDevice;
        public Sdl2Window SDLWindow => m_Window;

        /// <summary> Pumps window Events, and calls  </summary>
        public bool PumpEvents()
        {
            m_Window.PumpEvents();
            return m_Window.Exists;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);

            m_GraphicsDevice.Dispose();
            m_GraphicsDevice = null;
        }

        public void SetupEvents(Application appInstance)
        {
            m_Window.KeyDown += (KeyEvent key) =>
            {
                //if (key.Key == Key.CapsLock)
                //{
                //    eventCapture.capsLock = true;
                //}
                eventCapture.lastModifiers = key.Modifiers;

                if (key.Repeat)
                {
                    appInstance.HandleEvent(new KeyHeldEvent(this, key.Key, key.Modifiers));
                }
                else
                {
                    appInstance.HandleEvent(new KeyDownEvent(this, key.Key, key.Modifiers));
                }
            };
            m_Window.KeyUp += (KeyEvent key) =>
            {
                //if (key.Key == Key.CapsLock)
                //{
                //    eventCapture.capsLock = false;
                //}
                appInstance.HandleEvent(new KeyUpEvent(this, key.Key, key.Modifiers));
                eventCapture.lastModifiers = key.Modifiers;
            };

            m_Window.MouseDown += (Veldrid.MouseEvent mouse) =>
            {
                appInstance.HandleEvent(
                    new MouseButtonDownEvent(this, eventCapture.lastModifiers,
                    mouse.MouseButton, eventCapture.mousePosition));
            };
            m_Window.MouseUp += (Veldrid.MouseEvent mouse) =>
            {
                appInstance.HandleEvent(
                    new MouseButtonUpEvent(this, eventCapture.lastModifiers,
                    mouse.MouseButton, eventCapture.mousePosition));
            };
            m_Window.MouseEntered += () =>
            {
                appInstance.HandleEvent(
                    new MouseEnteredWindowEvent(this, eventCapture.lastModifiers,
                    eventCapture.mousePosition));
            };
            m_Window.MouseLeft += () =>
            {
                appInstance.HandleEvent(
                    new MouseLeftWindowEvent(this, eventCapture.lastModifiers,
                    eventCapture.mousePosition));
            };
            m_Window.MouseMove += (MouseMoveEventArgs args) =>
            {
                appInstance.HandleEvent(
                    new MouseMovedEvent(this, eventCapture.lastModifiers, args.MousePosition)
                    );
            };
            m_Window.MouseWheel += (MouseWheelEventArgs args) =>
            {
                appInstance.HandleEvent(
                    new MouseScrollEvent(this, eventCapture.lastModifiers,
                    eventCapture.mousePosition, args.WheelDelta));
            };

            m_Window.Moved += (Point p) => 
            {
                appInstance.HandleEvent(new WindowMovedEvent(this, new Vector2(p.X, p.Y)));
            };
            m_Window.Resized += () =>
            {
                appInstance.HandleEvent(new WindowResizedEvent(this));
            };
            m_Window.Shown += () =>
            {
                appInstance.HandleEvent(new WindowShownEvent(this));
            };
            m_Window.Closed += () =>
            {
                appInstance.HandleEvent(new WindowClosedEvent(this));
            };
            m_Window.Closing += () =>
            {
                appInstance.HandleEvent(new WindowClosingEvent(this));
            };
            m_Window.FocusGained += () =>
            {
                appInstance.HandleEvent(new WindowFocusGained(this));
            };
            m_Window.FocusLost += () => 
            {
                appInstance.HandleEvent(new WindowFocusLost(this));
            };
            m_Window.Hidden += () => 
            {
                appInstance.HandleEvent(new WindowHidden(this));
            };
            m_Window.Exposed += () => 
            {
                appInstance.HandleEvent(new WindowExposed(this));
            };
        }

    }
}

#region OLD_CODE
/*

        private void SetupEventSystem() //TODO: Better Event Handling
        {
            //Keyboard
            m_Window.KeyDown += (key) => Application.Instance.HandleEvent(new KeyDownEvent(this, key.Key, key.Modifiers));
            m_Window.KeyUp += (key) => Application.Instance.HandleEvent(new KeyUpEvent(this, key.Key, key.Modifiers));

            //Mouse
            m_Window.MouseDown += (mouse) => Console.WriteLine($"MOUSE DOWN: {mouse.MouseButton}");
            m_Window.MouseEntered += () => Console.WriteLine($"MOUSE ENTER");
            m_Window.MouseLeft += () => Console.WriteLine($"MOUSE LEFT");
            m_Window.MouseMove += (MouseMoveEventArgs args) => Console.WriteLine($"MOUSE MOVE: {args.MousePosition}");
            m_Window.MouseUp += (args) => Console.WriteLine($"MOUSE UP: {args.MouseButton}");
            m_Window.MouseWheel += (MouseWheelEventArgs args) => Console.WriteLine($"MOUSE MOVE: {args.WheelDelta}");
            m_Window.DragDrop += (DragDropEvent drag) => Console.WriteLine("DRAG DROP");

            //Application
            m_Window.Moved += (Point p) => Application.Instance.HandleEvent(new WindowMovedEvent(this, new Vector2(p.X, p.Y)));
            m_Window.Resized += () => Application.Instance.HandleEvent(new WindowResizedEvent(this));
            m_Window.Shown += () => Application.Instance.HandleEvent(new WindowShownEvent(this));
            m_Window.Closed += () => Application.Instance.HandleEvent(new WindowClosedEvent(this));
            m_Window.Closing += () => Application.Instance.HandleEvent(new WindowClosingEvent(this));
            m_Window.FocusGained += () => Application.Instance.HandleEvent(new WindowFocusGained(this));
            m_Window.FocusLost += () => Application.Instance.HandleEvent(new WindowFocusLost(this));
            m_Window.Hidden += () => Application.Instance.HandleEvent(new WindowHidden(this));
            m_Window.Exposed += () => Application.Instance.HandleEvent(new WindowExposed(this));
        }
//CommandList m_CommandList;
//DeviceBuffer _vertexBuffer;
//DeviceBuffer _indexBuffer;
//Pipeline m_Pipeline;
public struct VertexPositionColor
{
    public const uint SizeInBytes = 24;
    public Vector2 Position;
    public RgbaFloat Color;
    public VertexPositionColor(Vector2 position, RgbaFloat color)
    {
        Position = position;
        Color = color;
    }
}
public void OnUpdate()
        {
            if (m_Window.Exists)
            {
                m_Window.PumpEvents();
                
            }
        }
void Draw()
        {
            m_CommandList.Begin();

            m_CommandList.SetFramebuffer(m_GraphicsDevice.SwapchainFramebuffer);
            m_CommandList.ClearColorTarget(0, RgbaFloat.Black);

            m_CommandList.SetVertexBuffer(0, _vertexBuffer);
            m_CommandList.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
            m_CommandList.SetPipeline(m_Pipeline);

            m_CommandList.DrawIndexed(
                indexCount: 4,
                instanceCount: 1,
                indexStart: 0,
                vertexOffset: 0,
                instanceStart: 0);

            m_CommandList.End();
            m_GraphicsDevice.SubmitCommands(m_CommandList);

            m_GraphicsDevice.SwapBuffers();
        }
        private void SetupEventSystem()
        {
            //Keyboard
            m_Window.KeyDown += (key) => Application.Instance.HandleEvent(new KeyDownEvent(this, key.Key, key.Modifiers));
            m_Window.KeyUp += (key) => Application.Instance.HandleEvent(new KeyUpEvent(this, key.Key, key.Modifiers));

            //Mouse
            m_Window.MouseDown += (mouse) => Console.WriteLine($"MOUSE DOWN: {mouse.MouseButton}");
            m_Window.MouseEntered += () => Console.WriteLine($"MOUSE ENTER");
            m_Window.MouseLeft += () => Console.WriteLine($"MOUSE LEFT");
            m_Window.MouseMove += (MouseMoveEventArgs args) => Console.WriteLine($"MOUSE MOVE: {args.MousePosition}");
            m_Window.MouseUp += (args) => Console.WriteLine($"MOUSE UP: {args.MouseButton}");
            m_Window.MouseWheel += (MouseWheelEventArgs args) => Console.WriteLine($"MOUSE MOVE: {args.WheelDelta}");
            m_Window.DragDrop += (DragDropEvent drag) => Console.WriteLine("DRAG DROP");

            //Application
            m_Window.Moved += (Point p) => Application.Instance.HandleEvent(new WindowMovedEvent(this, new Vector2(p.X, p.Y)));
            m_Window.Resized += () => Application.Instance.HandleEvent(new WindowResizedEvent(this));
            m_Window.Shown += () => Application.Instance.HandleEvent(new WindowShownEvent(this));
            m_Window.Closed += () => Application.Instance.HandleEvent(new WindowClosedEvent(this));
            m_Window.Closing += () => Application.Instance.HandleEvent(new WindowClosingEvent(this));
            m_Window.FocusGained += () => Application.Instance.HandleEvent(new WindowFocusGained(this));
            m_Window.FocusLost += () => Application.Instance.HandleEvent(new WindowFocusLost(this));
            m_Window.Hidden += () => Application.Instance.HandleEvent(new WindowHidden(this));
            m_Window.Exposed += () => Application.Instance.HandleEvent(new WindowExposed(this));


        }
        void CreateResources()
        {
            ResourceFactory factory = m_GraphicsDevice.ResourceFactory;

            #region CREATE_GPU_DATA
            //Create a Quad
            VertexPositionColor[] quadVertices =
            {
                new VertexPositionColor(new Vector2(-.75f, .75f), RgbaFloat.Red),
                new VertexPositionColor(new Vector2(.75f, .75f), RgbaFloat.Green),
                new VertexPositionColor(new Vector2(-.75f, -.75f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector2(.75f, -.75f), RgbaFloat.Yellow)
            };

            //Buffer descrption ####
            BufferDescription vbDescription = new BufferDescription(
                4 * VertexPositionColor.SizeInBytes,
                BufferUsage.VertexBuffer);

            //Create a Buffer for the GPU containing the Quad
            _vertexBuffer = factory.CreateBuffer(vbDescription);

            //Send buffer to the GPU
            m_GraphicsDevice.UpdateBuffer(_vertexBuffer, 0, quadVertices);


            ushort[] quadIndices = { 0, 1, 2, 3 };
            BufferDescription ibDescription = new BufferDescription(
                4 * sizeof(ushort),
                BufferUsage.IndexBuffer);
            _indexBuffer = factory.CreateBuffer(ibDescription);
            m_GraphicsDevice.UpdateBuffer(_indexBuffer, 0, quadIndices);

            #endregion

            #region PREPARE_SHADERS
            //
            VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));

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
            Shader[] shaders = factory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);

            #endregion

            // Create pipeline description
            GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription
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
                ResourceLayouts = System.Array.Empty<ResourceLayout>(),
                ShaderSet = new ShaderSetDescription(
                    vertexLayouts: new VertexLayoutDescription[] { vertexLayout },
                    shaders: shaders
                    ),
                Outputs = m_GraphicsDevice.SwapchainFramebuffer.OutputDescription
            };

            m_Pipeline = factory.CreateGraphicsPipeline(pipelineDescription);

            m_CommandList = factory.CreateCommandList();
        }
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


//*/
#endregion
