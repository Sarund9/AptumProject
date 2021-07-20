using Veldrid;

namespace AptumEngine.Core
{
    public abstract class KeyboardEvent : Event
    {
        protected KeyboardEvent(AWindow window, Key key, ModifierKeys mods) : base(window)
        {
            Key = key;
            Modifiers = mods;
        }
        public override EventCategoryFlags CategoryFlags => EventCategoryFlags.Keyboard;

        public Key Key { get; }
        public ModifierKeys Modifiers { get; }

        protected override string EventInfo()
        {
            return $"[{Key}]--[{Modifiers}]";
        }
    }

    public class KeyDownEvent : KeyboardEvent
    {
        public KeyDownEvent(AWindow window, Key key, ModifierKeys mods) : base(window, key, mods)
        {}
    }
    public class KeyUpEvent : KeyboardEvent
    {
        public KeyUpEvent(AWindow window, Key key, ModifierKeys mods) : base(window, key, mods)
        {}
    }
    public class KeyHeldEvent : KeyboardEvent
    {
        public KeyHeldEvent(AWindow window, Key key, ModifierKeys mods) : base(window, key, mods)
        {}
    }
}
