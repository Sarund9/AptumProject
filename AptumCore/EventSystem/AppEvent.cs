namespace AptumEngine.Core
{
    public abstract class AppEvent : Event
    {
        protected AppEvent(AWindow window) : base(window)
        {
        }
        public override EventCategoryFlags CategoryFlags => EventCategoryFlags.Application;
    }

    public class WindowMovedEvent : AppEvent
    {
        public WindowMovedEvent(AWindow window, Vector2 newPosition) : base(window)
        {
            NewPos = newPosition;
        }
        //New position of the Window
        public Vector2 NewPos { get; }
    }

    public class WindowResizedEvent : AppEvent
    {
        public WindowResizedEvent(AWindow window) : base(window)
        {}
    }

    public class WindowShownEvent : AppEvent
    {
        public WindowShownEvent(AWindow window) : base(window)
        {}
    }
    public class WindowClosedEvent : AppEvent
    {
        public WindowClosedEvent(AWindow window) : base(window)
        {}
    }
    public class WindowClosingEvent : AppEvent
    {
        public WindowClosingEvent(AWindow window) : base(window)
        { }
    }
    public class WindowFocusGained : AppEvent
    {
        public WindowFocusGained(AWindow window) : base(window)
        {}
    }
    public class WindowFocusLost : AppEvent
    {
        public WindowFocusLost(AWindow window) : base(window)
        {}
    }
    public class WindowHiddenEvent : AppEvent
    {
        public WindowHiddenEvent(AWindow window) : base(window)
        {}
    }
    public class WindowExposed : AppEvent
    {
        public WindowExposed(AWindow window) : base(window)
        {}
    }
}
