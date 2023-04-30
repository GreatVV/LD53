using Leopotam.Ecs;

namespace LD52
{
    internal class OpenUISystem : IEcsRunSystem
    {
        private EcsFilter<OpenInventoryEvent> _openInventoryFilter;
        private EcsFilter<OpenQuestWindowEvent> _openQuestFilter;
        private EcsFilter<OpenDiaryEvent> _openDiaryFilter;
        private RuntimeData _runtimeData;
        public void Run()
        {
            foreach (var i in _openInventoryFilter)
            {
                UI.Instance.InventoryView.Show(_runtimeData.Inventory);
                _openInventoryFilter.GetEntity(i).Del<OpenInventoryEvent>();
            }
            
            foreach (var i in _openQuestFilter)
            {
                UI.Instance.QuestWindow.Show(_runtimeData.Quester);
                _openQuestFilter.GetEntity(i).Del<OpenQuestWindowEvent>();
            }
            
            foreach (var i in _openDiaryFilter)
            {
                UI.Instance.DiaryView.Show(_runtimeData.Diary);
                _openDiaryFilter.GetEntity(i).Del<OpenDiaryEvent>();
            }
        }
    }
}