
#if UNITY_5_3_OR_NEWER
#define UNITY
using UnityEngine;
#endif

namespace SeweralIdeas.Pooling
{
    public class ListPool<T> : StackPool<System.Collections.Generic.List<T>>
    {
        protected override System.Collections.Generic.List<T> Alloc()
        {
            return new System.Collections.Generic.List<T>();
        }

        protected override void Finalize(System.Collections.Generic.List<T> obj)
        {
            obj.Clear();
        }

        protected override void Prepare(System.Collections.Generic.List<T> obj)
        {
            obj.Clear();
        }
    }

    public class SortedListPool<TKey, TVal> : StackPool<System.Collections.Generic.SortedList<TKey, TVal>>
    {
        protected override System.Collections.Generic.SortedList<TKey, TVal> Alloc()
        {
            return new System.Collections.Generic.SortedList<TKey, TVal>();
        }

        protected override void Finalize(System.Collections.Generic.SortedList<TKey, TVal> obj)
        {
            obj.Clear();
        }

        protected override void Prepare(System.Collections.Generic.SortedList<TKey, TVal> obj)
        {
            obj.Clear();
        }
    }

#if UNITY
    public class GameObjectPool : StackPool<UnityEngine.GameObject>
    {
        protected override UnityEngine.GameObject Alloc()
        {
            return new UnityEngine.GameObject();
        }

        protected override void Finalize(UnityEngine.GameObject obj)
        {
            obj.SetActive(false);
        }

        protected override void Prepare(UnityEngine.GameObject obj)
        {
            obj.name = "GameObject";
            obj.SetActive(true);
        }
    }
#endif
}