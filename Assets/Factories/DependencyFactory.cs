using UnityEngine;

namespace Factories
{
    [RequireComponent(typeof(TileColliderFactory))]
    public class DependencyFactory : MonoBehaviour
    {
        [field: SerializeField] public TileColliderFactory TileColliderFactory { get; private set; }
        
        public static DependencyFactory Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Instance = null;
        }
    }
}
