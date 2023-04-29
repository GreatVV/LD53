using Leopotam.Ecs;
using UnityEngine;

namespace LD52
{
    public class EntityLink : MonoBehaviour
    {
        public EcsEntity Entity;
        public EcsWorld World;
        
        public void Init(EcsWorld world)
        {
            Entity = world.NewEntity();
            ref var t = ref Entity.Get<TransformComponent>();
            t.Transform = transform;
            OnInit();
        }

        protected virtual void OnInit()
        {

        }
    }
}