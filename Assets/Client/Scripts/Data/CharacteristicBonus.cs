using Fusion;

namespace LD52
{
    [System.Serializable]
    public struct CharacteristicBonus : INetworkStruct
    {
        public int Type;
        public float Value;
        public float Multipler;

        public override string ToString()
        {
            return $"t:{Type} v:{Value} m:{Multipler}";
        }
    }
}