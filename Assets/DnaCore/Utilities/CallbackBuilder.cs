using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace DnaCore.Utilities
{
    public readonly ref struct CallbackBuilder<TUserArgsType>
    {
        private readonly TUserArgsType _args;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CallbackBuilder(TUserArgsType args) => _args = args;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CallbackBuilder<TUserArgsType> Register<TEventType>(
            CallbackEventHandler eventHandler,
            EventCallback<TEventType, TUserArgsType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
            where TEventType : EventBase<TEventType>, new()
        {
            eventHandler.RegisterCallback(callback, _args, useTrickleDown);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CallbackBuilder<TUserArgsType> Register<TEventType>(
            CallbackEventHandler eventHandler,
            EventCallback<TEventType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
            where TEventType : EventBase<TEventType>, new()
        {
            eventHandler.RegisterCallback(callback, useTrickleDown);
            return this;
        }
    }
}
