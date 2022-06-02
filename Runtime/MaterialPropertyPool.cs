#if UNITY_5_3_OR_NEWER
using UnityEngine;

namespace SeweralIdeas.Pooling
{
    [UnityEngine.Scripting.Preserve]
    public class MaterialPropertyPool : StackPool<MaterialPropertyBlock, MaterialPropertyPool>
    {
        protected override MaterialPropertyBlock Alloc() => new MaterialPropertyBlock();
        protected override void Finalize(MaterialPropertyBlock obj) => obj.Clear();
        protected override void Prepare(MaterialPropertyBlock obj) => obj.Clear();
    }
}
#endif