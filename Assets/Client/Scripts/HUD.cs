using System;
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
        public DiaryButton DiaryButton;

        public void Start()
        {
            InventoryButton.onClick.AddListener(OnInventoryButtonClick);
            QuestWindowButton.onClick.AddListener(OnQuestWindowButtonClick);
            DiaryButton.Button.onClick.AddListener(OnDiaryButtonClick);

        }

        private void OnDiaryButtonClick()
        {
            Service<EcsWorld>.Get().NewEntity().Get<OpenDiaryEvent>();
        }

        private void OnDestroy()
        {
            InventoryButton.onClick.RemoveListener(OnInventoryButtonClick);
            QuestWindowButton.onClick.RemoveListener(OnQuestWindowButtonClick);
            DiaryButton.Button.onClick.RemoveListener(OnDiaryButtonClick);
        }

        private void OnInventoryButtonClick()
        {
            Service<EcsWorld>.Get().NewEntity().Get<OpenInventoryEvent>();
        }
        
        private void OnQuestWindowButtonClick()
        {
            Service<EcsWorld>.Get().NewEntity().Get<OpenQuestWindowEvent>();
        }
    }
}