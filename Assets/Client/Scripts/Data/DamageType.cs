using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Damage Type")]
    public class DamageType : ScriptableObject
    {
        public int Value;
        
        public static implicit operator int(DamageType d) => d.Value;
    }
}