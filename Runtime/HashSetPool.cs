
namespace SeweralIdeas.Pooling
{
    public class HashSetPool<T> : StackPool<System.Collections.Generic.HashSet<T>>
    {
        protected override System.Collections.Generic.HashSet<T> Alloc()
        {
            return new System.Collections.Generic.HashSet<T>();
        }

        protected override void Finalize(System.Collections.Generic.HashSet<T> obj)
        {
            obj.Clear();
        }

        protected override void Prepare(System.Collections.Generic.HashSet<T> obj)
        {
            obj.Clear();
        }
    }
    public class DictPool<TKey, TVal> : StackPool<System.Collections.Generic.Dictionary<TKey, TVal>>
    {
        protected override System.Collections.Generic.Dictionary<TKey, TVal> Alloc()
        {
            return new System.Collections.Generic.Dictionary<TKey, TVal>();
        }

        protected override void Finalize(System.Collections.Generic.Dictionary<TKey, TVal> obj)
        {
            obj.Clear();
        }

        protected override void Prepare(System.Collections.Generic.Dictionary<TKey, TVal> obj)
        {
            obj.Clear();
        }
    }

}