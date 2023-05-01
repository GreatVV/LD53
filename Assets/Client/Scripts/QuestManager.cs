using System;
using System.Linq;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace LD52
{
    public class QuestManager : NetworkBehaviour, IPointerDownHandler
    {
        [Networked][Capacity(8)] public NetworkLinkedList<Quest> PossibleQuests { get; }
        
        [Networked] public int MaxQuests { get; set; }

        public QuestTarget[] Targets;
        public QuestGiver[] Givers;
        public static QuestManager Instance;

        public override void Spawned()
        {
            if (Runner.IsServer)
            {
                Targets = FindObjectsOfType<QuestTarget>();
                Givers = FindObjectsOfType<QuestGiver>();
                for (int i = 0; i < MaxQuests; i++)
                {
                    TryGenerateNewQuest();
                }
            }

            Instance = this;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            Instance = default;
        }

        public static int UniversalId = 5;
        public void TryGenerateNewQuest()
        {
            if (Runner.IsServer)
            {
                if (PossibleQuests.Count < MaxQuests)
                {
                    var quest = new Quest();
                    quest.Id = UniversalId++;
                    var questGiver = Givers[Random.Range(0, Givers.Length)];
                    quest.From = questGiver.Object.Id;
                    quest.To = Targets[Random.Range(0, Targets.Length)].Object.Id;
                    quest.QuestState = QuestState.ReadyToBeTaken;
                    var possibleItems = questGiver.PossibleItems.Where(x=>!string.IsNullOrEmpty(x)).ToArray();
                    var range = Random.Range(0, possibleItems.Length);
                    quest.ItemID = possibleItems[range];
                    quest.XPReward = 1000;
                    PossibleQuests.Add(quest);
                }
            }
        }

        [Rpc]
        public void RPC_GiveQuestToPlayer(Quester quester, Quest quest)
        {
            if (Runner.IsServer)
            {
                quest.QuestState = QuestState.NeedItem;
                Debug.Log($"Take quest: {quest.Id}");
                for (var index = PossibleQuests.Count - 1; index >= 0; index--)
                {
                    var pq = PossibleQuests[index];
                    if (pq.Id == quest.Id)
                    {
                        pq.QuestState = QuestState.NeedItem;
                        PossibleQuests.Set(index, pq);
                        PossibleQuests.Remove(pq);
                    }
                }
                quester.TakenQuests.Add(quest);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Service<EcsWorld>.Get().NewEntity().Get<TryToOpenQuestBoardEvent>().value = this;
        }

        public void RPC_TakeItemForQuest(Quester quester, Quest takenQuest)
        {
            if (Runner.IsServer)
            {
                takenQuest.QuestState = QuestState.Delivering;
                for (var index = 0; index < quester.TakenQuests.Count; index++)
                {
                    var questerTakenQuest = quester.TakenQuests[index];
                    if (questerTakenQuest.Id == takenQuest.Id)
                    {
                        quester.TakenQuests.Set(index, takenQuest);
                    }
                }
                quester.GetComponent<Character>().Items.Add(new ItemDesc()
                {
                    Id = UniversalId++,
                    ItemId = takenQuest.ItemID
                });
            }
        }
    }

    
}