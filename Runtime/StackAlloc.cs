using System;

namespace SeweralIdeas.Pooling
{
    
    public struct StackAlloc<T> : IDisposable where T : class
    {
        private static StackPool<T> s_pool = StackPoolLibrary.GetInstance().FindPool<T>();

        public StackAlloc(out T variable)
        {
            obj = s_pool.Take();
            variable = obj;
        }

        public void Dispose()
        {
            s_pool.Return(obj);
            obj = null;
        }

        public T obj
        {
            get; private set;
        }
    }

    public static class StackAlloc
    {
        public static StackAlloc<T> Get<T>(out T variable) where T : class
        {
            return new StackAlloc<T>(out variable);
        }
    }

}