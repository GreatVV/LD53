using Fusion;

namespace LD52
{
    [System.Serializable]
    public struct CharacteristicBonus : INetworkStruct
    {
        [Networked]
        public int Type { get; set; }
        [Networked]
        public float Value { get; set; }
        [Networked]
        public float Multipler { get; set; }
    }
}