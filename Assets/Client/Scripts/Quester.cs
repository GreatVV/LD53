using System;
using Fusion;
using UnityEngine;

namespace LD52
{
    public class Quester : NetworkBehaviour
    {
        [Networked]
        public NetworkLinkedList<Quest> TakenQuests { get; set; }

        private bool CanOpenQuestBoard = true;

        private void OnTriggerEnter(Collider other)
        {
            if (Runner.LocalPlayer)
            {
                var questGiver = other.GetComponentInChildren<QuestGiver>();
                if (questGiver)
                {
                    foreach (var takenQuest in TakenQuests)
                    {
                        if (takenQuest.QuestState == QuestState.NeedItem)
                        {
                            if (takenQuest.From == questGiver)
                            {
                                Debug.Log("Try to take item from quest giver");
                            }
                        }
                    }
                }

                var questTarget = other.GetComponentInChildren<QuestTarget>();
                if (questTarget)
                {
                    foreach (var takenQuest in TakenQuests)
                    {
                        if (takenQuest.QuestState == QuestState.Delivering)
                        {
                            if (takenQuest.To == questTarget)
                            {
                                Debug.Log("Try give item to quest target");
                            }
                        }
                    }
                }

                var questManager = other.GetComponentInChildren<QuestManager>();
                if (questManager && CanOpenQuestBoard)
                {
                    CanOpenQuestBoard = false;
                    UI.Instance.QuestBoard.Show(questManager, this);
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (Runner.LocalPlayer)
            {
                var questManager = other.GetComponentInChildren<QuestManager>();
                if (questManager)
                {
                    CanOpenQuestBoard = true;
                }
            }
        }
    }
}