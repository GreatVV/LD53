using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;
using UnityEngine.UI;

namespace LD52
{
    public class HUD : MonoBehaviour
    {
        public Button InventoryButton;
        public Button QuestWindowButton;

        public void Start()
        {
            InventoryButton.onClick.AddListener(OnInventoryButtonClick);
            QuestWindowButton.onClick.AddListener(OnQuestWindowButtonClick);
        }

        private void OnDestroy()
        {
            InventoryButton.onClick.RemoveListener(OnInventoryButtonClick);
            QuestWindowButton.onClick.RemoveListener(OnQuestWindowButtonClick);
        }

        private void OnInventoryButtonClick()
        {
            //todo make open inventory
            Service<EcsWorld>.Get().NewEntity().Get<OpenInventoryEvent>();
        }
        
        private void OnQuestWindowButtonClick()
        {
            Service<EcsWorld>.Get().NewEntity().Get<OpenQuestWindowEvent>();
        }
    }
}