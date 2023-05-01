using LeopotamGroup.Globals;
using UnityEngine;

namespace LD52
{
    public static class DamageHelper
    {
        public static void SendDamage(Character attacker, Character target, WeaponData weaponData)
        {
            if (target == attacker)
            {
                return;
            }

            Debug.Log($" {target.name} attacked");
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