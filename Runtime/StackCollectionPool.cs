using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeweralIdeas.Pooling
{
    public class StackCollectionPool<T>: StackPool<Stack<T>, StackCollectionPool<T>>
    {
        protected override Stack<T> Alloc() => new Stack<T>();

        protected override void Finalize(Stack<T> obj) => obj.Clear();

        protected override void Prepare(Stack<T> obj) => obj.Clear();
        
    }
}
