using Leopotam.Ecs;
using Object = UnityEngine.Object;

namespace LD52
{
    public class InitializeSystem : IEcsInitSystem
    {
        private readonly SceneData _sceneData = default;
        private readonly EcsWorld _world = default;
        private readonly StaticData _staticData;
        private readonly RuntimeData _runtimeData;
    
        public void Init()
        {
            foreach(var link in _sceneData.EntityLinks)
            {
                link.Init(_world);
            }

            _runtimeData.Diary.AddEntry(new StartDiaryEntry());

            Object.Instantiate(_staticData.UI);
        }
    }
}