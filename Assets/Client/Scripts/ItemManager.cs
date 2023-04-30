using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Item Manager")]
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

        public bool TryGetByItemId(string itemID, out ItemView o)
        {
            foreach (var x in Items)
            {
                if (x.ItemDescription.Id == itemID)
                {
                    o = x;
                    return true;
                }
            }

            o = default;
            return false;
        }
    }
}