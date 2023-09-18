#if UNITY_5_3_OR_NEWER
#define UNITY
#endif

namespace SeweralIdeas.Pooling
{
    public sealed class ListPool<T> : StackPool<List<T>, ListPool<T>>
    {
        protected override List<T> Alloc() => new List<T>();

        protected override void Finalize(List<T> obj) => obj.Clear();

        protected override void Prepare(List<T> obj) => obj.Clear();
        
    }

    public sealed class SortedListPool<TKey, TVal> : StackPool<SortedList<TKey, TVal>, SortedListPool<TKey, TVal>> where TKey : notnull
    {
        protected override SortedList<TKey, TVal> Alloc() => new SortedList<TKey, TVal>();

        protected override void Finalize(SortedList<TKey, TVal> obj) => obj.Clear();

        protected override void Prepare(SortedList<TKey, TVal> obj) => obj.Clear();
    }

#if UNITY
    [UnityEngine.Scripting.Preserve]
    public class GameObjectPool : StackPool<UnityEngine.GameObject, GameObjectPool>
    {
        protected override UnityEngine.GameObject Alloc() => new UnityEngine.GameObject();

        protected override void Finalize(UnityEngine.GameObject obj) => obj.SetActive(false);

        protected override void Prepare(UnityEngine.GameObject obj)
        {
            obj.name = "GameObject";
            obj.SetActive(true);
        }
    }
#endif
}