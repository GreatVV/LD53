using UnityEngine;

namespace LD52
{
    public abstract class ItemData : ScriptableObject
    {
        public ItemView Prefab;
        public ItemDescription Description => Prefab.ItemDescription;
    }
}