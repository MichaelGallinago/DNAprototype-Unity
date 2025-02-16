using System.Runtime.CompilerServices;
using Unity.Entities;

namespace Utilities
{
    public readonly ref struct BakerQuery
    {
        private readonly IBaker _baker;
        private readonly Entity _entity;
        
        public BakerQuery(IBaker baker, Entity entity)
        {
            _entity = entity;
            _baker = baker;
        }
        
        public BakerQuery(IBaker baker, TransformUsageFlags flags)
        {
            _entity = baker.GetEntity(flags);
            _baker = baker;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddComponent<T>(in T component) where T : unmanaged, IComponentData =>
            _baker.AddComponent(_entity, component);
        
        public BakerQuery AddComponents<T1>(
            in T1 component1) 
            where T1 : unmanaged, IComponentData
        {
            AddComponent(component1);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2>(
            in T1 component1, 
            in T2 component2) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
        {
            AddComponent(component1);
            AddComponent(component2);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3>(
            in T1 component1, 
            in T2 component2,
            in T3 component3) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
        {
            AddComponent(component1);
            AddComponent(component2);
            AddComponent(component3);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4>(
            in T1 component1, 
            in T2 component2,
            in T3 component3, 
            in T4 component4) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
            _baker.AddComponent(_entity, component3);
            _baker.AddComponent(_entity, component4);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5>(
            in T1 component1, 
            in T2 component2,
            in T3 component3, 
            in T4 component4,
            in T5 component5) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
            _baker.AddComponent(_entity, component3);
            _baker.AddComponent(_entity, component4);
            _baker.AddComponent(_entity, component5);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6>(
            in T1 component1, 
            in T2 component2,
            in T3 component3, 
            in T4 component4,
            in T5 component5,
            in T6 component6) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
            _baker.AddComponent(_entity, component3);
            _baker.AddComponent(_entity, component4);
            _baker.AddComponent(_entity, component5);
            _baker.AddComponent(_entity, component6);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7>(
            in T1 component1, 
            in T2 component2,
            in T3 component3, 
            in T4 component4,
            in T5 component5,
            in T6 component6,
            in T7 component7) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
            where T7 : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
            _baker.AddComponent(_entity, component3);
            _baker.AddComponent(_entity, component4);
            _baker.AddComponent(_entity, component5);
            _baker.AddComponent(_entity, component6);
            _baker.AddComponent(_entity, component7);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8>(
            in T1 component1, 
            in T2 component2,
            in T3 component3, 
            in T4 component4,
            in T5 component5,
            in T6 component6,
            in T7 component7,
            in T8 component8) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
            where T7 : unmanaged, IComponentData
            where T8 : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
            _baker.AddComponent(_entity, component3);
            _baker.AddComponent(_entity, component4);
            _baker.AddComponent(_entity, component5);
            _baker.AddComponent(_entity, component6);
            _baker.AddComponent(_entity, component7);
            _baker.AddComponent(_entity, component8);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            in T1 component1, 
            in T2 component2,
            in T3 component3, 
            in T4 component4,
            in T5 component5,
            in T6 component6,
            in T7 component7,
            in T8 component8,
            in T9 component9) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
            where T7 : unmanaged, IComponentData
            where T8 : unmanaged, IComponentData
            where T9 : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
            _baker.AddComponent(_entity, component3);
            _baker.AddComponent(_entity, component4);
            _baker.AddComponent(_entity, component5);
            _baker.AddComponent(_entity, component6);
            _baker.AddComponent(_entity, component7);
            _baker.AddComponent(_entity, component8);
            _baker.AddComponent(_entity, component9);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            in T1 component1, 
            in T2 component2,
            in T3 component3, 
            in T4 component4,
            in T5 component5,
            in T6 component6,
            in T7 component7,
            in T8 component8,
            in T9 component9,
            in T10 component10) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
            where T7 : unmanaged, IComponentData
            where T8 : unmanaged, IComponentData
            where T9 : unmanaged, IComponentData
            where T10 : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
            _baker.AddComponent(_entity, component3);
            _baker.AddComponent(_entity, component4);
            _baker.AddComponent(_entity, component5);
            _baker.AddComponent(_entity, component6);
            _baker.AddComponent(_entity, component7);
            _baker.AddComponent(_entity, component8);
            _baker.AddComponent(_entity, component9);
            _baker.AddComponent(_entity, component10);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddDisabledComponent<T>(in T component) where T : unmanaged, IComponentData, IEnableableComponent
        {
            AddComponent(component);
            _baker.SetComponentEnabled<T>(_entity, false);
        }
        
        public BakerQuery AddDisabledComponents<T1>(
            in T1 component) 
            where T1 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent(component);
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2>(
            in T1 component1, 
            in T2 component2) 
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent(component1);
            AddDisabledComponent(component2);
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3>(
            in T1 component1, 
            in T2 component2,
            in T3 component3) 
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent(component1);
            AddDisabledComponent(component2);
            AddDisabledComponent(component3);
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3, T4>(
            in T1 component1, 
            in T2 component2,
            in T3 component3, 
            in T4 component4) 
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
            where T4 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent(component1);
            AddDisabledComponent(component2);
            AddDisabledComponent(component3);
            AddDisabledComponent(component4);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddComponentObject<T>(in T component) where T : class, IComponentData
        {
            _baker.AddComponentObject(_entity, component);
        }
        
        public BakerQuery AddComponentObjects<T1>(
            in T1 component1) 
            where T1 : class, IComponentData
        {
            AddComponentObject(component1);
            return this;
        }
        
        public BakerQuery AddComponentObjects<T1, T2>(
            in T1 component1,
            in T2 component2) 
            where T1 : class, IComponentData
            where T2 : class, IComponentData
        {
            AddComponentObject(component1);
            AddComponentObject(component2);
            return this;
        }
        
        public BakerQuery AddComponentObjects<T1, T2, T3>(
            in T1 component1,
            in T2 component2,
            in T3 component3) 
            where T1 : class, IComponentData
            where T2 : class, IComponentData
            where T3 : class, IComponentData
        {
            AddComponentObject(component1);
            AddComponentObject(component2);
            AddComponentObject(component3);
            return this;
        }
        
        public BakerQuery AddComponentObjects<T1, T2, T3, T4>(
            in T1 component1,
            in T2 component2,
            in T3 component3,
            in T4 component4) 
            where T1 : class, IComponentData
            where T2 : class, IComponentData
            where T3 : class, IComponentData
            where T4 : class, IComponentData
        {
            AddComponentObject(component1);
            AddComponentObject(component2);
            AddComponentObject(component3);
            AddComponentObject(component4);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddDisabledComponentObject<T>(in T component) where T : class, IComponentData, IEnableableComponent
        {
            AddComponentObject(component);
            _baker.AddComponentObject(_entity, component);
        }
        
        public BakerQuery AddDisabledComponentObjects<T1>(
            in T1 component1) 
            where T1 : class, IComponentData, IEnableableComponent
        {
            AddDisabledComponentObject(component1);
            return this;
        }
        
        public BakerQuery AddDisabledComponentObjects<T1, T2>(
            in T1 component1,
            in T2 component2) 
            where T1 : class, IComponentData, IEnableableComponent
            where T2 : class, IComponentData, IEnableableComponent
        {
            AddDisabledComponentObject(component1);
            AddDisabledComponentObject(component2);
            return this;
        }
        
        public BakerQuery AddDisabledComponentObjects<T1, T2, T3>(
            in T1 component1,
            in T2 component2,
            in T3 component3) 
            where T1 : class, IComponentData, IEnableableComponent
            where T2 : class, IComponentData, IEnableableComponent
            where T3 : class, IComponentData, IEnableableComponent
        {
            AddDisabledComponentObject(component1);
            AddDisabledComponentObject(component2);
            AddDisabledComponentObject(component3);
            return this;
        }
        
        public BakerQuery AddDisabledComponentObjects<T1, T2, T3, T4>(
            in T1 component1,
            in T2 component2,
            in T3 component3,
            in T4 component4) 
            where T1 : class, IComponentData, IEnableableComponent
            where T2 : class, IComponentData, IEnableableComponent
            where T3 : class, IComponentData, IEnableableComponent
            where T4 : class, IComponentData, IEnableableComponent
        {
            AddDisabledComponentObject(component1);
            AddDisabledComponentObject(component2);
            AddDisabledComponentObject(component3);
            AddDisabledComponentObject(component4);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddComponent<T>() where T : unmanaged, IComponentData => _baker.AddComponent<T>(_entity);
        
        public BakerQuery AddComponents<T1>() 
            where T1 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            AddComponent<T3>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            AddComponent<T3>();
            AddComponent<T4>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            AddComponent<T3>();
            AddComponent<T4>();
            AddComponent<T5>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            AddComponent<T3>();
            AddComponent<T4>();
            AddComponent<T5>();
            AddComponent<T6>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
            where T7 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            AddComponent<T3>();
            AddComponent<T4>();
            AddComponent<T5>();
            AddComponent<T6>();
            AddComponent<T7>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
            where T7 : unmanaged, IComponentData
            where T8 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            AddComponent<T3>();
            AddComponent<T4>();
            AddComponent<T5>();
            AddComponent<T6>();
            AddComponent<T7>();
            AddComponent<T8>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
            where T7 : unmanaged, IComponentData
            where T8 : unmanaged, IComponentData
            where T9 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            AddComponent<T3>();
            AddComponent<T4>();
            AddComponent<T5>();
            AddComponent<T6>();
            AddComponent<T7>();
            AddComponent<T8>();
            AddComponent<T9>();
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
            where T1 : unmanaged, IComponentData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
            where T4 : unmanaged, IComponentData
            where T5 : unmanaged, IComponentData
            where T6 : unmanaged, IComponentData
            where T7 : unmanaged, IComponentData
            where T8 : unmanaged, IComponentData
            where T9 : unmanaged, IComponentData
            where T10 : unmanaged, IComponentData
        {
            AddComponent<T1>();
            AddComponent<T2>();
            AddComponent<T3>();
            AddComponent<T4>();
            AddComponent<T5>();
            AddComponent<T6>();
            AddComponent<T7>();
            AddComponent<T8>();
            AddComponent<T9>();
            AddComponent<T10>();
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddDisabledComponent<T>() where T : unmanaged, IComponentData, IEnableableComponent
        {
            AddComponent<T>();
            _baker.SetComponentEnabled<T>(_entity, false);
        }

        public BakerQuery AddDisabledComponents<T1>() 
            where T1 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            AddDisabledComponent<T3>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3, T4>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
            where T4 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            AddDisabledComponent<T3>();
            AddDisabledComponent<T4>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3, T4, T5>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
            where T4 : unmanaged, IComponentData, IEnableableComponent
            where T5 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            AddDisabledComponent<T3>();
            AddDisabledComponent<T4>();
            AddDisabledComponent<T5>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
            where T4 : unmanaged, IComponentData, IEnableableComponent
            where T5 : unmanaged, IComponentData, IEnableableComponent
            where T6 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            AddDisabledComponent<T3>();
            AddDisabledComponent<T4>();
            AddDisabledComponent<T5>();
            AddDisabledComponent<T6>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3, T4, T5, T6, T7>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
            where T4 : unmanaged, IComponentData, IEnableableComponent
            where T5 : unmanaged, IComponentData, IEnableableComponent
            where T6 : unmanaged, IComponentData, IEnableableComponent
            where T7 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            AddDisabledComponent<T3>();
            AddDisabledComponent<T4>();
            AddDisabledComponent<T5>();
            AddDisabledComponent<T6>();
            AddDisabledComponent<T7>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3, T4, T5, T6, T7, T8>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
            where T4 : unmanaged, IComponentData, IEnableableComponent
            where T5 : unmanaged, IComponentData, IEnableableComponent
            where T6 : unmanaged, IComponentData, IEnableableComponent
            where T7 : unmanaged, IComponentData, IEnableableComponent
            where T8 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            AddDisabledComponent<T3>();
            AddDisabledComponent<T4>();
            AddDisabledComponent<T5>();
            AddDisabledComponent<T6>();
            AddDisabledComponent<T7>();
            AddDisabledComponent<T8>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
            where T4 : unmanaged, IComponentData, IEnableableComponent
            where T5 : unmanaged, IComponentData, IEnableableComponent
            where T6 : unmanaged, IComponentData, IEnableableComponent
            where T7 : unmanaged, IComponentData, IEnableableComponent
            where T8 : unmanaged, IComponentData, IEnableableComponent
            where T9 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            AddDisabledComponent<T3>();
            AddDisabledComponent<T4>();
            AddDisabledComponent<T5>();
            AddDisabledComponent<T6>();
            AddDisabledComponent<T7>();
            AddDisabledComponent<T8>();
            AddDisabledComponent<T9>();
            return this;
        }
        
        public BakerQuery AddDisabledComponents<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
            where T1 : unmanaged, IComponentData, IEnableableComponent
            where T2 : unmanaged, IComponentData, IEnableableComponent
            where T3 : unmanaged, IComponentData, IEnableableComponent
            where T4 : unmanaged, IComponentData, IEnableableComponent
            where T5 : unmanaged, IComponentData, IEnableableComponent
            where T6 : unmanaged, IComponentData, IEnableableComponent
            where T7 : unmanaged, IComponentData, IEnableableComponent
            where T8 : unmanaged, IComponentData, IEnableableComponent
            where T9 : unmanaged, IComponentData, IEnableableComponent
            where T10 : unmanaged, IComponentData, IEnableableComponent
        {
            AddDisabledComponent<T1>();
            AddDisabledComponent<T2>();
            AddDisabledComponent<T3>();
            AddDisabledComponent<T4>();
            AddDisabledComponent<T5>();
            AddDisabledComponent<T6>();
            AddDisabledComponent<T7>();
            AddDisabledComponent<T8>();
            AddDisabledComponent<T9>();
            AddDisabledComponent<T10>();
            return this;
        }
    }
}
