using System;
using System.Text;

namespace AptumEngine.Core
{
    public interface IApplicationLayer
    {
        EventCategoryFlags TargetCategoryFlags { get => EventCategoryFlags.All; }
        
        void OnEvent(Event e);
        void OnUpdate();
        void OnAttach();
        void OnDetach();
    }
}
