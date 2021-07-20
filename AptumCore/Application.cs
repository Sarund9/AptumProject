using System.Collections.Generic;
using System;
using Veldrid.Sdl2;
using Veldrid;
//using KeyEvent = Veldrid.KeyEvent;
//using MouseEvent = Veldrid.MouseEvent;

namespace AptumEngine.Core
{
    public class Application
    {
        //List<Window> m_Windows = new List<Window>();

        LayerStack m_LayerStack = new LayerStack();

        private static Application _i;
        public Application()
        {
            if (_i is null)
            {
                _i = this;
            }
            else
            {
                throw new Exception("Application should not be Instantiated Twice");
            }
            ShaderLoader.ShaderPath = "Shaders";
        }

        public static Application Instance => _i;

        //public Application AddWindow(Window window)
        //{
        //    m_Windows.Add(window);
        //    return this;
        //}
        public Application AddLayer(IApplicationLayer layer)
        {
            m_LayerStack.Push(layer);
            layer.OnAttach();
            return this;
        }
        public Application SetupEvents(AWindow window)
        {
            window.SetupEvents(this);
            return this;
            #region OLD
            /*
            //Keyboard
            window.SDLWindow.KeyDown += (key) => HandleEvent(new KeyDownEvent(window, key.Key, key.Modifiers));
            window.SDLWindow.KeyUp += (key) => HandleEvent(new KeyUpEvent(window, key.Key, key.Modifiers));

            //Mouse
            #region OLD_LOG
            //window.SDLWindow.MouseDown += (mouse) => Console.WriteLine($"MOUSE DOWN: {mouse.MouseButton}");
            //window.SDLWindow.MouseEntered += () => Console.WriteLine($"MOUSE ENTER");
            //window.SDLWindow.MouseLeft += () => Console.WriteLine($"MOUSE LEFT");
            //window.SDLWindow.MouseMove += (MouseMoveEventArgs args) => Console.WriteLine($"MOUSE MOVE: {args.MousePosition}");
            //window.SDLWindow.MouseUp += (args) => Console.WriteLine($"MOUSE UP: {args.MouseButton}");
            //window.SDLWindow.MouseWheel += (MouseWheelEventArgs args) => Console.WriteLine($"MOUSE MOVE: {args.WheelDelta}");
            //window.SDLWindow.DragDrop += (DragDropEvent drag) => Console.WriteLine("DRAG DROP");
            #endregion
            window.SDLWindow.MouseDown += (mouse) =>
                    HandleEvent(
                        new MouseButtonDownEvent(window, default, mouse.MouseButton, window.SDLWindow.M)
                            );
            window.SDLWindow.MouseEntered += () => Console.WriteLine($"MOUSE ENTER");
            window.SDLWindow.MouseLeft += () => Console.WriteLine($"MOUSE LEFT");
            window.SDLWindow.MouseMove += (MouseMoveEventArgs args) => Console.WriteLine($"MOUSE MOVE: {args.MousePosition}");
            window.SDLWindow.MouseUp += (args) => Console.WriteLine($"MOUSE UP: {args.MouseButton}");
            window.SDLWindow.MouseWheel += (MouseWheelEventArgs args) => Console.WriteLine($"MOUSE MOVE: {args.WheelDelta}");
            window.SDLWindow.DragDrop += (DragDropEvent drag) => Console.WriteLine("DRAG DROP");

            //Window
            #region OLD_LOG
            //window.SDLWindow.Moved += (Point p) => Console.WriteLine($"Window Moved: {p}");
            //window.SDLWindow.Resized += () => Console.WriteLine($"Window Resized");
            //window.SDLWindow.Shown += () => Console.WriteLine($"Window Shown");
            //window.SDLWindow.Closed += () => Console.WriteLine($"Window Closed");
            //window.SDLWindow.Closing += () => Console.WriteLine($"Window Closing");
            //window.SDLWindow.FocusGained += () => Console.WriteLine($"Window Focus Gained");
            //window.SDLWindow.FocusLost += () => Console.WriteLine($"Window Focus Lost");
            //window.SDLWindow.Hidden += () => Console.WriteLine($"Window Hidden");
            //window.SDLWindow.Exposed += () => Console.WriteLine($"Window Exposed");
            #endregion
            window.SDLWindow.Moved += (Point p) => HandleEvent(new WindowMovedEvent(window, new Vector2(p.X, p.Y)));
            window.SDLWindow.Resized += () => HandleEvent(new WindowResizedEvent(window));
            window.SDLWindow.Shown += () => HandleEvent(new WindowShownEvent(window));
            window.SDLWindow.Closed += () => HandleEvent(new WindowClosedEvent(window));
            window.SDLWindow.Closing += () => HandleEvent(new WindowClosingEvent(window));
            window.SDLWindow.FocusGained += () => HandleEvent(new WindowFocusGained(window));
            window.SDLWindow.FocusLost += () => HandleEvent(new WindowFocusLost(window));
            window.SDLWindow.Hidden += () => HandleEvent(new WindowHiddenEvent(window));
            window.SDLWindow.Exposed += () => HandleEvent(new WindowExposed(window));

            return this; //*/
            #endregion
        }
        public void Run()
        {
            while (m_LayerStack.Count > 0)
            {
                for (int l = 0; l < m_LayerStack.Count; l++)
                {
                    m_LayerStack[l].OnUpdate();
                }
            }
        }

        public void HandleEvent(Event e)
        {
            m_LayerStack.Dispatch(e);
        }
    }
}
