using Leopotam.Ecs;

namespace LD52
{
    public class InventorySystem : IEcsRunSystem
    {
        private readonly EcsFilter<OpenInventoryEvent> _filter = default;
        private readonly RuntimeData _runtimeData = default;
        private readonly UI _ui = default;
    
        public void Run()
        {
            foreach(var i in _filter)
            {
                ref var cmd = ref _filter.Get1(i);
                
                _ui.InventoryView.Set(_runtimeData.Inventory);
                _ui.InventoryView.gameObject.SetActive(true);
                _filter.GetEntity(i).Del<OpenInventoryEvent>();
            }
        }
    }
}