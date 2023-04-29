using UnityEngine;

namespace LD52
{
    public class ItemView : MonoBehaviour
    {
        public ItemDescription ItemDescription;
        public string Id => ItemDescription.Id;
    }
}