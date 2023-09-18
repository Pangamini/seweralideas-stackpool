using System.Collections.Concurrent;

namespace SeweralIdeas.Pooling
{
    public abstract class StackPoolBase
    {
        public abstract System.Type GetElementType();
        public abstract int pooledCount { get; }

#if DEBUG
        private static HashSet<System.WeakReference> s_allStackPools = new HashSet<System.WeakReference>();

        private System.WeakReference m_weakRef;

        protected StackPoolBase()
        {
            lock (s_allStackPools)
            {
                m_weakRef = new System.WeakReference(this);
                s_allStackPools.Add(m_weakRef);
            }
        }

        public static void VisitAllPools(System.Action<StackPoolBase> visitor)
        {
            lock (s_allStackPools)
            {
                foreach (var weakRef in s_allStackPools)
                {
                    var pool = weakRef.Target as StackPoolBase;
                    if(pool != null)
                        visitor(pool);
                }
            }
        }

        ~StackPoolBase()
        {
            lock (s_allStackPools)
            {
                s_allStackPools.Remove(m_weakRef);
            }
        }
#endif
    }
    
    /*
    General usage stack-like pool. Use to emulate stack allocation of non-value type objects.
    DO NOT keep the reference to popped object after it's been pushed back to the pool.
    */

    public abstract class StackPool<T> : StackPoolBase
    {
        public override int pooledCount => m_bag.Count;

        public override Type GetElementType() => typeof(T);

        private readonly ConcurrentBag<T> m_bag = new ConcurrentBag<T>();
        public T Take()
        {
            if (!m_bag.TryTake(out T? obj))
                obj = Alloc();
            Prepare(obj);
            return obj;
        }
        
        public void Return(T obj)
        {
            Finalize(obj);
            m_bag.Add(obj);
        }

        protected abstract T Alloc();

        /////////////////////////////////////////////////////////////////////
        /// called when object returns to the pool
        /// Clear the object here or in Prepare
        /// Optionally release the object internal lock
        
        protected virtual void Finalize(T obj) { }

        /////////////////////////////////////////////////////////////////////
        /// called before object is returned by Pop()
        /// clear the object here or in Finalize 
        /// Optionally occupy the object's internal lock

        protected virtual void Prepare(T obj) { }
    }
    
    public abstract class StackPool<T, TPool> : StackPool<T> where T : class where TPool : StackPool<T, TPool>, new()
    {          
        public static readonly TPool Instance = new TPool();
        public static StackAlloc<T> Get(out T obj) => new StackAlloc<T>(Instance, out obj);

    }

    /// <summary>
    /// A StackPool that implements Alloc using default constructor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BasicStackPool<T, TPool> : StackPool<T, TPool> where T:class, new() where TPool:BasicStackPool<T, TPool>, new()
    {
        protected sealed override T Alloc() => new T();
    }
}