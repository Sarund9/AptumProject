using System;
using Veldrid;

namespace AptumEngine.Core.OLD
{
    abstract class AppEvent : Event
    {
        protected AppEvent(AWindow window) : base(window)
        {}

        public override EventCategoryFlags Category => EventCategoryFlags.Application;
        public override ModifierKeys Modifiers => ModifierKeys.None;
        public override Key KeyCode => default;
        public override int Button => -1;
        public override char Character => default;
        public override int ClickCount => -1;
        public override Vector2 MousePosition => Vector2.Zero; //TODO Mouse Pos
        public override Rang01 Pressure => 0;
        public override Vector2 Delta => Vector2.Zero;
    }
    class WindowMovedEvent : AppEvent
    {
        Vector2 point;

        public WindowMovedEvent(AWindow window, Vector2 point) : base(window)
        {
            this.point = point;
            Console.WriteLine($"WINDOW MOVED - POINT: {point}");
        }

        public override EventType EventType => EventType.MouseMoved;
    }
    class WindowResizedEvent : AppEvent
    {
        public WindowResizedEvent(AWindow window) : base(window)
        {
            
        }
        public override EventType EventType => EventType.WindowResized;
    }
    class WindowShownEvent : AppEvent
    {
        public WindowShownEvent(AWindow window) : base(window)
        {}

        public override EventType EventType => EventType.WindowShown;

    }

    class WindowClosedEvent : AppEvent
    {
        public WindowClosedEvent(AWindow window) : base(window)
        {}

        public override EventType EventType => EventType.WindowClosed;

    }

    class WindowClosingEvent : AppEvent
    {
        public WindowClosingEvent(AWindow window) : base(window)
        {}
        public override EventType EventType => EventType.WindowClosing;

    }

    class WindowFocusGained : AppEvent
    {
        public WindowFocusGained(AWindow window) : base(window)
        {}

        public override EventType EventType => EventType.WindowFocusGained;

    }
    class WindowFocusLost : AppEvent
    {
        public WindowFocusLost(AWindow window) : base(window)
        {}

        public override EventType EventType => EventType.WindowFocusLost;

    }
    class WindowHidden : AppEvent
    {
        public WindowHidden(AWindow window) : base(window)
        {}

        public override EventType EventType => EventType.WindowHidden;

    }
    class WindowExposed : AppEvent
    {
        public WindowExposed(AWindow window) : base(window)
        {}

        public override EventType EventType => EventType.WindowExposed;

    }
}
