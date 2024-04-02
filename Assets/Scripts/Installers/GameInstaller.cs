using TemplateScripts;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject soundManagerPrefab;

        public override void InstallBindings()
        {
            var soundManager = Instantiate(soundManagerPrefab);
            DontDestroyOnLoad(soundManager);

            Container.Bind<SoundManager>().FromComponentOn(soundManager).AsSingle().NonLazy();
        }
    }
}
