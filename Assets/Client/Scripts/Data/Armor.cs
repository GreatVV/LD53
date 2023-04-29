using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Armor")]
    public class Armor : ScriptableObject
    {
        public DefenceDescription[] Defence;
    }
}