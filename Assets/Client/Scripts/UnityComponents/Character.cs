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
        [SerializeField] private double _heals;

        [Space] [Header("Start data")]
        public WeaponData WeaponData;
        public ItemsList DropList;

        public CharacteristicBonuses StartCharacteristics;
        
        public override void Spawned()
        {
            base.Spawned();
            
            Characteristics.Add(StartCharacteristics);
            var maxHeals = Service<StaticData>.Get().Formulas.GetHeals(Characteristics);
            _heals = maxHeals;
            HealsChanged();
            EquipItem(WeaponData);

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

        public void Attack()
        {
            if(IsDead) return;
            LastAttackTime = Runner.SimulationTime;
            Animator.SetTrigger(AnimationNames.Attack);
        }

        public void Respawn()
        {
            IsDead = false;
            Animator.Play(AnimationNames.Idle);
            Heals = MaxHeals;
        }

        public void Die()
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

        private void EquipItem(WeaponData item)
        {
            if(Weapon != default)
            {
                UnequipWeapon();
            }

            var parent = Pivots.Get(item.Pivot);
            var view = Instantiate(item.Description.Prefab, parent);
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
            Weapon.StartAttack();
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

            Vector3 direction = default;
            if (!IsDead && GetInput(out NetworkInputPrototype input))
            {
                // BUTTON_WALK is representing left mouse button
                if (input.IsDown(NetworkInputPrototype.BUTTON_WALK))
                {
                    direction = new Vector3(
                        Mathf.Cos((float)input.Yaw * Mathf.Deg2Rad),
                        0,
                        Mathf.Sin((float)input.Yaw * Mathf.Deg2Rad)
                    );
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

                    if(input.IsDown(NetworkInputPrototype.BUTTON_FIRE))
                    {
                        if(LastAttackTime + Weapon.Data.Coldown < Runner.SimulationTime)
                        {
                            Attack();
                        }
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
            }

            cc.SetInputDirection(direction);
            cc.SetKinematicVelocity(direction * Speed);
            
            if (direction != Vector3.zero)
            {
                Quaternion targetQ = Quaternion.AngleAxis(Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90, Vector3.down);
                cc.SetLookRotation(Quaternion.RotateTowards(transform.rotation, targetQ, lookTurnRate * 360 * Runner.DeltaTime));
                Animator.SetFloat(AnimationNames.Speed, Speed);
            }
            else
            {
                Animator.SetFloat(AnimationNames.Speed, 0);
            }
            
            Animator.SetFloat(AnimationNames.DirX, direction.x);
            Animator.SetFloat(AnimationNames.DirY, direction.y);

            if (Runner.IsResimulation == false)
                LocalInput.Clear();
        }

        private void HealsChanged()
        {
            CharacterUI.Refresh(this);
        }

        public double Heals
        {
            get => _heals;
            set
            {
                var oldValue = _heals;

                var newValue = System.Math.Clamp(value, 0d, MaxHeals);
                if(oldValue != newValue)
                {
                    //send event
                    _heals = newValue;
                    HealsChanged();
                    if(_heals == 0)
                    {
                        Die();
                    }
                }
            }
        }

        public double MaxHeals
        {
            get
            {
                return Service<StaticData>.Get().Formulas.GetHeals(Characteristics);
            }
        }
    }
}