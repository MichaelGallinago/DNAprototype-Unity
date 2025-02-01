using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Scenes.Menu.Logo
{
    public class LogoAnimation : MonoBehaviour
    {
        private void Start() => LMotion.Create(90f, 360f, 1f)
            .WithEase(Ease.InOutQuad)
            .BindToLocalEulerAnglesY(transform);
    }
}
