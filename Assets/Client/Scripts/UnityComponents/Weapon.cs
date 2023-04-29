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
            Owner.Animator.SetTrigger(AnimationNames.Attack);
        }

    }

    public interface IWeapon
    {
        Transform transform {get;}
        Character Owner {get; set;}
        WeaponData Data {get; set;}
        void StartAttack();
    }
}