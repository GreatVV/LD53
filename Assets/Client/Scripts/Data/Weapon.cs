using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Weapon")]
    public class Weapon : ScriptableObject
    {
        public float Coldown;
        public DamageDescription[] Damage;
        public CharacteristicType Characteristic;
    }
}