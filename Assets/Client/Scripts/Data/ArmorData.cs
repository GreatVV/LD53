using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Armor")]
    public class ArmorData : ItemData
    {
        public DefenceDescription[] Defence;
    }
}