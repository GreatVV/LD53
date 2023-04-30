using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Items List")]
    public class ItemsList : ScriptableObject
    {
        public ItemData[] Items;

        public ItemData GetRandomItem()
        {
            var r = Random.Range(0, Items.Length);
            return Items[r];
        }

        public ItemData GetItemById(string id)
        {
            foreach(var item in Items)
            {
                if(item.Description.Id == id)
                {
                    return item;
                }
            }

            return default;
        }
    }
}