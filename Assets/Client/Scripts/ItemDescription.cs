using System;
using UnityEngine;

namespace LD52
{
    [Serializable]
    public class ItemDescription
    {
        public string Id;
        public string LocalizedName;
        public Sprite Icon;
        public Vector2Int Size;
        public ItemView Prefab;
    }
}