using System;
using System.Diagnostics;

namespace SeweralIdeas.Pooling
{
    
    public struct StackAlloc<T> : IDisposable where T : class
    {
        private readonly StackPool<T> m_pool;

        public StackAlloc(StackPool<T> pool, out T variable)
        {
            m_pool = pool;
            Obj = m_pool.Take();
            variable = Obj;
        }

        public void Dispose()
        {
            Debug.Assert(Obj != null);
            m_pool.Return(Obj);
            Obj = default;
        }

        public T? Obj
        {
            get; private set;
        }
    }
}