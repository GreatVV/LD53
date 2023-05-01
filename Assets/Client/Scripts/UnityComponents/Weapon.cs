using System.Collections.Generic;
using Fusion;
using LeopotamGroup.Globals;
using NaughtyAttributes;
using UnityEngine;

namespace LD52
{
    public class Weapon : ItemView, IWeapon
    {
        [Networked] public NetworkBool InAttack {get; set;}
        
        public bool Range;
        [ShowIf(nameof(Range))] public Projectile Projectile;
        [ShowIf(nameof(Range))] public Transform ProjectileSpawnPoint;
        [Networked]
        public NetworkId Owner {get; set;}
        [Networked, Capacity(32)]
        public string DataID {get; set;}

        [HideIf(nameof(Range))] public float Distance;
        [HideIf(nameof(Range))] public float Radius;
        [HideIf(nameof(Range))] public float AttackDelay;

        private List<LagCompensatedHit> overlapResults = new List<LagCompensatedHit>();

        private TickTimer _attackDelay;

        public override void FixedUpdateNetwork()
        {
            if(Object.HasStateAuthority && InAttack)
            {
                var owner = Runner.FindObject(Owner).GetComponent<Character>();
                if(!owner.IsDead)
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

        public override void Spawned()
        {
            base.Spawned();
            var owner = Runner.FindObject(Owner).GetComponent<Character>();
            var weaponData = this.GetData();
            owner.Animator.runtimeAnimatorController = weaponData.Animations;
            
            var parent = owner.Pivots.Get(weaponData.Pivot);
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            owner.Weapon = this;
        }

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

                var owner = Runner.FindObject(Owner).GetComponent<Character>();
                DamageHelper.SendDamage(owner, otherCharacter, this.GetData());
                return;
            }
        }

        private void ProjectileAttack()
        {
            var owner = Runner.FindObject(Owner).GetComponent<Character>();
            var projectile = Runner.Spawn(Projectile, ProjectileSpawnPoint.position, owner.transform.rotation);
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
            var owner = Runner.FindObject(Owner).GetComponent<Character>();
            return owner.transform.position + owner.transform.forward * Distance;
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
            target.Health -= damage;
        }

        public static void SendDamage(Character attacker, Collider targetCollider, WeaponData weaponData)
        {
            if(!targetCollider || !attacker || targetCollider.gameObject == attacker.gameObject) return;

            if(targetCollider.TryGetComponent<Character>(out var target))
            {
                SendDamage(attacker, target, weaponData);
            }
        }
    }
}