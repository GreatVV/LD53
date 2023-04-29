using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Healing potion")]
    public class HealingPotion : UsableItem
    {
        [Range(0f, 1f)]
        public float Heals;
    }
}