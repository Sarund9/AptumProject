using System;
using System.Collections.Generic;
using System.Text;

namespace AptumEngine.Core
{
    /// <summary> Disposable Object </summary>
    public abstract class AObject : IDisposable
    {
        private readonly List<IDisposable> m_Disposables = new List<IDisposable>();

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        protected internal bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is currently being disposed.
        /// </summary>
        protected internal bool IsDisposing { get; private set; }


        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposing = true;
                Dispose(true);
                IsDisposed = true;
            }
        }
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
                for (var i = m_Disposables.Count - 1; i >= 0; i--)
                    m_Disposables[i].Dispose();
        }

        protected IDisposable AddDisposable(IDisposable disposable)
        {
            if (disposable is object)
            {
                m_Disposables.Add(disposable);
            }
            return disposable;
        }
    }
}
