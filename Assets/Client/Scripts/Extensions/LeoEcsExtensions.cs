using Leopotam.Ecs;

namespace ECS
{
    public static class LeoEcsExtensions
    {
        public static void Clear<T>(this EcsFilter<T> filter) where T : struct
        {
            foreach(var i in filter)
            {
                filter.GetEntity(i).Del<T>();
            }
        }
    }
}