using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Formulas")]
    public class Formulas : ScriptableObject
    {
        public CharacteristicType HealsCharacteristic;
        public double HealsMultipler;

        [Space]
        public double MinDamage;

        [Space]
        public Levels Levels;
        public double ExpForKillMultipler;

        [Header("Carry weight")]
        public CharacteristicType CarryingWeightCharacteristic;

        public int CarryingWeightMultiplayer = 10;
        
        public int GetCarryingCapacity(Characteristics characteristics)
        {
            return (int)(characteristics.GetCharacteristic(CarryingWeightCharacteristic) * CarryingWeightMultiplayer);
        }


        public double GetHeals(Characteristics characteristics)
        {
            return characteristics.GetCharacteristic(HealsCharacteristic) * HealsMultipler;
        }

        public double GetDamage(Characteristics damager, Characteristics target, WeaponData weapon)
        {
            double result = 0;

            var mainCharacteristic = damager.GetCharacteristic(weapon.Characteristic);

            foreach(var weaponDamge in weapon.Damage)
            {
                var dmg = damager.GetDamage(weaponDamge.DamageType);
                var defence = target.GetDefence(weaponDamge.DamageType);
                result += dmg * (mainCharacteristic*10) / 100 * (100 - defence);
            }

            return System.Math.Max(result, MinDamage);
        }

        public int GetExp(int killerLevel, int targetLevel)
        {
            return Mathf.Max(100*(2+killerLevel - targetLevel), 0);
        }
    }
}