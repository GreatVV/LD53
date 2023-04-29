using System.Collections;
using LeopotamGroup.Globals;
using UnityEngine;

namespace LD52
{
    public class Character : MonoBehaviour
    {
        public Collider Collider;
        public Animator Animator;
        public Pivots Pivots;
        [Space] [Header("Runtime data")]//How to sync it?
        private RuntimeAnimatorController RuntimeAnimatorController;
        public Characteristics Characteristics;
        public IWeapon Weapon;
        [SerializeField] private double _heals;
        [SerializeField] private bool _isDead;

        [Space] [Header("Start data")]
        public WeaponData WeaponData;
        public CharacteristicBonuses StartCharacteristics;

        private IEnumerator Start()
        {
            yield return default;
            Characteristics = new Characteristics();
            Characteristics.Add(StartCharacteristics);
            var maxHeals = Service<StaticData>.Get().Formulas.GetHeals(Characteristics);
            _heals = maxHeals;
            EquipItem(WeaponData);
        }
        
        public void Attack()
        {
            if(_isDead) return;

            Animator.SetTrigger(AnimationNames.Attack);
        }

        public void Dead()
        {
            Debug.Log($"{name} is dying");
            Animator.SetTrigger(AnimationNames.Death);
        }

        private void EquipItem(WeaponData item)
        {
            if(Weapon != default)
            {
                UnequipWeapon();
            }

            var parent = Pivots.Get(item.Pivot);
            var view = Instantiate(item.Prefab, parent);
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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
        }

        public double Heals
        {
            get => _heals;
            set
            {
                var oldValue = _heals;
                
                var maxHeals = Service<StaticData>.Get().Formulas.GetHeals(Characteristics);

                var newValue = System.Math.Clamp(value, 0d, maxHeals);
                if(oldValue != newValue)
                {
                    //send event
                    _heals = newValue;
                    if(_heals == 0)
                    {
                        Dead();
                    }
                }
            }
        }
    }
}