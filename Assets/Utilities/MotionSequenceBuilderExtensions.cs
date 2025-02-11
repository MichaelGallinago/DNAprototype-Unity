using System;
using LitMotion;

namespace Utilities
{
    public static class MotionSequenceBuilderExtensions
    {
        public static MotionSequenceBuilder JoinAction(this MotionSequenceBuilder builder, Action action) =>
            builder.Join(LMotion.Create(0f, 0f, 0f).WithOnComplete(action).RunWithoutBinding());
    }
}
