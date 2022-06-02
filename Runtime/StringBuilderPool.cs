using System.Text;

namespace SeweralIdeas.Pooling
{
#if UNITY_5_3_OR_NEWER
    [UnityEngine.Scripting.Preserve]
#endif
    public class StringBuilderPool : StackPool<StringBuilder, StringBuilderPool>
    {
        protected override StringBuilder Alloc()
        {
            return new StringBuilder();
        }

        protected override void Finalize(StringBuilder obj)
        {
            obj.Clear();
        }

        protected override void Prepare(StringBuilder obj)
        {
            obj.Clear();
        }
    }
}