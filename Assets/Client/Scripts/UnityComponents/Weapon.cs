using Fusion;
using LeopotamGroup.Globals;
using NaughtyAttributes;
using UnityEngine;

namespace LD52
{
    public class Weapon : ItemView, IWeapon
    {
        public Collider Collider;
        public bool Range;
        [ShowIf(nameof(Range))] public Projectile Projectile;
        [ShowIf(nameof(Range))] public Transform ProjectileSpawnPoint;
        public Character Owner {get; set;}
        public WeaponData Data {get; set;}
        
        private void OnTriggerEnter(Collider other)
        {
            DamageHelper.SendDamage(Owner, other, Data);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_StartAttack()
        {
            if(Range)
            {
                var projectile = Runner.Spawn(Projectile, ProjectileSpawnPoint.position, Owner.transform.rotation);
                projectile.Owner = Owner;
                projectile.WeaponData = Data;
                projectile.Fire();

            }
            else
            {
                Collider.enabled = true;
            }
        }

        public void EndAttack()
        {
            if(Collider != default)
            {
                Collider.enabled = false;
            }
           
           Owner.IsStopped = false;
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
            if(!targetCollider || !attacker || targetCollider.gameObject == attacker.gameObject) return;

            if(targetCollider.TryGetComponent<Character>(out var target))
            {
                SendDamage(attacker, target, weaponData);
            }
        }
    }
}