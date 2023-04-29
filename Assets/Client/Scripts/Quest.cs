﻿using System;
using Fusion;

namespace LD52
{
    [Serializable]
    public struct Quest : INetworkStruct
    {
        [Networked]
        public NetworkId From { get; set; }
        [Networked]
        public NetworkId To { get; set; }
        [Networked]
        public NetworkString<_32> ItemID { get; set; }
        [Networked]
        public QuestState QuestState { get; set; }
    }
}