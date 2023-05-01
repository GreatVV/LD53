using System.Collections.Generic;
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
                            RemoveItemAt(inventoryItems, index);
                            TryAdd(itemDesc, inventory, index);
                        }
                    }
                    else
                    {
                        TryAdd(itemDesc, inventory);
                    }

                    index++;
                }

                for (int j = inventory.Items.Count - 1; j >= updateInventory.value.Count; j--)
                {
                    RemoveItemAt(inventory.Items, j);
                }
                
                _filter.GetEntity(i).Del<UpdateInventory>();
            }
        }

        private void RemoveItemAt(List<ItemState> inventoryItems, int index)
        {
            _runtimeData.Inventory.TryDelete(inventoryItems[index]);   
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