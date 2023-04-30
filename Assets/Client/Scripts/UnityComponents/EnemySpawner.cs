using Fusion;
using UnityEngine;

namespace LD52
{
    public class EnemySpawner : NetworkBehaviour
    {
        [Networked] public float Coldown {get; set;}
        public Character Prefab;
        public float Radius;
        public int EnemyLevel;

        public override void Spawned()
        {
            base.Spawned();
            Spawn();
        }

        [ContextMenu("Spawn")]
        private void Spawn()
        {
            var character = Runner.Spawn(Prefab, GetSpawnPoint());
            character.Dead += DeadHandler;
            Coldown = Runner.SimulationTime;
        }

        private Vector3 GetSpawnPoint()
        {
            var r =  Random.insideUnitCircle * Radius;
            var spawnPoint = transform.position + new Vector3(r.x, 0f, r.y);
            return spawnPoint;
        }

        private void DeadHandler(Character character)
        {
            character.Respawn();
            character.cc.SetPosition(GetSpawnPoint());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}