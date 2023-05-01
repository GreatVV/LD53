using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.KCC;
using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace LD52
{
    public class Character : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(ItemsChanged))] [Capacity(20)] public NetworkLinkedList<ItemDesc> Items { get; }
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


        [Networked(OnChanged = nameof(XpChanged))]
        public ref Characteristics Characteristics => ref MakeRef<LD52.Characteristics>();
        public IWeapon Weapon;
        [Networked] public float LastAttackTime { get; set; }
        [Networked(OnChanged = nameof(DeadChanged))]
        public NetworkBool IsDead { get; set; }
        public bool IsStopped;
        [Networked] public float _health { get; set; }
        [Networked(OnChanged = nameof(WeaponChanged))] public string ChosenWeapon { get; set; }

        public static void WeaponChanged(Changed<Character> changed)
        {
            
        }
        public static void DeadChanged(Changed<Character> changed)
        {
            if (changed.Behaviour.IsDead)
            {
                changed.Behaviour.Animator.SetTrigger(AnimationNames.Death);
                changed.Behaviour.Collider.enabled = false;
                changed.Behaviour.cc.enabled = false;
                changed.Behaviour.Dead?.Invoke(changed.Behaviour);
            }
        }
        
        public bool ReadyForAttack{get;set;}

        [Space] [Header("Start data")]
        public WeaponData WeaponData;
        public ItemsList DropList;

        public CharacteristicBonuses StartCharacteristics;

        public static void ItemsChanged( Changed<Character> changed)
        {
            if (changed.Behaviour.Runner.LocalPlayer && changed.Behaviour.HasInputAuthority)
            {
                Debug.Log("Items changed", changed.Behaviour.gameObject);
                Service<EcsWorld>.Get().NewEntity().Get<UpdateInventory>().value = changed.Behaviour.Items;
            }
        }

        public static void XpChanged(Changed<Character> changed)
        {
            var formulas = Service<StaticData>.Get().Formulas;
            var character = changed.Behaviour;
            var characteristics = character.Characteristics;
            characteristics.Level = formulas.Levels.GetLevel(characteristics.Exp);
            character.Characteristics = characteristics;
            Debug.Log("XP Changed");
        }
        
        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasStateAuthority)
            {
                Characteristics.Add(StartCharacteristics);
                var maxHealth = (float)Service<StaticData>.Get().Formulas.GetHeals(Characteristics);
                Debug.Log($"Max health: {maxHealth}");
                _health = maxHealth;
            }

            RPC_EquipItem(WeaponData.Description.Id);

            HealsChanged();
            
            if (Object.HasInputAuthority)
            {
                var runtimeData = Service<RuntimeData>.Get();
                Service<SceneData>.Get().CameraFollow.Target = transform;
                runtimeData.PlayerCharacter = this;
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
                //    Weapon.RPC_StartAttack();
                //    ReadyForAttack = false;
                }
            }
        }

        [Rpc]
        public void RPC_Attack()
        {
            if (IsDead)
            {
                return;
            }
            
            Animator.SetTrigger(AnimationNames.Attack);
            
            if (Object.HasStateAuthority)
            {
                LastAttackTime = Runner.SimulationTime;
                Weapon.StartAttack();
            }
        }

        [Rpc]
        public void RPC_Respawn()
        {
            IsDead = false;
            Animator.Play(AnimationNames.Idle);
            Health = MaxHeals;
            Collider.enabled = true;
            cc.enabled = true;
        }
        
        [Rpc]
        public void RPC_Die()
        {
            if (IsDead)
            {
                return;
            }

            if (HasStateAuthority)
            {
                IsDead = true;
                Debug.Log($"{name} is dying");
                if(DropList != default)
                {
                    SpawnDrop();
                }
            }
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
            var characteristics = Characteristics;
            characteristics.Exp += formulas.GetExp(characteristics.Level, other.Characteristics.Level);
            Characteristics = characteristics;
        }

        [Rpc]
        private void RPC_EquipItem(string itemID)
        {
            if (Runner.IsServer)
            {
                ChosenWeapon = itemID;

                var staticData = Service<StaticData>.Get();
                var item = staticData.AllItems.GetItemById(itemID);
                if (item != default)
                {
                    if (item is WeaponData weapon)
                    {
                        var view = Runner.Spawn(item.Description.Prefab, onBeforeSpawned: (r, o) =>
                        {
                            var w = o.GetComponent<Weapon>();
                            w.Owner = Object.Id;
                            w.DataID = itemID;
                          
                        });
                       
                        if (Weapon != default)
                        {
                            UnequipWeapon();
                        }
                        Characteristics.AddDamage(weapon.Damage);
                    }
                }
            }
        }
        
        private void EquipItem(ArmorData item)
        {
            Characteristics.AddDefence(item.Defence);
        }

        private void UnequipWeapon()
        {
            Characteristics.RemoveDamage(Weapon.GetData().Damage);
        }

        private void Unequip(ArmorData item)
        {
            Characteristics.RemoveDefence(item.Defence);
        }
        
    //    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_StartAttack()
        {
//            ReadyForAttack = true;
//            Weapon.RPC_StartAttack();
        }

    //    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_EndAttack()
        {
        //    Weapon.EndAttack();
        }

        private void HealsChanged()
        {
            CharacterUI.Refresh(this);
        }

        public float Health
        {
            get => _health;
            set
            {
                var oldValue = _health;
                var newValue = Mathf.Clamp(value, 0f, MaxHeals);
                if(oldValue != newValue)
                {
                    //send event
                    _health = newValue;
                    HealsChanged();
                    if(_health == 0)
                    {
                        RPC_Die();
                    }
                }
            }
        }

        public float MaxHeals => (float) Service<StaticData>.Get().Formulas.GetHeals(Characteristics);
    }

    public struct UpdateInventory
    {
        public NetworkLinkedList<ItemDesc> value;
    }

    [Serializable]
    public struct ItemDesc : INetworkStruct
    {
        [Networked]
        public int Id { get; set; }
        [Networked]
        public NetworkString<_32> ItemId { get; set; }
    }
    
}