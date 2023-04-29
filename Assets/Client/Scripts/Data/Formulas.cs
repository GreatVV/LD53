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


        public double GetHeals(Characteristics characteristics)
        {
            return characteristics.GetCharacteristic(HealsCharacteristic) * HealsMultipler;
        }

        public double GetDamage(Characteristics damager, Characteristics target, Weapon weapon)
        {
            double result = 0;

            var mainCharacteristic = damager.GetCharacteristic(weapon.Characteristic);

            foreach(var weaponDamge in weapon.Damage)
            {
                var dmg = damager.GetDamage(weaponDamge.Type);
                var defence = target.GetDefence(weaponDamge.Type);
                result += dmg * (100 + mainCharacteristic*10) / 100 * (100 - defence);
            }

            return System.Math.Max(result, MinDamage);
        }

        public int GetExp(int killerLevel, int targetLevel)
        {
            return Mathf.Max(100*(2+killerLevel - targetLevel), 0);
        }
    }
}