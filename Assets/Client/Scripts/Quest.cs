using System;
using Fusion;

namespace LD52
{
    [Serializable]
    public struct Quest : INetworkStruct
    {
        [Networked]
        public QuestGiver From { get; set; }
        [Networked]
        public QuestTarget To { get; set; }
        [Networked]
        public string ItemID { get; set; }
        [Networked]
        public QuestState QuestState { get; set; }
    }
}