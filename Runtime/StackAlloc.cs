using System;

namespace SeweralIdeas.Pooling
{
    
    public struct StackAlloc<T> : IDisposable where T : class
    {
        private StackPool<T> m_pool;

        public StackAlloc(StackPool<T> pool, out T variable)
        {
            m_pool = pool;
            obj = m_pool.Take();
            variable = obj;
        }

        public void Dispose()
        {
            m_pool.Return(obj);
            obj = null;
        }

        public T obj
        {
            get; private set;
        }
    }

    public static class StackAlloc
    {
        [Obsolete("No longer supported. Use the pool type explicitly.", true)]
        public static StackAlloc<T> Get<T>(out T variable) where T : class => throw new NotSupportedException();
    }

}