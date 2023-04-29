using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD52
{
    public class ItemInfo : MonoBehaviour
    {
        public Image Icon;
        public TMP_Text Info;
        public Button DropButton;
        private InventoryIcon CurrentItem;

        private void Start()
        {
            DropButton.onClick.AddListener(OnDropClick);
        }

        private void OnDestroy()
        {
            DropButton.onClick.RemoveListener(OnDropClick);
        }

        private void OnDropClick()
        {
            if (CurrentItem != default)
            {
                CurrentItem.IconView.InventoryView.Inventory.TryDelete(CurrentItem.ItemState);
                CurrentItem.IconView.InventoryView.UpdateView();
                CurrentItem = default;
                Set(CurrentItem);
            }
        }

        public void Set(InventoryIcon inventoryIcon)
        {
            if (inventoryIcon != default)
            {
                Icon.sprite = inventoryIcon.ItemState.ItemDescription.Icon;
                Info.text = inventoryIcon.ItemState.GetDescription();
                CurrentItem = inventoryIcon;
            }
            gameObject.SetActive(inventoryIcon != default);
        }
    }
}