using System.Collections.Generic;
namespace SeweralIdeas.Pooling
{
    public class HashSetPool<T> : StackPool<HashSet<T>, HashSetPool<T>>
    {
        protected override HashSet<T> Alloc() => new HashSet<T>();
        protected override void Finalize(HashSet<T> obj) => obj.Clear();
        protected override void Prepare(HashSet<T> obj) => obj.Clear();
    }
    public class DictPool<TKey, TVal> : StackPool<Dictionary<TKey, TVal>, DictPool<TKey, TVal>> where TKey : notnull
    {
        protected override Dictionary<TKey, TVal> Alloc() => new Dictionary<TKey, TVal>();
        protected override void Finalize(Dictionary<TKey, TVal> obj) => obj.Clear();
        protected override void Prepare(Dictionary<TKey, TVal> obj) => obj.Clear();
    }

}