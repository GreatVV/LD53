using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.KCC;
using LeopotamGroup.Globals;
using UnityEngine;

namespace LD52
{
    public class Character : NetworkBehaviour
    {
        public event Action<Character> Dead;
        
        public Collider Collider;
        public Animator Animator;
        public KCC cc;
        public CharacterUI CharacterUI;
	    public bool TransformLocal = false;
        public float Speed;
	    public float lookTurnRate = 1.5f;
        public Pivots Pivots;
        [Space] [Header("Runtime data")]//How to sync it?
        private RuntimeAnimatorController RuntimeAnimatorController;
        public Characteristics Characteristics;
        public IWeapon Weapon;
        [Networked] public float LastAttackTime { get; set; }
        [Networked]
        public NetworkBool IsDead { get; set; }
        public bool IsStopped;
        [Networked] public float _heals { get; set; }
        public bool ReadyForAttack{get;set;}

        [Space] [Header("Start data")]
        public WeaponData WeaponData;
        public ItemsList DropList;

        public CharacteristicBonuses StartCharacteristics;
        
        public override void Spawned()
        {
            base.Spawned();
            
            Characteristics.Add(StartCharacteristics);
            var maxHeals = (float) Service<StaticData>.Get().Formulas.GetHeals(Characteristics);
            _heals = maxHeals;
            HealsChanged();
            RPC_EquipItem(WeaponData.Description.Id);

            if (Runner.LocalPlayer.IsValid)
            {
                var runtimeData = Service<RuntimeData>.Get();
                if (runtimeData.Inventory != default)
                {
                    runtimeData.Inventory = new Inventory()
                    {
                        Width = 10,
                        Height = 8
                    };
                }
            }
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if(Runner.IsServer)
            {
                if(!IsDead && ReadyForAttack)
                {
                    Weapon.RPC_StartAttack();
                    ReadyForAttack = false;
                }
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Attack()
        {
            if(IsDead) return;
            LastAttackTime = Runner.SimulationTime;
            Animator.SetTrigger(AnimationNames.Attack);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Respawn()
        {
            IsDead = false;
            Animator.Play(AnimationNames.Idle);
            Heals = MaxHeals;
            Collider.enabled = true;
            cc.enabled = true;
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Die()
        {
            if (IsDead)
            {
                return;
            }
            IsDead = true;
            Debug.Log($"{name} is dying");
            Animator.SetTrigger(AnimationNames.Death);
            Collider.enabled = false;
            cc.enabled = false;
            if(DropList != default)
            {
                SpawnDrop();
                
            }
            Dead?.Invoke(this);
        }

        private void SpawnDrop()
        {
            var prefab = Service<StaticData>.Get().DropPrefab;
            var drop = Runner.Spawn(prefab, transform.position);
            var item = DropList.GetRandomItem();
            drop.Show(item);
        }

        public void Kill(Character other)
        {
            var formulas = Service<StaticData>.Get().Formulas;
            Characteristics.Exp += formulas.GetExp(Characteristics.Level, other.Characteristics.Level);
            Characteristics.Level = formulas.Levels.GetLevel(Characteristics.Exp);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_EquipItem(string itemID)
        {
            var staticData = Service<StaticData>.Get();
            var item = staticData.AllItems.GetItemById(itemID);
            if(item != default)
            {
                if(item is WeaponData weapon)
                EquipItem(weapon);
            }
        }
        
        private void EquipItem(WeaponData item)
        {
            if(Weapon != default)
            {
                UnequipWeapon();
            }

            var parent = Pivots.Get(item.Pivot);
            var view =  Runner.Spawn(item.Description.Prefab, inputAuthority: Runner.LocalPlayer);
            view.transform.SetParent(parent);
            view.transform.localPosition = Vector3.zero;
            view.transform.localRotation = Quaternion.identity;
            view.transform.localScale = Vector3.one;

            Animator.runtimeAnimatorController = item.Animations;

            if(view.TryGetComponent<IWeapon>(out var weapon))
            {
                Weapon = weapon;
                weapon.Owner = this;
                weapon.Data = item;
            }

            Characteristics.AddDamage(item.Damage);
        }

        private void EquipItem(ArmorData item)
        {
            Characteristics.AddDefence(item.Defence);
        }

        private void UnequipWeapon()
        {
            Characteristics.RemoveDamage(Weapon.Data.Damage);
        }

        private void Unequip(ArmorData item)
        {
            Characteristics.RemoveDefence(item.Defence);
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_StartAttack()
        {
            ReadyForAttack = true;
//            Weapon.RPC_StartAttack();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_EndAttack()
        {
            Weapon.EndAttack();
        }

        private void HealsChanged()
        {
            CharacterUI.Refresh(this);
        }

        public float Heals
        {
            get => _heals;
            set
            {
                var oldValue = _heals;

                var newValue = Mathf.Clamp(value, 0f, MaxHeals);
                if(oldValue != newValue)
                {
                    //send event
                    _heals = newValue;
                    HealsChanged();
                    if(_heals == 0)
                    {
                        RPC_Die();
                    }
                }
            }
        }

        public float MaxHeals
        {
            get
            {
                return (float) Service<StaticData>.Get().Formulas.GetHeals(Characteristics);
            }
        }
    }
}