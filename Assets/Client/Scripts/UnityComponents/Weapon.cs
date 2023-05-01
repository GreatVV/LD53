using System.Collections.Generic;
using Fusion;
using LeopotamGroup.Globals;
using NaughtyAttributes;
using UnityEngine;

namespace LD52
{
    public class Weapon : ItemView, IWeapon
    {
        public bool Range;
        [ShowIf(nameof(Range))] public Projectile Projectile;
        [ShowIf(nameof(Range))] public Transform ProjectileSpawnPoint;
        public Character Owner {get; set;}
        public WeaponData Data {get; set;}

        [HideIf(nameof(Range))] public float Distance;
        [HideIf(nameof(Range))] public float Radius;
        [HideIf(nameof(Range))] public float AttackDelay;

        [Networked] public NetworkBool InAttack {get; set;}

        private List<LagCompensatedHit> overlapResults = new List<LagCompensatedHit>();

        private TickTimer _attackDelay;

        public override void FixedUpdateNetwork()
        {
            if(Object.HasStateAuthority && InAttack)
            {
                if(!Owner.IsDead)
                {
                    if(_attackDelay.Expired(Runner))
                    {
                        InAttack = false;
                        if(Range)
                        {
                            ProjectileAttack();
                        }
                        else
                        {
                            MeleeAttack();
                        }
                    }
                }
            }
        }

     //   [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void StartAttack()
        {
            InAttack = true;
            _attackDelay = TickTimer.CreateFromSeconds(Runner, AttackDelay);
        }

        public void EndAttack()
        {
        }

        private void MeleeAttack()
        {
            var point = GetAttackPoint();

            var hits = Runner.LagCompensation.OverlapSphere(point, Radius, Object.StateAuthority, overlapResults, options: HitOptions.IncludePhysX);
            for(var i = 0; i < hits; i++)
            {
                var otherCharacter = overlapResults[i].Collider.GetComponent<Character>();
                if(otherCharacter == default || otherCharacter == Owner)
                {
                    continue;
                }

                DamageHelper.SendDamage(Owner, otherCharacter, Data);
                return;
            }
        }

        private void ProjectileAttack()
        {
            var projectile = Runner.Spawn(Projectile, ProjectileSpawnPoint.position, Owner.transform.rotation);
            projectile.Owner = Owner;
            projectile.Weapon = this;
            projectile.Fire();
        }


        private void OnDrawGizmosSelected()
        {
            if(Owner != default)
            {
               Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(GetAttackPoint(), Radius); 
            }
        }

        private Vector3 GetAttackPoint()
        {
            return Owner.transform.position + Owner.transform.forward * Distance;
        }
    }

    public static class DamageHelper
    {
        public static void SendDamage(Character attacker, Character target, WeaponData weaponData)
        {
            if(target == attacker) return;

            Debug.Log($" {target.name} attacked");

            if (target.Characteristics.Equals(default)) //todo check
            {
                return;
            }
            var staticData = Service<StaticData>.Get();
            var damage = (float) staticData.Formulas.GetDamage(attacker.Characteristics, target.Characteristics, weaponData);
            Debug.Log($"Damage {damage} from {attacker.name} to {target.name}");
            target.Heals -= damage;
        }

        public static void SendDamage(Character attacker, Collider targetCollider, WeaponData weaponData)
        {
            if(targetCollider.gameObject == attacker.gameObject) return;

            if(targetCollider.TryGetComponent<Character>(out var target))
            {
                SendDamage(attacker, target, weaponData);
            }
        }
    }
}