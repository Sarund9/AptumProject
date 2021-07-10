using Veldrid;

namespace AptumEngine.Core.OLD
{
    abstract class MouseEvent : Event
    {
        protected MouseEvent(AWindow window) : base(window)
        {
        }

        public override Key KeyCode => default;

        public override EventCategoryFlags Category => EventCategoryFlags.Mouse;
    }

    abstract class MouseButtonEvent : MouseEvent
    {

        protected MouseButtonEvent(AWindow window, ModifierKeys mods) : base(window)
        {
            Modifiers = mods;
        }

        public override ModifierKeys Modifiers { get; }
    }

    class MouseDownEvent
    {

    }
    class MouseUp
    {

    }
    class MouseMoved
    {

    }
    class MouseScroll
    {

    }

    abstract class MouseWindowEvent : Event
    {
        protected MouseWindowEvent(AWindow window) : base(window)
        {
        }

        public override Key KeyCode => default;

    }
    class MouseEnterWindow
    {

    }


}

namespace Experiment
{
    class Sample
    {
        void Handle(E e)
        {

            if (e.Capture<WindowE>(out var win))
            {
                
            }
        }
    }


    abstract class E
    {
        public bool Capture<T>(out T e) where T : E
        {
            if (this is T)
            {
                e = (T)this;
                return true;
            }
            else
            {
                e = default;
                return false;
            }
        }

    }

    class WindowE : E
    {

    }
}