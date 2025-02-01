using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Scenes.Menu.Card
{
    public class CardAnimation : MonoBehaviour
    {
        [SerializeField] private float _appearanceDuration = 2f;
        [SerializeField] private float _offset = 64f;
        
        private void Start() => 
            LMotion.Create(transform.position.x - _offset, transform.position.x, _appearanceDuration)
                .WithEase(Ease.InOutBack)
                .BindToPositionX(transform);
    }
}
