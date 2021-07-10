using System.Collections;
using System.Collections.Generic;

namespace AptumEngine.Core
{
    public class LayerStack : IEnumerable<IApplicationLayer>
    {
        List<IApplicationLayer> m_Layers = new List<IApplicationLayer>();

        public IApplicationLayer this[int i]
        {
            get => m_Layers[i];
            set => m_Layers[i] = value;
        }
        public int Count => m_Layers.Count;
        public void Push(IApplicationLayer layer)
        {
            m_Layers.Add(layer);
        }

        public void Pop(IApplicationLayer layer)
        {
            m_Layers.Remove(layer);
        }

        public void Overlay(IApplicationLayer layer)
        {
            m_Layers.Insert(0, layer);
        }

        public IEnumerator<IApplicationLayer> GetEnumerator() =>
            ((IEnumerable<IApplicationLayer>)m_Layers).GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)m_Layers).GetEnumerator();
        
        public void Dispatch(Event e)
        {
            for (int i = m_Layers.Count - 1; i >= 0; i--)
                if (!e.Handled)
                    m_Layers[i].OnEvent(e);
        }
    }
}
