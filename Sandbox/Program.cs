using AptumEngine.Core;
using AptumEngine.GUI;
using System;

namespace Sandbox
{
    class Program
    {
        static WindowConfig config = new WindowConfig()
        {
            initPos = new Vector2(200, 200),
            initSize = new Vector2(960, 960),
            title = "Window",
            resizable = true,
        };
        
        static readonly AWindow win = new AWindow(config);
        static void Main(string[] args) =>
            new Application()
            //.AddLayer(new AptumGUILayer(win))
            .AddLayer(new EventListener(win))
            .SetupEvents(win)
            .Run();
    }


    class EventListener : IApplicationLayer
    {
        AWindow m_Window;

        public EventListener(AWindow window)
        {
            m_Window = window;
        }

        public void OnAttach()
        {
            
        }

        public void OnDetach()
        {
            
        }

        public void OnEvent(Event e)
        {
            Console.WriteLine(e.ToString());
            if (e.Capture<MouseButtonDownEvent>(out var md))
            {
                Console.WriteLine("MOUSE DOWN");
            }
        }

        public void OnUpdate()
        {
            if (m_Window.PumpEvents()) { }
        }
    }
}
