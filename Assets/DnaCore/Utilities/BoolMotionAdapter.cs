using DnaCore.Utilities;
using LitMotion;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(MotionUpdateJob<bool, NoOptions, BoolMotionAdapter>))]

namespace DnaCore.Utilities
{
    public readonly struct BoolMotionAdapter : IMotionAdapter<bool, NoOptions>
    {
        public bool Evaluate(
            ref bool startValue, ref bool endValue, ref NoOptions options, in MotionEvaluationContext context)
        {
            if (context.Time <= 0f) return false;
            
            if (startValue == endValue) return endValue;
            
            startValue = endValue;
            return !endValue;
        }
    }
}
