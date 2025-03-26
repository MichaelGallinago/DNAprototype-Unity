using System;
using System.Runtime.CompilerServices;
using LitMotion;

namespace DnaCore.Utilities.LitMotion
{
    public static class MotionSequenceBuilderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionSequenceBuilder AppendAction(this MotionSequenceBuilder builder, Action action) => 
            builder.Append(CreateAction(action));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionHandle RunAfterAction(this MotionSequenceBuilder builder) =>
            builder.AppendInterval(0.01f).Run();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionSequenceBuilder JoinAction(this MotionSequenceBuilder builder, Action action) => 
            builder.Join(CreateAction(action));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionSequenceBuilder JoinAction<TUserArgs>(
            this MotionSequenceBuilder builder, TUserArgs args, Action<TUserArgs> action) 
                where TUserArgs : class => 
                    builder.Join(CreateAction(action, args));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionSequenceBuilder AppendAction<TUserArgs>(
            this MotionSequenceBuilder builder, TUserArgs args, Action<TUserArgs> action) 
            where TUserArgs : class => 
                builder.Append(CreateAction(action, args));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionSequenceBuilder JoinAction<TUserArgs1, TUserArgs2>(
            this MotionSequenceBuilder builder, 
            TUserArgs1 args1, TUserArgs2 args2, 
            Action<TUserArgs1, TUserArgs2> action) 
                where TUserArgs1 : class 
                where TUserArgs2 : class => 
                    builder.Join(CreateAction(action, args1, args2));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionSequenceBuilder AppendAction<TUserArgs1, TUserArgs2>(
            this MotionSequenceBuilder builder, 
            TUserArgs1 args1, TUserArgs2 args2, 
            Action<TUserArgs1, TUserArgs2> action) 
                where TUserArgs1 : class 
                where TUserArgs2 : class => 
                    builder.Join(CreateAction(action, args1, args2));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionSequenceBuilder AppendAndInterval(
            this MotionSequenceBuilder builder, MotionHandle handle, float interval) =>
                builder.AppendInterval(interval).Join(handle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MotionHandle CreateAction(Action action) =>
            LMotion.Create(0f, 1f, 0f).WithOnComplete(action).RunWithoutBinding();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MotionHandle CreateAction<TUserArgs>(Action<TUserArgs> action, TUserArgs args) 
            where TUserArgs : class => 
                LMotion.Create<bool, NoOptions, ReactiveMotionAdapter>(false, true, 0f)
                    .Bind(args, action, static (value, innerArgs, innerAction) => 
                    {
                        if (value) innerAction(innerArgs);
                    });
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MotionHandle CreateAction<TUserArgs1, TUserArgs2>(
            Action<TUserArgs1, TUserArgs2> action, TUserArgs1 args1, TUserArgs2 args2) 
                where TUserArgs1 : class 
                where TUserArgs2 : class => 
                    LMotion.Create<bool, NoOptions, ReactiveMotionAdapter>(false, true, 0f)
                        .Bind(args1, args2, action, static (value, innerArgs1,  innerArgs2, innerAction) => 
                        {
                            if (value) innerAction(innerArgs1, innerArgs2);
                        });
    }
}
