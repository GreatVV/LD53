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
	    readonly FixedInput LocalInput = new FixedInput();
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

            if (Runner.LocalPlayer)
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

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_Attack()
        {
            if(IsDead) return;
            LastAttackTime = Runner.SimulationTime;
            Animator.SetTrigger(AnimationNames.Attack);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_Respawn()
        {
            IsDead = false;
            Animator.Play(AnimationNames.Idle);
            Heals = MaxHeals;
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
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

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
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
        
        public void StartAttack()
        {
            Weapon.RPC_StartAttack();
        }

        public void EndAttack()
        {
            Weapon.EndAttack();
        }
        
        public override void FixedUpdateNetwork()
	    {
            if (Runner.Config.PhysicsEngine == NetworkProjectConfig.PhysicsEngines.None)
            {
                return;
            }

            bool isAttack = false;

            Vector3 direction = default;
            if (!IsDead && GetInput(out NetworkInputPrototype input))
            {
                // BUTTON_WALK is representing left mouse button
                if (input.IsDown(NetworkInputPrototype.BUTTON_FIRE))
                {
                    direction = new Vector3(
                        Mathf.Cos((float)input.Yaw * Mathf.Deg2Rad),
                        0,
                        Mathf.Sin((float)input.Yaw * Mathf.Deg2Rad)
                    );
                    if(LastAttackTime + Weapon.Data.Coldown < Runner.SimulationTime)
                    {
                        RPC_Attack();
                    }

                    isAttack = true;

                }
                else
                {
                    if (input.IsDown(NetworkInputPrototype.BUTTON_FORWARD))
                    {
                        direction += TransformLocal ? transform.forward : Vector3.forward;
                    }

                    if (input.IsDown(NetworkInputPrototype.BUTTON_BACKWARD))
                    {
                        direction -= TransformLocal ? transform.forward : Vector3.forward;
                    }

                    if (input.IsDown(NetworkInputPrototype.BUTTON_LEFT))
                    {
                        direction -= TransformLocal ? transform.right : Vector3.right;
                    }

                    if (input.IsDown(NetworkInputPrototype.BUTTON_RIGHT))
                    {
                        direction += TransformLocal ? transform.right : Vector3.right;
                    }

                   

                    direction = direction.normalized;
                }

                if (Object.HasInputAuthority && Runner.IsResimulation == false)
                {
                    if (LocalInput.GetDown(KeyCode.E))
                    {
                     //   TryKill();
                     //   TryUse(true);
                     //   TryUse(false);
                    }
                }

                if(!IsDead && ReadyForAttack)
                {
                    Weapon.RPC_StartAttack();
                    ReadyForAttack = false;
                }
            }

            
            if(isAttack || IsStopped)
            {
                cc.SetInputDirection(Vector3.zero);
                cc.SetKinematicVelocity(Vector3.zero);
                Animator.SetFloat(AnimationNames.DirX, 0);
                Animator.SetFloat(AnimationNames.DirY, 0);
                Animator.SetFloat(AnimationNames.Speed, 0);
            }
            else
            {
                cc.SetInputDirection(direction);
                cc.SetKinematicVelocity(direction * Speed);
                Animator.SetFloat(AnimationNames.DirX, direction.x);
                Animator.SetFloat(AnimationNames.DirY, direction.y);
                Animator.SetFloat(AnimationNames.Speed, Speed * direction.sqrMagnitude);
            }
            
            
            if (direction != Vector3.zero)
            {
                Quaternion targetQ = Quaternion.AngleAxis(Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90, Vector3.down);
                cc.SetLookRotation(Quaternion.RotateTowards(transform.rotation, targetQ, lookTurnRate * 360 * Runner.DeltaTime));
            }
            

            if (Runner.IsResimulation == false)
                LocalInput.Clear();
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