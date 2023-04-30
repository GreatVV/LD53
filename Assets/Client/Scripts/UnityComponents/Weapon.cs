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
            if(other.gameObject == Owner.gameObject) return;

            Debug.Log($" {other.name} attacked");

            if(other.TryGetComponent<Character>(out var target))
            {
                if (target.Characteristics.Equals(default)) //todo check
                {
                    return;
                }
                var staticData = Service<StaticData>.Get();
                var damage = staticData.Formulas.GetDamage(Owner.Characteristics, target.Characteristics, Data);
                Debug.Log($"Damage {damage} from {Owner.name} to {target.name}");
                target.Heals -= damage;
            }
        }

        public void StartAttack()
        {
            if(Range)
            {
                var projectile = Instantiate(Projectile, ProjectileSpawnPoint.position, Owner.transform.rotation);
                projectile.Owner = Owner;
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
}