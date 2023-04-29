using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Characteristic Bonuses")]
    public class CharacteristicBonuses : ScriptableObject
    {
        public CharacteristicBonus[] Values;
    }
}