using System;
using Fusion;
using LeopotamGroup.Globals;
using UnityEngine;

namespace LD52
{
    public class Quester : NetworkBehaviour
    {
        [Networked, Capacity(4)]
        public NetworkLinkedList<Quest> TakenQuests { get; }
        
        public ParticleSystem TakeQuestParticle;
        public ParticleSystem TakeItemParticle;
        public ParticleSystem DeliverItemParticle;

        private bool CanOpenQuestBoard = true;

        public override void Spawned()
        {
            base.Spawned();
            if (HasInputAuthority)
            {
                Service<RuntimeData>.Get().Quester = this;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (HasInputAuthority)
            {
                var questGiver = other.GetComponentInChildren<QuestGiver>();
                if (questGiver)
                {
                    foreach (var takenQuest in TakenQuests)
                    {
                        if (takenQuest.QuestState == QuestState.NeedItem)
                        {
                            if (takenQuest.From == questGiver.Object.Id)
                            {
                                Debug.Log($"Try to take item from {takenQuest.GetFromLocalizedName()} quest giver");
                                Service<RuntimeData>.Get().Diary.AddEntry(new TakeQuestItemEntry(takenQuest));
                                TakeItemParticle.Play();
                                QuestManager.Instance.RPC_TakeItemForQuest(this, takenQuest);
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
                                DeliverItemParticle.Play();
                                RPC_FinishQuest(this, takenQuest);
                                Service<RuntimeData>.Get().Diary.AddEntry(new FinishQuestEntry(takenQuest));
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

        [Rpc]
        private void RPC_FinishQuest(Quester quester, Quest takenQuest)
        {
            if (Runner.IsServer)
            {
                var character = quester.GetComponent<Character>();
                for (var index = character.Items.Count - 1; index >= 0; index--)
                {
                    var itemDesc = character.Items[index];
                    if (itemDesc.ItemId == takenQuest.ItemID)
                    {
                        character.Items.Remove(itemDesc);
                    }
                }

                for (var index = quester.TakenQuests.Count - 1; index >= 0; index--)
                {
                    var questerTakenQuest = quester.TakenQuests[index];
                    if (questerTakenQuest.Id == takenQuest.Id)
                    {
                        quester.TakenQuests.Remove(questerTakenQuest);
                    }
                }

                var characteristics = character.Characteristics;
                characteristics.Exp += takenQuest.XPReward;
                character.Characteristics = characteristics;

                QuestManager.Instance.RPC_FinishQuest();
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (HasInputAuthority)
            {
                var questManager = other.GetComponentInChildren<QuestManager>();
                if (questManager)
                {
                    CanOpenQuestBoard = true;
                    UI.Instance.QuestBoard.gameObject.SetActive(false);
                }
            }
        }
    }
}