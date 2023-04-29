using System.Collections.Generic;
using UnityEngine;

namespace LD52
{
    public class ItemManager : ScriptableObject
    {
        public Sprite NoIcon;
        public List<ItemView> Items = new List<ItemView>();

        public Sprite GetByIconId(string itemID)
        {
            foreach (var x in Items)
            {
                if (x.ItemDescription.Id == itemID) return x.ItemDescription.Icon;
            }

            return NoIcon;
        }
    }
}