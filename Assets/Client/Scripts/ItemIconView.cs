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
        public void OnPointerDown(PointerEventData eventData)
        {
            InventoryView.SelectItem(InventoryIcon);
        }
    }
}