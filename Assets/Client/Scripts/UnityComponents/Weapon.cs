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

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_StartAttack()
        {
            Owner.Stopped = true;
            Owner.ReadyForAttack = true;
        }

        public void EndAttack()
        {
            if(Collider != default)
            {
                Collider.enabled = false;
            }
           
           Owner.Stopped = false;
        }

    }

    public static class DamageHelper
    {
        public static void SendDamage(Character attacker, Collider targetCollider, WeaponData weaponData)
        {
            if(targetCollider.gameObject == attacker.gameObject) return;

            Debug.Log($" {targetCollider.name} attacked");

            if(targetCollider.TryGetComponent<Character>(out var target))
            {
                if (target.Characteristics.Equals(default)) //todo check
                {
                    return;
                }
                var staticData = Service<StaticData>.Get();
                var damage = (float) staticData.Formulas.GetDamage(attacker.Characteristics, target.Characteristics, weaponData);
                Debug.Log($"Damage {damage} from {attacker.name} to {target.name}");
                target.Heals -= damage;
            }
        }
    }
}