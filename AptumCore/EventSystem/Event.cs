using System;
using System.Collections.Generic;
using System.Text;

namespace AptumEngine.Core
{
    [Flags]
    public enum EventCategoryFlags : byte
    {
        None = 0b_0000_0000,

        Application = 0b_0000_0001,

        Mouse = 0b_0000_1010,
        Keyboard = 0b_0000_1100,
        Input = 0b_0000_1000,

        All = 0b_1111_1111,
    }

    public abstract class Event
    {
        protected bool b_handled;
        protected AWindow m_Window;

        protected Event(AWindow window)
        {
            m_Window = window;
        }

        public bool Handled => b_handled;
        public AWindow Window => m_Window;
        public abstract EventCategoryFlags CategoryFlags { get; }

        public bool Capture<T>(out T e) where T : Event
        {
            if (this is T captured)
            {
                e = captured;
                return true;
            }
            else
            {
                e = default;
                return false;
            }
        }
        public bool Capture(EventCategoryFlags flags)
        {
            return CategoryFlags.HasFlag(flags);
        }
    }
}
