using Fusion;
using LeopotamGroup.Globals;
using UnityEngine;

namespace LD52
{
    public class EnemySpawner : NetworkBehaviour
    {
        public float Coldown = 10f;
        public Character Prefab;
        public float Radius;
        [Networked] public NetworkBool EnemySpawned {get; set;}
        [Networked] public NetworkBool EnemyDead {get; set;}
        private TickTimer _respawnTimer;
        private Character _spawnerCharacter;

        public override void Spawned()
        {
            if (Runner.IsServer)
            {
                SpawnEnemy();
            }
        }
        
        public void SpawnEnemy()
        {
            var runtimeData = Service<RuntimeData>.Get();
            _spawnerCharacter = runtimeData.Runner.Spawn(Prefab, GetSpawnPoint(), Quaternion.identity, onBeforeSpawned:
                (r, o) =>
                {
                    o.GetComponent<Character>().Dead += DeadHandler;
                });
            EnemySpawned = true;
        }

        public override void FixedUpdateNetwork()
        {
            if(Runner.IsServer)
            {
                if(!EnemySpawned)
                {
                    SpawnEnemy();
                }
                else if(_spawnerCharacter.IsDead && _respawnTimer.Expired(Runner))
                {
                    _spawnerCharacter.RPC_Respawn();
                    _spawnerCharacter.cc.SetPosition(GetSpawnPoint());
                }
            }
        }

        private Vector3 GetSpawnPoint()
        {
            var r =  Random.insideUnitCircle * Radius;
            var spawnPoint = transform.position + new Vector3(r.x, 0f, r.y);
            return spawnPoint;
        }

        private void DeadHandler(Character character)
        {
            var runtimeData = Service<RuntimeData>.Get();
            if(runtimeData.Runner.IsServer)
            {
                _respawnTimer = TickTimer.CreateFromSeconds(Runner, Coldown);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}