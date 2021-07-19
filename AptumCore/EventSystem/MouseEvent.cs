using Veldrid;

namespace AptumEngine.Core
{
    public abstract class MouseEvent : Event
    {
        protected MouseEvent(
            AWindow window, ModifierKeys modifiers, Vector2 mousePosition)
            : base(window)
        {
            Modifiers = modifiers;
            MousePosition = mousePosition;
        }
        public override EventCategoryFlags CategoryFlags => EventCategoryFlags.Mouse;

        public ModifierKeys Modifiers { get; }
        public Vector2 MousePosition { get; }
    }

    // Mouse Button
    public abstract class MouseButtonEvent : MouseEvent
    {
        protected MouseButtonEvent(
            AWindow window, ModifierKeys modifiers, MouseButton button, Vector2 mousePosition)
            : base(window, modifiers, mousePosition)
        {
            Button = button;
        }
        public MouseButton Button { get; }
    }
    public class MouseButtonDownEvent : MouseButtonEvent
    {
        public MouseButtonDownEvent(
            AWindow window, ModifierKeys modifiers, MouseButton button, Vector2 mousePosition)
            : base(window, modifiers, button, mousePosition)
        {
        }
    }
    public class MouseButtonUpEvent : MouseButtonEvent
    {
        public MouseButtonUpEvent(
            AWindow window, ModifierKeys modifiers, MouseButton button, Vector2 mousePosition)
            : base(window, modifiers, button, mousePosition)
        {
        }
    }

    #region MOUSE_WINDOW
    // Mouse Window
    public abstract class MouseWindowEvent : MouseEvent
    {
        public MouseWindowEvent(
            AWindow window, ModifierKeys modifiers, Vector2 mousePosition)
            : base(window, modifiers, mousePosition)
        {}
    }
    public class MouseEnteredWindowEvent : MouseWindowEvent
    {
        public MouseEnteredWindowEvent(
            AWindow window, ModifierKeys modifiers, Vector2 mousePosition)
            : base(window, modifiers, mousePosition)
        {}
    }
    public class MouseLeftWindowEvent : MouseWindowEvent
    {
        public MouseLeftWindowEvent(
            AWindow window, ModifierKeys modifiers, Vector2 mousePosition)
            : base(window, modifiers, mousePosition)
        {}
    }
    #endregion

    //Mouse Movement
    public class MouseMovedEvent : MouseEvent
    {
        public MouseMovedEvent(
            AWindow window, ModifierKeys modifiers, Vector2 mousePosition)
            : base(window, modifiers, mousePosition)
        {}
    }
    public class MouseScrollEvent : MouseEvent
    {
        public MouseScrollEvent(
           AWindow window, ModifierKeys modifiers, Vector2 mousePosition, float delta)
           : base(window, modifiers, mousePosition)
        {
            Delta = delta;
        }
        float Delta { get; }
    }
}
