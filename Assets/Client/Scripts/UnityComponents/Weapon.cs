using LeopotamGroup.Globals;
using NaughtyAttributes;
using UnityEngine;

namespace LD52
{
    public class Weapon : MonoBehaviour, IWeapon
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

        public void StartAttack()
        {
            if(Range)
            {
                var projectile = Instantiate(Projectile, ProjectileSpawnPoint.position, Owner.transform.rotation);
                projectile.Owner = Owner;
                projectile.WeaponData = Data;
            }
            else
            {
                Collider.enabled = true;
            }
        }

        public void EndAttack()
        {
            Collider.enabled = false;
        }

    }

    public interface IWeapon
    {
        Transform transform {get;}
        Character Owner {get; set;}
        WeaponData Data {get; set;}
        void StartAttack();
        void EndAttack();
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
                var damage = staticData.Formulas.GetDamage(attacker.Characteristics, target.Characteristics, weaponData);
                Debug.Log($"Damage {damage} from {attacker.name} to {target.name}");
                target.Heals -= damage;
            }
        }
    }
}