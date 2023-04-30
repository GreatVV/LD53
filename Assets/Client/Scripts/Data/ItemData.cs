using UnityEngine;

namespace LD52
{
    public abstract class ItemData : ScriptableObject
    {
        public GameObject Prefab;
        public ItemDescription Description;
    }
}