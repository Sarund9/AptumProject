using Veldrid;
using Veldrid.Sdl2;

namespace AptumEngine.Core.OLD
{
    abstract class KeyboardEvent : Event
    {
        Key e_Keycode;
        ModifierKeys e_Mods;

        protected KeyboardEvent(AWindow window, Key keycode, ModifierKeys mods) : base(window)
        {
            e_Keycode = keycode;
            e_Mods = mods;
        }

        public override Key KeyCode => e_Keycode;
        public override char Character => default; //TODO: Keycode to Char

        public override int Button => -1;
        public override Vector2 Delta => Vector2.Zero;

        public override Vector2 MousePosition => Vector2.Zero; //TODO Mouse Pos
        public override Rang01 Pressure => 0;
        public override int ClickCount => 0;

        public override EventCategoryFlags Category => EventCategoryFlags.Keyboard | EventCategoryFlags.Input;
        public override ModifierKeys Modifiers => e_Mods;
    }

    class KeyDownEvent : KeyboardEvent
    {
        public KeyDownEvent(AWindow window, Key keycode, ModifierKeys mods) : base(window, keycode, mods)
        {}

        public override EventType EventType => EventType.KeyDown;


    }
    class KeyUpEvent : KeyboardEvent
    {
        public KeyUpEvent(AWindow window, Key keycode, ModifierKeys mods) : base(window, keycode, mods)
        {}

        public override EventType EventType => EventType.KeyUp;

    }
}
