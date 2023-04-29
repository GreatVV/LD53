using System.Collections.Generic;

namespace LD52
{
    public class Characteristics
    {
        public int Exp;
        public int Level;

        private Dictionary<CharacteristicType, CharacteristicBonus> Values = new();
        private Dictionary<DamageType, CharacteristicBonus> Damage = new();
        private Dictionary<DamageType, double> Defence = new();

        public void Add(CharacteristicBonuses bonuses)
        {
            foreach(var bonus in bonuses.Values)
            {
                if(Values.ContainsKey(bonus.Type))
                {
                    var value = Values[bonus.Type];
                    value.Value += bonus.Value;
                    value.Multipler += bonus.Multipler;
                    Values[bonus.Type] = value;
                }
                else
                {
                    Values[bonus.Type] = bonus;
                }
            }
        }

        public void Remove(CharacteristicBonuses bonuses)
        {
            foreach(var bonus in bonuses.Values)
            {
                if(Values.ContainsKey(bonus.Type))
                {
                    var value = Values[bonus.Type];
                    value.Value -= bonus.Value;
                    value.Multipler -= bonus.Multipler;
                    Values[bonus.Type] = value;
                }
            }
        }

        public double GetCharacteristic(CharacteristicType characteristic)
        {
            if(Values.TryGetValue(characteristic, out var value))
            {
                return value.Value * value.Multipler;
            }
            return 0;
        }

        public double GetDamage(DamageType damageType)
        {
            if(Damage.TryGetValue(damageType, out var characteristic))
            {
                var value = characteristic.Value * characteristic.Multipler;
                return System.Math.Max(0, value);
            }
            return 0;
        }

        public void AddDamage(DamageType damageType, double value, double multipler)
        {
            if(Damage.TryGetValue(damageType, out var v))
            {
                v.Value += value;
                v.Multipler += multipler;
                Damage[damageType] = v;
            }
            else
            {
                v = new CharacteristicBonus()
                {
                    Value = value,
                    Multipler = 1d,
                };
                
                Damage[damageType] = v;
            }
        }


        public void AddDamage(DamageDescription[] damage)
        {
            foreach(var d in damage)
            {
                AddDamage(d.DamageType, d.Value, 0d);
            }
        }

        public void RemoveDamage(DamageType damageType, double value, double multipler)
        {
            if(Damage.TryGetValue(damageType, out var v))
            {
                v.Value -= value;
                v.Multipler -= multipler;
                Damage[damageType] = v;
            }
        }

        public void RemoveDamage(DamageDescription[] damage)
        {
            foreach(var d in damage)
            {
                RemoveDamage(d.DamageType, d.Value, 0d);
            }
        }

        public double GetDefence(DamageType damageType)
        {
            if(Defence.TryGetValue(damageType, out var value))
            {
                return value;
            }
            return 0;
        }


        public void AddDefence(DamageType damageType, double value)
        {
            if(Defence.TryGetValue(damageType, out var oldValue))
            {
                Defence[damageType] = oldValue + value;
            }
            else
            {
                Defence[damageType] = value;
            }
        }


        public void AddDefence(DefenceDescription[] defence)
        {
            foreach(var d in defence)
            {
                AddDefence(d.DamageType, d.DefencePercent);
            }
        }

        public void RemoveDefence(DamageType damageType, double value)
        {
            if(Defence.TryGetValue(damageType, out var oldValue))
            {
                Defence[damageType] = oldValue - value;
            }
        }

        public void RemoveDefence(DefenceDescription[] defence)
        {
            foreach(var d in defence)
            {
                RemoveDefence(d.DamageType, d.DefencePercent);
            }
        }
    }
}