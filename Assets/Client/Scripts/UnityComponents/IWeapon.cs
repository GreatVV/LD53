using UnityEngine;

namespace LD52
{
    public interface IWeapon
    {
        Transform transform {get;}
        Character Owner {get; set;}
        WeaponData Data {get; set;}
        void RPC_StartAttack();
        void EndAttack();
    }
}