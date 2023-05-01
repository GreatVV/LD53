using System;
using System.Collections.Generic;
using Fusion;

namespace LD52
{
    [Serializable]
    public struct Characteristics : INetworkStruct
    {
        public int Exp;

        public int Level;

        [Networked, Capacity(6)] private NetworkDictionary<int, CharacteristicBonus> Values { get; }

        [Networked, Capacity(6)]
        private NetworkDictionary<int, CharacteristicBonus> Damage { get; }
        
        [Networked, Capacity(5)]
        private NetworkDictionary<int, double> Defence { get; }

       
       

        public void Add(CharacteristicBonuses bonuses)
        {
            foreach(var bonus in bonuses.Values)
            {
                if(Values.ContainsKey(bonus.Type))
                {
                    var value = Values[bonus.Type];
                    value.Value += bonus.Value;
                    value.Multipler += bonus.Multipler;
                    Values.Set(bonus.Type, value);
                }
                else
                {
                    Values.Set(bonus.Type, bonus);
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
                    Values.Set(bonus.Type, value);
                }
            }
        }

        public double GetCharacteristic(CharacteristicType characteristic)
        {
            if(Values.TryGet(characteristic, out var value))
            {
                return value.Value * value.Multipler;
            }
            return 0;
        }

        public float GetDamage(DamageType damageType)
        {
            if(Damage.TryGet(damageType, out var characteristic))
            {
                var value = characteristic.Value * characteristic.Multipler;
                return System.Math.Max(0, value);
            }
            return 0;
        }

        public void AddDamage(DamageType damageType, float value, float multipler)
        {
            if(Damage.TryGet(damageType, out var v))
            {
                v.Value += value;
                v.Multipler += multipler;
                Damage.Set(damageType, v);
            }
            else
            {
                v = new CharacteristicBonus()
                {
                    Value = value,
                    Multipler = 1f,
                };
                
                Damage.Set(damageType, v);
            }
        }


        public void AddDamage(DamageDescription[] damage)
        {
            foreach(var d in damage)
            {
                AddDamage(d.DamageType, d.Value, 0f);
            }
        }

        public void RemoveDamage(DamageType damageType, float value, float multipler)
        {
            if(Damage.TryGet(damageType, out var v))
            {
                v.Value -= value;
                v.Multipler -= multipler;
                Damage.Set(damageType, v);
            }
        }

        public void RemoveDamage(DamageDescription[] damage)
        {
            foreach(var d in damage)
            {
                RemoveDamage(d.DamageType, d.Value, 0f);
            }
        }

        public double GetDefence(DamageType damageType)
        {
            if(Defence.TryGet(damageType, out var value))
            {
                return value;
            }
            return 0;
        }


        public void AddDefence(DamageType damageType, double value)
        {
            if(Defence.TryGet(damageType, out var oldValue))
            {
                Defence.Set(damageType, oldValue + value);
            }
            else
            {
                Defence.Set(damageType, value);
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
            if(Defence.TryGet(damageType, out var oldValue))
            {
                Defence.Set(damageType, oldValue - value);
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