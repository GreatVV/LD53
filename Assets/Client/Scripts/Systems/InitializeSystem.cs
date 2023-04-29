using Leopotam.Ecs;

namespace LD52
{
    public class InitializeSystem : IEcsInitSystem
    {
        private readonly SceneData _sceneData = default;
        private readonly EcsWorld _world = default;
    
        public void Init()
        {
            foreach(var link in _sceneData.EntityLinks)
            {
                link.Init(_world);
                
            }
        }
    }
}