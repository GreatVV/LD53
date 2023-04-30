using NaughtyAttributes;
using UnityEngine;

namespace LD52
{
    public class SceneData : MonoBehaviour
    {
        public Camera Camera;
        public EntityLink[] EntityLinks;
        public UI UI;

        [Button]
        public void Grab()
        {
            EntityLinks = GameObject.FindObjectsByType<EntityLink>(FindObjectsSortMode.None);
        }
    }
}