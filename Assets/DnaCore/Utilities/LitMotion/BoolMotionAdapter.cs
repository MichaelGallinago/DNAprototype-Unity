using DnaCore.Utilities.LitMotion;
using LitMotion;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(MotionUpdateJob<bool, NoOptions, ReactiveMotionAdapter>))]

namespace DnaCore.Utilities.LitMotion
{
    public readonly struct ReactiveMotionAdapter : IMotionAdapter<bool, NoOptions>
    {
        public bool Evaluate(
            ref bool startValue, ref bool endValue, ref NoOptions options, in MotionEvaluationContext context)
        {
            if (context.Progress < 1f) return startValue;
            if (startValue == endValue) return endValue;
            
            bool result = endValue;
            endValue = startValue;
            return result;
        }
    }
}
