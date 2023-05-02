using System;
using Fusion;
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
}