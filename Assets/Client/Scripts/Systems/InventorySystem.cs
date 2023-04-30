using Leopotam.Ecs;

namespace LD52
{
    public class InventorySystem : IEcsRunSystem
    {
        private readonly EcsFilter<OpenInventoryEvent> _filter = default;
        private readonly RuntimeData _runtimeData = default;
        private UI _ui => UI.Instance;
    
        public void Run()
        {
            foreach(var i in _filter)
            {
                _ui.InventoryView.Show(_runtimeData.Inventory);
                _ui.InventoryView.gameObject.SetActive(true);
                _filter.GetEntity(i).Del<OpenInventoryEvent>();
            }
        }
    }
}