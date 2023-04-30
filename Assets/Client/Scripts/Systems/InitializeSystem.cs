using Leopotam.Ecs;
using UnityEngine;

namespace LD52
{
    public class InitializeSystem : IEcsInitSystem
    {
        private readonly SceneData _sceneData = default;
        private readonly EcsWorld _world = default;
        private readonly StaticData _staticData;
    
        public void Init()
        {
            foreach(var link in _sceneData.EntityLinks)
            {
                link.Init(_world);
            }

            Object.Instantiate(_staticData.UI);
        }
    }
}