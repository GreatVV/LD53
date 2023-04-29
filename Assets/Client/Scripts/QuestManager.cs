using System;
using Fusion;
using Leopotam.Ecs;
using LeopotamGroup.Globals;
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
            }

            Instance = this;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            Instance = default;
        }

        public void TryGenerateNewQuest()
        {
            if (Runner.IsServer)
            {
                if (PossibleQuests.Count < MaxQuests)
                {
                    var quest = new Quest();
                    var questGiver = Givers[Random.Range(0, Givers.Length)];
                    quest.From = questGiver.Object.Id;
                    quest.To = Targets[Random.Range(0, Targets.Length)].Object.Id;
                    quest.ItemID = questGiver.PossibleItems[Random.Range(0, questGiver.PossibleItems.Length)].ItemDescription.Id;
                    PossibleQuests.Add(quest);
                }
            }
        }

        [Rpc]
        public void RPC_GiveQuestToPlayer(Quester quester, Quest quest)
        {
            if (Runner.IsServer)
            {
                PossibleQuests.Remove(quest);
                quester.TakenQuests.Add(quest);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Service<EcsWorld>.Get().NewEntity().Get<TryToOpenQuestBoardEvent>().value = this;
        }
    }

    
}