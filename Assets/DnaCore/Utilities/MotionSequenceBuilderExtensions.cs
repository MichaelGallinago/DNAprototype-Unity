using System;
using LitMotion;

namespace DnaCore.Utilities
{
    public static class MotionSequenceBuilderExtensions
    {
        public static MotionSequenceBuilder AppendAction(this MotionSequenceBuilder builder, Action action) => 
            builder.Append(CreateAction(action));
        
        public static MotionSequenceBuilder JoinAction(this MotionSequenceBuilder builder, Action action) => 
            builder.Join(CreateAction(action));
        
        public static MotionSequenceBuilder AppendAndInterval(
            this MotionSequenceBuilder builder, MotionHandle handle, float interval) =>
                builder.AppendInterval(interval).Join(handle);

        private static MotionHandle CreateAction(Action action) =>
            LMotion.Create(0f, 1f, 0f).WithOnComplete(action).RunWithoutBinding();
    }
}
