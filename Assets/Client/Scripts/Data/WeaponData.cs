using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Weapon")]
    public class WeaponData : ItemData
    {
        public float Coldown;
        public DamageDescription[] Damage;
        public CharacteristicType Characteristic;
        public PivotType Pivot;
        public AnimatorOverrideController Animations;
    }
}