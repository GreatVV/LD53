using System;
using Fusion;

namespace LD52
{
    [Serializable]
    public struct ItemDesc : INetworkStruct
    {
        [Networked]
        public int Id { get; set; }
        [Networked]
        public NetworkString<_32> ItemId { get; set; }
    }
}