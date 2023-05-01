using System;
using System.Linq;
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
                    quest.QuestState = QuestState.ReadyToBeTaken;
                    var possibleItems = questGiver.PossibleItems.Where(x=>!string.IsNullOrEmpty(x)).ToArray();
                    var range = Random.Range(0, possibleItems.Length);
                    quest.ItemID = possibleItems[range];
                    PossibleQuests.Add(quest);
                }
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_GiveQuestToPlayer(Quester quester, Quest quest)
        {
            if (Runner.IsServer)
            {
                PossibleQuests.Remove(quest);
                quester.TakenQuests.Add(quest);
            }

            if (Runner.LocalPlayer.IsValid)
            {
                Debug.Log("On local client");
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Service<EcsWorld>.Get().NewEntity().Get<TryToOpenQuestBoardEvent>().value = this;
        }
    }

    
}