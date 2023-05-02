using System;
using System.Linq;
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
        public Image Icon;

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
            Icon.sprite = quest.GetItemIcon();
            CantTakeRoot.SetActive(!CanTakeQuest());
        }

        private void Update()
        {
            bool any = false;
            foreach (var x in QuestManager.Instance.PossibleQuests)
            {
                if (x.Id == Quest.Id)
                {
                    any = true;
                    break;
                }
            }

            if (!any)
            {
                Destroy(gameObject);
            }
        }

        public bool CanTakeQuest()
        {
            if (Quest.QuestState == QuestState.ReadyToBeTaken && Quester.TakenQuests.Count < Quester.TakenQuests.Capacity)
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
                Service<RuntimeData>.Get().Diary.AddEntry(new QuestDiaryEntry(Quest));
                Quester.TakeQuestParticle.Play();
                QuestManager.Instance.RPC_GiveQuestToPlayer(Quester, Quest);
            }
        }
    }
}