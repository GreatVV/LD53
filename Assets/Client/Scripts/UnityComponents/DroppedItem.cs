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
/*
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<Character>(out var character))
            {
                if (HasInputAuthority)
                {
                    RPC_Pick(character, Data.Description.Id);
                }
            }
        }

        [Rpc]
        private void RPC_Pick(Character character, string droppedItem)
        {
            if (Runner.IsServer)
            {
                character.Items.Add(new ItemDesc()
                {
                    Id = QuestManager.UniversalId++,
                    ItemId = droppedItem
                });
                Runner.Despawn(Object);
            }
        }*/
    }
}