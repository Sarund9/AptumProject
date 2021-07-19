using System;
using System.Collections.Generic;
using System.Text;
using Veldrid;

namespace AptumEngine.Core.OLD
{
    public enum EventType
    {
        Error = 0,
        KeyDown, KeyUp,                                   // Keyboard Input
        
        MouseDown, MouseUp,                               // Mouse Input
        MouseMoved, MouseScroll,                          // Mouse Input
        MouseEnterWindow, MouseLeftWindow, MouseDragDrop, // Mouse Application

        WindowMoved, WindowResized, WindowShown,          // Application
        WindowClosed, WindowClosing,
        WindowFocusGained, WindowFocusLost,
        WindowHidden, WindowExposed,
    }

    [Flags]
    public enum EventCategoryFlags : byte
    {
        None = 0b_0000_0000,

        Application = 0b_0000_0001,

        Mouse       = 0b_0000_0010,
        Keyboard    = 0b_0000_0100,
        Input       = 0b_0000_0110,

        All = 0b_1111_1111,
    }

    //[Flags]
    //public enum ModifierFlags : byte
    //{
    //    None = 0,
    //    Cntrl = 1,
    //    Shift = 2,
    //    Alt = 4,
    //    LCntrl = 8,
    //    LShift = 16,
    //    LAlt = 32,
    //}

    public abstract class Event
    {
        protected bool b_handled;
        protected AWindow m_Window;

        protected Event(AWindow window)
        {
            m_Window = window;
        }

        #region ABSTRACT_DATA
        public abstract EventType EventType { get; }
        public abstract EventCategoryFlags Category { get; }
        public abstract ModifierKeys Modifiers { get; }
        public abstract Key KeyCode { get; } //TODO: KeyCodes
        public abstract int Button { get; }
        public abstract Vector2 Delta { get; }
        //public abstract bool CapsLock { get; }
        public abstract char Character { get; }
        public abstract int ClickCount { get; }
        public abstract Vector2 MousePosition { get; }
        public abstract Rang01 Pressure { get; }
        //public abstract Rect WindowLocation { get; }
        #endregion

        public bool Handled => b_handled;
        public AWindow Window => m_Window;
        

        public bool IsFromCategory(EventCategoryFlags category)
        {
            return Category.HasFlag(category);
        }
        public bool IsModifierPressed(ModifierKeys modifiers)
        {
            return Modifiers.HasFlag(modifiers);
        }
    }


}
