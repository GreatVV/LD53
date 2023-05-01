using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LD52
{
    public class ItemIconView : MonoBehaviour, IPointerDownHandler
    {
        public Image Image;
        public GameObject SelectedBorder;
        public InventoryIcon InventoryIcon;
        public InventoryView InventoryView;
        public RectTransform TargetCellView;
        public void OnPointerDown(PointerEventData eventData)
        {
            InventoryView.SelectItem(InventoryIcon);
        }

        private void Update()
        {
            if (TargetCellView)
            {
                (transform as RectTransform).anchoredPosition = TargetCellView.anchoredPosition;
            }
        }
    }
}