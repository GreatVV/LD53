using Leopotam.Ecs;
using UnityEngine;
using LeopotamGroup.Globals;

namespace LD52
{
    public class Game : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        public RuntimeData RuntimeData;
        public SceneData SceneData;
        public StaticData StaticData;

        private void Start ()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
            InitServices();
            _systems
                .Add(new InitializeSystem())
                .Add(new ServerLogicSystem())
                .Add(new OpenUISystem())
                .Add(new InventorySystem())
                .Inject(SceneData)
                .Inject(StaticData)
                .Inject(RuntimeData)
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if(_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }

        private void InitServices()
        {
            Service<EcsWorld>.Set(_world);
            Service<RuntimeData>.Set(RuntimeData);
            Service<StaticData>.Set(StaticData);
            Service<UI>.Set(SceneData.UI);
        }
    }
}