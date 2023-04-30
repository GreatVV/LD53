using Fusion;
using LeopotamGroup.Globals;
using UnityEngine;

namespace LD52
{
    public class DroppedItem : NetworkBehaviour
    {
        public ItemData Data {get; private set;}
        public GameObject View;
        public Transform Container;

        public void Show(ItemData itemData)
        {
            if(View != default)
            {
                Destroy(View);
            }

            View = Instantiate(itemData.Description.Prefab).gameObject;
            View.transform.position = transform.position;
            Data = itemData;
            View.transform.position = Container.position;
            View.transform.parent = Container;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<Character>(out var character))
            {
                Pick(character);
            }
        }

        private void Pick(Character character)
        {
            var runtime = Service<RuntimeData>.Get();
            if(character == runtime.PlayerCharacter)
            {
                if(runtime.Inventory.TryAddItem(Data.Description))
                {
                    Runner.Despawn(Object);
                }
            }
        }
    }
}