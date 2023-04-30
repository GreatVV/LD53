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
        public int EnemyLevel;
        [Networked] public NetworkBool EnemySpawned {get; set;}
        [Networked] public NetworkBool EnemyDead {get; set;}
        private float _respawnTimer = 0;
        private Character _spawnerCharacter;

        public override void Spawned()
        {
            base.Spawned();
        }
        
        public void SpawnEnemy()
        {
            var runtimeData = Service<RuntimeData>.Get();
            _spawnerCharacter = runtimeData.Runner.Spawn(Prefab, GetSpawnPoint(), Quaternion.identity);
            _spawnerCharacter.Dead += DeadHandler;
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
                else if(EnemyDead)
                {
                    _respawnTimer -= Runner.DeltaTime;

                    if(_respawnTimer < 0)
                    {
                        Respawn();
                    }
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
                character.RPC_Respawn();
                character.cc.SetPosition(GetSpawnPoint());
    //            EnemyDead = true;
                _respawnTimer = Coldown;
            }
        }

        private void Respawn()
        {
            _spawnerCharacter.RPC_Respawn();
            EnemyDead = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}