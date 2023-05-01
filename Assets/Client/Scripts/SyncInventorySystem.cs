using Leopotam.Ecs;
using UnityEngine;

namespace LD52
{
    internal class SyncInventorySystem : IEcsRunSystem
    {
        private EcsFilter<UpdateInventory> _filter;
        private RuntimeData _runtimeData;
        private StaticData _staticData;
        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var updateInventory = ref _filter.Get1(i);
                var index = 0;
                var inventory = _runtimeData.Inventory;
                foreach (var itemDesc in updateInventory.value)
                {
                    var inventoryItems = inventory.Items;
                    if (index < inventoryItems.Count)
                    {
                        if (inventoryItems[index].NetworkSyncId != itemDesc.Id)
                        {
                            for (var index1 = UI.Instance.InventoryView.Icons.Count - 1; index1 >= 0; index1--)
                            {
                                var inventoryViewIcon = UI.Instance.InventoryView.Icons[index1];
                                if (inventoryViewIcon.ItemState == inventoryItems[index])
                                {
                                    Object.Destroy(inventoryViewIcon.IconView.gameObject);
                                    UI.Instance.InventoryView.Icons.RemoveAt(index1);
                                }
                            }

                            inventoryItems.RemoveAt(index);
                            TryAdd(itemDesc, inventory, index);
                        }
                    }
                    else
                    {
                        TryAdd(itemDesc, inventory);
                    }

                    index++;
                }
                
                
                _filter.GetEntity(i).Del<UpdateInventory>();
            }
        }

        private void TryAdd(ItemDesc itemDesc, Inventory inventory, int index = -1)
        {
            if (_staticData.Items.TryGetByItemId(itemDesc.ItemId.ToString(), out var item))
            {
                inventory.TryAddItem(item.ItemDescription, itemDesc.Id, index);
            }
        }
    }
}