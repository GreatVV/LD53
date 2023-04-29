using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD52
{
    public class QuestBoardView : MonoBehaviour
    {
        public TMP_Text Text;
        public Quest Quest;
        public Quester Quester;
        public Button Button;

        private void Start()
        {
            Button.onClick.AddListener(TryTakeQuest);
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(TryTakeQuest);
        }

        public void Set(Quest quest, Quester quester)
        {
            Quest = quest;
            Quester = quester;
            Text.text = quest.ToDescription();
        }

        private void Update()
        {
            if (Quest.QuestState != QuestState.None)
            {
                Destroy(gameObject);
            }
        }

        public void TryTakeQuest()
        {
            QuestManager.Instance.RPC_GiveQuestToPlayer(Quester, Quest);
        }
    }
}