using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Characteristic Type")]
    public class CharacteristicType : ScriptableObject
    {
        public int Value;
        
        public static implicit operator int(CharacteristicType d) => d.Value;
    }
}