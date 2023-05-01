using Fusion;
using UnityEngine;

namespace LD52
{
    public class EnemyAI : NetworkBehaviour
    {
        public Character Character;
        public Character Target;
        public float LookEnemyRadius;
        public float LookEnemyRate;
        public float AttackDistance;
        private float _lastLookEnemyTime;

        private Collider[] _overlapResults = new Collider[10];


        private void RunAI()
        {
            if(Character.IsDead) return;

            if(IsReadyForLookTarget())
            {
                LookTarget();
            }

            if(Target == default || Target.IsDead) return;

            if(transform.IsNear(Target.transform, AttackDistance))
            {
                Attack();
            }
            else
            {
                MoveToTarget();
            }
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork(); 
            if(Runner.IsServer)
            {
                RunAI();
            }
        }

        private void Attack()
        {
            Character.RPC_Attack();
        }

        private void MoveToTarget()
        {
            var distance = transform.position - Target.transform.position;
            var direction = distance.normalized;
            Character.cc.SetLookRotation(direction);
            Character.cc.SetKinematicVelocity(direction * Character.Speed);
        }

        private void LookTarget()
        {
            _lastLookEnemyTime = Time.time;
            var hits = Physics.OverlapSphereNonAlloc(transform.position, LookEnemyRadius, _overlapResults);


            for(var i = 0; i < hits; i++)
            {
                if(_overlapResults[i].TryGetComponent<Character>(out var other))
                {
                    if(other == this) continue;
                    var enemy = other.GetComponent<EnemyAI>();
                    if(enemy == default && other.IsDead == false)
                    {
                        Target = other;
                        return;
                    }
                }
            }
        }

        private bool IsReadyForLookTarget()
        {
            return (Target == default || Target.IsDead) && _lastLookEnemyTime + LookEnemyRate > Time.time;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GizmosUtils.DrawWireCircle(transform.position, Quaternion.Euler(90f, 0f, 0f), LookEnemyRadius);
        }
    }
}