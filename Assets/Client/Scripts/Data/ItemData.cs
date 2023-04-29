using UnityEngine;

namespace LD52
{
    public abstract class ItemData : ScriptableObject
    {
        public Sprite Icon;
        public GameObject Prefab;
        public string Name;
    }
}