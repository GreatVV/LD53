using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace LD52
{
    [Serializable]
    public struct Characteristics : INetworkStruct
    {
        public int Exp;

        public int Level;

        [Networked, Capacity(6)] public NetworkDictionary<int, CharacteristicBonus> Values => default;

        [Networked, Capacity(6)] public NetworkDictionary<int, CharacteristicBonus> Damage => default;

        [Networked, Capacity(5)] public NetworkDictionary<int, double> Defence => default;
        
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
                    Values.Add(bonus.Type, bonus);
                }
                Debug.Log($"Set {bonus.Type} to {bonus}");
            }
            Print();
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

        public double GetCharacteristic(in CharacteristicType characteristic)
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
                
                Damage.Add(damageType, v);
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

        public void Print()
        {
            Debug.Log($"Count: {Values.Count}");
            foreach (var keyValuePair in Values)
            {
                Debug.Log($"VV: {keyValuePair.Key}=> {keyValuePair.Value.Value} {keyValuePair.Value.Multipler} {keyValuePair.Value.Type}");
            }
        }
    }
}