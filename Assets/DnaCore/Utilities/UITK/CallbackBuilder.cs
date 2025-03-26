using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace DnaCore.Utilities
{
    public readonly ref struct CallbackBuilder<TUserArgsType>
    {
        private readonly TUserArgsType _args;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CallbackBuilder(TUserArgsType args) => _args = args;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueRegistrar<TUserArgsType, T> SetValueTarget<T>(INotifyValueChanged<T> control) => 
            new(control, this);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Registrar<TUserArgsType> SetTarget(CallbackEventHandler eventHandler) => 
            new (eventHandler, this);
        
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CallbackBuilder<TUserArgsType> RegisterValueChanged<T>(
            INotifyValueChanged<T> control, 
            EventCallback<ChangeEvent<T>> callback, 
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
        {
            if (control is not CallbackEventHandler handler)
            {
                Debug.LogError($"{nameof(control)} is not of type {nameof(CallbackEventHandler)}");
                return this;
            }
            handler.RegisterCallback(callback, useTrickleDown);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CallbackBuilder<TUserArgsType> RegisterValueChanged<T>(
            INotifyValueChanged<T> control, 
            EventCallback<ChangeEvent<T>, TUserArgsType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
        {
            if (control is not CallbackEventHandler handler)
            {
                Debug.LogError($"{nameof(control)} is not of type {nameof(CallbackEventHandler)}");
                return this;
            }
            handler.RegisterCallback(callback, _args, useTrickleDown);
            return this;
        }
    }

    public readonly ref struct ValueRegistrar<TUserArgsType, T>
    {
        public readonly Registrar<TUserArgsType> Registrar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueRegistrar(INotifyValueChanged<T> control, CallbackBuilder<TUserArgsType> builder)
        {
            if (control is not CallbackEventHandler eventHandler)
            {
                Debug.LogError($"{nameof(control)} is not of type {nameof(CallbackEventHandler)}");
                Registrar = new Registrar<TUserArgsType>(null, builder);
                return;
            }
            Registrar = new Registrar<TUserArgsType>(eventHandler, builder);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueRegistrar<TUserArgsType, T> Register<TEventType>(
            EventCallback<TEventType, TUserArgsType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
            where TEventType : EventBase<TEventType>, new()
        {
            Registrar.Register(callback, useTrickleDown);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueRegistrar<TUserArgsType, T> Register<TEventType>(
            EventCallback<TEventType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
            where TEventType : EventBase<TEventType>, new()
        {
            Registrar.Register(callback, useTrickleDown);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueRegistrar<TUserArgsType, T> RegisterValueChanged(
            EventCallback<ChangeEvent<T>> callback, 
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
        {
            Registrar.Register(callback, useTrickleDown);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueRegistrar<TUserArgsType, T> RegisterValueChanged(
            EventCallback<ChangeEvent<T>, TUserArgsType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
        {
            Registrar.Register(callback, useTrickleDown);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueRegistrar<TUserArgsType, TNew> SetValueTarget<TNew>(INotifyValueChanged<TNew> control) => 
            Registrar.SetValueTarget(control);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Registrar<TUserArgsType> SetTarget(CallbackEventHandler eventHandler) => 
            Registrar.SetTarget(eventHandler);
    }
    
    public readonly ref struct Registrar<TUserArgsType>
    {
        public readonly CallbackBuilder<TUserArgsType> Builder;
        public readonly CallbackEventHandler EventHandler;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Registrar(CallbackEventHandler eventHandler, CallbackBuilder<TUserArgsType> builder)
        {
            Builder = builder;
            EventHandler = eventHandler;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Registrar<TUserArgsType> Register<TEventType>(
            EventCallback<TEventType, TUserArgsType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
            where TEventType : EventBase<TEventType>, new()
        {
            Builder.Register(EventHandler, callback, useTrickleDown);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Registrar<TUserArgsType> Register<TEventType>(
            EventCallback<TEventType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
            where TEventType : EventBase<TEventType>, new()
        {
            Builder.Register(EventHandler, callback, useTrickleDown);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueRegistrar<TUserArgsType, T> SetValueTarget<T>(INotifyValueChanged<T> control) => 
            new(control, Builder);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Registrar<TUserArgsType> SetTarget(CallbackEventHandler eventHandler) => 
            new (eventHandler, Builder);
    }
}
