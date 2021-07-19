using AptumEngine.Core;
using System;

namespace AptumEngine.GUI
{
    public class AptumGUILayer : IApplicationLayer
    {
        AWindow m_Window;

        GUIRenderer m_Render;

        public AptumGUILayer(AWindow window)
        {
            m_Window = window;
            m_Render = new GUIRenderer(window);
        }

        public void OnAttach()
        {
            
        }

        public void OnDetach()
        {
            
        }

        public void OnEvent(Event e)
        {
            
            
            
        }

        public void OnUpdate()
        {
            if (m_Window.PumpEvents())
            {
                m_Render.Begin();

                m_Render.DrawRect(new Rect(300, 150, 400, 200), Color.DarkCyan);
                m_Render.DrawRect(new Rect(200, 100, 400, 200), Color.Orange);

                m_Render.End();
            }
        }
    }
}
