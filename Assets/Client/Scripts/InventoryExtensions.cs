using System.Text;
using UnityEngine;

namespace LD52
{
    public static class InventoryExtensions
    {
        public static string PrintInventory(this Inventory inventory)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < inventory.Width; i++)
            {
                for (int j = 0; j < inventory.Height; j++)
                {
                    stringBuilder.Append(inventory.Taken[i, j]);
                    stringBuilder.Append(" ");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public static bool TryAddItem(this Inventory inventory, ItemDescription itemDescription, int networkId, int index =-1)
        {
            for (var i = 0; i < inventory.Width; i++)
            {
                for (int j = 0; j < inventory.Height; j++)
                {
                    if (inventory.Taken[i, j] == -1)
                    {
                        var success = true;
                        for (int k = 0; k < itemDescription.Size.x; k++)
                        {
                            for (int l = 0; l < itemDescription.Size.y; l++)
                            {
                                if (i + l < inventory.Width && j + k < inventory.Height)
                                {
                                    if (inventory.Taken[i + l, j + k] == -1)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        success = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    success = false;
                                    break;
                                }
                            }
                        }

                        if (success)
                        {
                            var itemState = new ItemState();
                            itemState.NetworkSyncId = networkId;
                            itemState.Position = new Vector2Int(i, j);
                            itemState.ItemDescription = itemDescription;
                            itemState.Amount = 1;

                            for (int k = 0; k < itemDescription.Size.x; k++)
                            {
                                for (int l = 0; l < itemDescription.Size.y; l++)
                                {
                                    inventory.Taken[i + l, j + k] = inventory.Items.Count;
                                }
                            }

                            if (index == -1)
                            {
                                inventory.Items.Add(itemState);
                            }
                            else
                            {
                                inventory.Items.Insert(index, itemState);
                            }
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static void TryDelete(this Inventory inventory, ItemState itemState)
        {
            var indexOf = inventory.Items.IndexOf(itemState);
            if (indexOf >= 0)
            {
                for (int i = 0; i < inventory.Width; i++)
                {
                    for (int j = 0; j < inventory.Height; j++)
                    {
                        if (inventory.Taken[i, j] == indexOf)
                        {
                            inventory.Taken[i, j] = -1;
                        }
                        else
                        {
                            if (inventory.Taken[i, j] > indexOf)
                            {
                                inventory.Taken[i, j]--;
                            }
                        }
                    }
                }

                inventory.Items.RemoveAt(indexOf);
                Debug.Log($"Remove item at {indexOf}");
            }
        }
    }
}