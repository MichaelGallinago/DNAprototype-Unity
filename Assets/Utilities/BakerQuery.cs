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
        
        public BakerQuery AddComponent<T>(in T component) where T : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2>(
            in T1 component1, 
            in T2 component2) 
            where T1 : unmanaged, IComponentData 
            where T2 : unmanaged, IComponentData
        {
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
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
            _baker.AddComponent(_entity, component1);
            _baker.AddComponent(_entity, component2);
            _baker.AddComponent(_entity, component3);
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
        
        public BakerQuery AddComponentObject<T>(in T component) where T : class, IComponentData
        {
            _baker.AddComponentObject(_entity, component);
            return this;
        }
        
        public BakerQuery AddComponentObjects<T1, T2>(
            in T1 component1,
            in T2 component2) 
            where T1 : class, IComponentData
            where T2 : class, IComponentData
        {
            _baker.AddComponentObject(_entity, component1);
            _baker.AddComponentObject(_entity, component2);
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
            _baker.AddComponentObject(_entity, component1);
            _baker.AddComponentObject(_entity, component2);
            _baker.AddComponentObject(_entity, component3);
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
            _baker.AddComponentObject(_entity, component1);
            _baker.AddComponentObject(_entity, component2);
            _baker.AddComponentObject(_entity, component3);
            _baker.AddComponentObject(_entity, component4);
            return this;
        }
        
        public BakerQuery AddComponent<T>()
        {
            _baker.AddComponent<T>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            _baker.AddComponent<T3>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            _baker.AddComponent<T3>(_entity);
            _baker.AddComponent<T4>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            _baker.AddComponent<T3>(_entity);
            _baker.AddComponent<T4>(_entity);
            _baker.AddComponent<T5>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            _baker.AddComponent<T3>(_entity);
            _baker.AddComponent<T4>(_entity);
            _baker.AddComponent<T5>(_entity);
            _baker.AddComponent<T6>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            _baker.AddComponent<T3>(_entity);
            _baker.AddComponent<T4>(_entity);
            _baker.AddComponent<T5>(_entity);
            _baker.AddComponent<T6>(_entity);
            _baker.AddComponent<T7>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            _baker.AddComponent<T3>(_entity);
            _baker.AddComponent<T4>(_entity);
            _baker.AddComponent<T5>(_entity);
            _baker.AddComponent<T6>(_entity);
            _baker.AddComponent<T7>(_entity);
            _baker.AddComponent<T8>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            _baker.AddComponent<T3>(_entity);
            _baker.AddComponent<T4>(_entity);
            _baker.AddComponent<T5>(_entity);
            _baker.AddComponent<T6>(_entity);
            _baker.AddComponent<T7>(_entity);
            _baker.AddComponent<T8>(_entity);
            _baker.AddComponent<T9>(_entity);
            return this;
        }
        
        public BakerQuery AddComponents<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        {
            _baker.AddComponent<T1>(_entity);
            _baker.AddComponent<T2>(_entity);
            _baker.AddComponent<T3>(_entity);
            _baker.AddComponent<T4>(_entity);
            _baker.AddComponent<T5>(_entity);
            _baker.AddComponent<T6>(_entity);
            _baker.AddComponent<T7>(_entity);
            _baker.AddComponent<T8>(_entity);
            _baker.AddComponent<T9>(_entity);
            _baker.AddComponent<T10>(_entity);
            return this;
        }
    }
}
