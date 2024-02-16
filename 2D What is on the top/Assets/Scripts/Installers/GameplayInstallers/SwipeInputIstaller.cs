using GG.Infrastructure.Utils.Swipe;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SwipeInputIstaller : MonoInstaller
    {
        [SerializeField] private SwipeListener _swipeListener;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SwipeListener>().FromInstance(_swipeListener).AsSingle();
        }
    }
}