using System;
using Fusion;
using LeopotamGroup.Globals;
using UnityEngine;

namespace LD52
{
    public interface IWeapon
    {
        Transform transform {get;}
        [Networked]
        NetworkId Owner {get; set;}
        [Networked, Capacity(16)]
        string DataID {get; set;}
        void StartAttack();
        void EndAttack();
    }

    static class WeaponExtensions
    {
        public static WeaponData GetData(this IWeapon weapon)
        {
            if (weapon == default)
            {
                return default;
            }
            
            var staticData = Service<StaticData>.Get();
            var item = staticData.AllItems.GetItemById(weapon.DataID);
            return item as WeaponData;
        }
    }
}