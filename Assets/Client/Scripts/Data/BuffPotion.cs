using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Buff potion")]
    public class BuffPotion : UsableItem
    {
        public float Duration;
        public CharacteristicBonuses Bonuses;
    }
}