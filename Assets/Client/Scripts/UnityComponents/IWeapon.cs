using UnityEngine;

namespace LD52
{
    public interface IWeapon
    {
        Transform transform {get;}
        Character Owner {get; set;}
        WeaponData Data {get; set;}
        void StartAttack();
        void EndAttack();
    }
}