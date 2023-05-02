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
        [Networked(OnChanged = nameof(QuestToWinChanged))] public int QuestsToWin { get; set; }

        public int StartQuestToWin = 3;

        public QuestTarget[] Targets;
        public QuestGiver[] Givers;
        public float RegenerationTime = 10;
        public static QuestManager Instance;
        public float TimeForRestart = 15;

        public TickTimer RestartTimer;

        public static void QuestToWinChanged(Changed<QuestManager> changed)
        {
            UI.Instance.HUD.MissionText.text = string.Format(UI.Instance.HUD.Phrase, changed.Behaviour.QuestsToWin);
        }

        public override void Spawned()
        {
            if (Runner.IsServer)
            {
                QuestsToWin = StartQuestToWin;
                Targets = FindObjectsOfType<QuestTarget>();
                Givers = FindObjectsOfType<QuestGiver>();
                for (int i = 0; i < MaxQuests; i++)
                {
                    TryGenerateNewQuest();
                }
                _regenerateQuestTimer = TickTimer.CreateFromSeconds(Runner, RegenerationTime);
            }
            UI.Instance.HUD.MissionText.text = string.Format(UI.Instance.HUD.Phrase, QuestsToWin);
            Instance = this;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            Instance = default;
        }
        

        public static int UniversalId = 5;
        private TickTimer _regenerateQuestTimer;

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

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (Runner.IsServer)
            {
                if (_regenerateQuestTimer.Expired(Runner))
                {
                    for (int i = PossibleQuests.Count; i < MaxQuests; i++)
                    {
                        TryGenerateNewQuest();
                    }
                    _regenerateQuestTimer = TickTimer.CreateFromSeconds(Runner, RegenerationTime);
                }

                if (IsWin && RestartTimer.Expired(Runner))
                {
                    Runner.SetActiveScene(1);
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

        private void Update()
        {
            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    RPC_FinishQuest();
                }
            }
        }

        public void RPC_TakeItemForQuest(Quester quester, Quest quest)
        {
            if (Runner.IsServer)
            {
                for (var index = 0; index < quester.TakenQuests.Count; index++)
                {
                    var takenQuest = quester.TakenQuests[index];
                    if (takenQuest.Id == quest.Id)
                    {
                        quest.QuestState = QuestState.Delivering;
                        quester.TakenQuests.Set(index, quest);
                    }
                }
                quester.GetComponent<Character>().Items.Add(new ItemDesc()
                {
                    Id = UniversalId++,
                    ItemId = quest.ItemID
                });
            }
        }

        [Rpc]
        public void RPC_FinishQuest()
        {
            if (Runner.IsServer)
            {
                QuestsToWin--;
                if (QuestsToWin <= 0)
                {
                    IsWin = true;
                    RestartTimer = TickTimer.CreateFromSeconds(Runner, TimeForRestart);
                    RPC_ShowWinScreen();
                }
            }
        }

        [Networked]
        public NetworkBool IsWin { get; set; }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ShowWinScreen()
        {
            Boss.Instance.PlayDeath();
            UI.Instance.WinScreen.Show(TimeForRestart);
        }
    }
}