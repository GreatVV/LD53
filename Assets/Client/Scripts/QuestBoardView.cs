using System;
using LeopotamGroup.Globals;
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
        public GameObject CantTakeRoot;

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
            CantTakeRoot.SetActive(!CanTakeQuest());
        }

        private void Update()
        {
            if (Quest.QuestState != QuestState.ReadyToBeTaken)
            {
                Destroy(gameObject);
            }
        }

        public bool CanTakeQuest()
        {
            if (Quest.QuestState == QuestState.ReadyToBeTaken)
            {
                var staticData = Service<StaticData>.Get();
                var items = staticData.Items;
                var itemID = Quest.ItemID.ToString();
                if (items.TryGetByItemId(itemID, out var item))
                {
                    var carryingCapacity = staticData.Formulas.GetCarryingCapacity(Quester.GetComponent<Character>().Characteristics);
                    if (item.ItemDescription.Mass <= carryingCapacity)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void TryTakeQuest()
        {
            if (CanTakeQuest())
            {
                QuestManager.Instance.RPC_GiveQuestToPlayer(Quester, Quest);
            }
        }
    }
}