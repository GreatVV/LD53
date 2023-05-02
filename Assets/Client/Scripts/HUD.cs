using System;
using Helpers.Collections;
using Leopotam.Ecs;
using LeopotamGroup.Globals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD52
{
    public class HUD : MonoBehaviour
    {
        public Button InventoryButton;
        public Button QuestWindowButton;
        public DiaryButton DiaryButton;
        public TMP_Text MissionText;
        private int ChosenText =-1;
        
        string[] bossDefeatPhrases = {
            "Complete {0} more deliveries to collect the critical items that will lead to the boss's instant demise.",
            "Finish {0} additional deliveries to secure the key components, resulting in the immediate defeat of the boss.",
            "Only {0} more deliveries stand between you and the sudden destruction of the formidable boss.",
            "Fulfill {0} remaining deliveries to acquire the essential elements, causing the boss's instant downfall.",
            "Successfully perform {0} more deliveries to obtain the crucial resources that will annihilate the boss in an instant.",
            "Carry out {0} extra deliveries to gather the vital ingredients that will ensure the boss's swift elimination.",
            "Execute {0} more deliveries to amass the indispensable materials that will bring about the boss's rapid end.",
            "Accomplish {0} more deliveries to seize the significant pieces that will precipitate the boss's immediate defeat.",
            "Conduct {0} additional deliveries to harness the powerful tools that will obliterate the boss on the spot.",
            "Achieve {0} remaining deliveries to accumulate the decisive factors that will exterminate the boss without delay."
        };


        public void Start()
        {
            InventoryButton.onClick.AddListener(OnInventoryButtonClick);
            QuestWindowButton.onClick.AddListener(OnQuestWindowButtonClick);
            DiaryButton.Button.onClick.AddListener(OnDiaryButtonClick);

            
        }

        public string Phrase
        {
            get
            {
                if (ChosenText == -1)
                {
                    ChosenText = bossDefeatPhrases.RandomIndex();
                }
                return bossDefeatPhrases [ChosenText];
            }
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