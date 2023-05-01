using UnityEngine;

namespace LD52
{
    public class TestInventoryCreator : MonoBehaviour
    {
        public InventoryView InventoryView;
        public int Width;
        public int Height;
        private Inventory inventory;

        public ItemView ExampleItem;

        private void Start()
        {
            inventory = new Inventory();
            inventory.Width = Width;
            inventory.Height = Height;
            InventoryView.Show(inventory); 
        }

        
        [ContextMenu("Add 2x2")]
        public void AddItem2x2()
        {
            var itemDescription = new ItemDescription();
            itemDescription.Size = new Vector2Int(2, 2);
            itemDescription.Id = "x22";

            inventory.TryAddItem(itemDescription, 1);
            InventoryView.UpdateView();
        }
        
        [ContextMenu("Add example item")]
        public void AddExampleItem()
        {
            inventory.TryAddItem(ExampleItem.ItemDescription, 2);
            InventoryView.UpdateView();
        }
    }
}