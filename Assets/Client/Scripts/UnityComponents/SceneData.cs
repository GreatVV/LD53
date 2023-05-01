using NaughtyAttributes;
using UnityEngine;

namespace LD52
{
    public class SceneData : MonoBehaviour
    {
        public Camera Camera;
        public CameraFollow CameraFollow;
        public EnemySpawner[] Spawners;
        public EntityLink[] EntityLinks;
        public Transform PlayerSpawnPosition;

        [Button]
        public void Grab()
        {
            EntityLinks = GameObject.FindObjectsByType<EntityLink>(FindObjectsSortMode.None);
        }
    }
}