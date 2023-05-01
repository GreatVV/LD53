using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LD52
{
    public class InventoryView : MonoBehaviour
    {
        public Inventory Inventory;
        public Transform Root;
        public GridLayoutGroup LayoutGroup;
        public CellView CellViewPrefab;
        public CellView[,] Cells;
        public List<InventoryIcon> Icons = new ();
        public ItemIconView IconPrefab;

        public ItemInfo ItemInfo;

        public void Show(Inventory inventory)
        {
            gameObject.SetActive(true);
            var width = inventory.Width;
            var height = inventory.Height;
            Cells = new CellView[width, height];

            for (int i = Root.childCount - 1; i >= 0; i--)
            {
                Destroy(Root.GetChild(i).gameObject);
            }
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = Instantiate(CellViewPrefab, Root);   
                    cell.Position = new Vector2Int(i, j);
                    Cells[i, j] = cell;
                }
            }

            var sizeDelta = (Root as RectTransform).sizeDelta;
            var cellSize = Mathf.Min(sizeDelta.y / width, sizeDelta.x / height);
            LayoutGroup.cellSize = new Vector2(cellSize, cellSize);

            LayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            LayoutGroup.constraintCount = height;

            Inventory = inventory;
            UpdateView();
        }

        public void UpdateView()
        {
            var width = Inventory.Width;
            var height = Inventory.Height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Inventory.Taken[i, j] != -1)
                    {
                        Cells[i, j].SetTaken(true);
                    }
                    else
                    {
                        Cells[i, j].SetTaken(false);
                    }
                }
            }

            for (var index = Icons.Count - 1; index >= 0; index--)
            {
                var icon = Icons[index];
                if (!Inventory.Items.Contains(icon.ItemState))
                {
                    Icons.RemoveAt(index);
                    Destroy(icon.IconView.gameObject);
                }
            }

            foreach (var itemState in Inventory.Items)
            {
                InventoryIcon inventoryIcon = null;
                foreach (var x in Icons)
                {
                    if (x.ItemState == itemState)
                    {
                        inventoryIcon = x;
                        break;
                    }
                }

                if (inventoryIcon == default || !inventoryIcon.IconView)
                {
                    inventoryIcon = new InventoryIcon()
                    {
                        ItemState = itemState,
                        IconView = Instantiate(IconPrefab, Root)
                    };
                    inventoryIcon.IconView.Image.sprite = itemState.ItemDescription.Icon;
                    inventoryIcon.IconView.InventoryIcon = inventoryIcon;
                    inventoryIcon.IconView.InventoryView = this;
                    Icons.Add(inventoryIcon);
                }

                var cellView = Cells[itemState.Position.x, itemState.Position.y];
                var cellViewTransform = (cellView.transform as RectTransform);
                var sizeDelta = cellViewTransform.sizeDelta;
                sizeDelta.x *= itemState.ItemDescription.Size.x;
                sizeDelta.y *= itemState.ItemDescription.Size.y;
                var iconViewTransform = inventoryIcon.IconView.transform as RectTransform;
                iconViewTransform.anchoredPosition = cellViewTransform.anchoredPosition;
                inventoryIcon.IconView.TargetCellView = cellViewTransform;
                Debug.Log($"Set to {cellViewTransform.anchoredPosition} {itemState.Position}", iconViewTransform.gameObject);
                iconViewTransform.sizeDelta = sizeDelta;
            }
        }

        public void SelectItem(InventoryIcon selectedItem)
        {
            foreach (var i in Icons)
            {
                i.IconView.SelectedBorder.SetActive(i == selectedItem);
            }

            ItemInfo.Set(selectedItem);
        }
    }
}