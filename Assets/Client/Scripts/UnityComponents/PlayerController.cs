using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace LD52
{
    public class PlayerController : NetworkBehaviour
    {
        public Character Character;
        public float PickItemRadius;
        public float PickItemColdown;
        private TickTimer _pickItemTimer;
	    readonly FixedInput LocalInput = new FixedInput();
        private List<LagCompensatedHit> overlapResults = new List<LagCompensatedHit>();

        public override void FixedUpdateNetwork()
	    {
            if (Runner.Config.PhysicsEngine == NetworkProjectConfig.PhysicsEngines.None)
            {
                return;
            }

            if(Character.IsDead)
            {
                if (Runner.IsResimulation == false)
                {
                    LocalInput.Clear();
                }

                return;
            }

            TryPickItem();

            bool isAttack = false;

            Vector3 direction = default;
            if (GetInput(out NetworkInputPrototype input))
            {
                // BUTTON_WALK is representing left mouse button
                if (input.IsDown(NetworkInputPrototype.BUTTON_FIRE))
                {
                    direction = new Vector3(
                        Mathf.Cos((float)input.Yaw * Mathf.Deg2Rad),
                        0,
                        Mathf.Sin((float)input.Yaw * Mathf.Deg2Rad)
                    );
                    if(Character.LastAttackTime + Character.Weapon.Data.Coldown < Runner.SimulationTime)
                    {
                        Quaternion targetQ = Quaternion.AngleAxis(Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90, Vector3.down);
                        Character.cc.SetLookRotation(Quaternion.RotateTowards(transform.rotation, targetQ, 360f));//Character.lookTurnRate * 360 * Runner.DeltaTime));
            
                        Character.RPC_Attack();
                    }

                    isAttack = true;

                }
                else
                {
                    if (input.IsDown(NetworkInputPrototype.BUTTON_FORWARD))
                    {
                        direction += Character.TransformLocal ? transform.forward : Vector3.forward;
                    }

                    if (input.IsDown(NetworkInputPrototype.BUTTON_BACKWARD))
                    {
                        direction -= Character.TransformLocal ? transform.forward : Vector3.forward;
                    }

                    if (input.IsDown(NetworkInputPrototype.BUTTON_LEFT))
                    {
                        direction -= Character.TransformLocal ? transform.right : Vector3.right;
                    }

                    if (input.IsDown(NetworkInputPrototype.BUTTON_RIGHT))
                    {
                        direction += Character.TransformLocal ? transform.right : Vector3.right;
                    }

                    direction = direction.normalized;
                }
            }

            
            if(isAttack || Character.IsStopped)
            {
                Character.cc.SetInputDirection(Vector3.zero);
                Character.cc.SetKinematicVelocity(Vector3.zero);
                Character.Animator.SetFloat(AnimationNames.DirX, 0);
                Character.Animator.SetFloat(AnimationNames.DirY, 0);
                Character.Animator.SetFloat(AnimationNames.Speed, 0);
            }
            else
            {
                Character.cc.SetInputDirection(direction);
                Character.cc.SetKinematicVelocity(direction * Character.Speed);
                Character.Animator.SetFloat(AnimationNames.DirX, direction.x);
                Character.Animator.SetFloat(AnimationNames.DirY, direction.y);
                Character.Animator.SetFloat(AnimationNames.Speed, Character.Speed * direction.sqrMagnitude);
            }
            
            
            if (direction != Vector3.zero)
            {
                Quaternion targetQ = Quaternion.AngleAxis(Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90, Vector3.down);
                Character.cc.SetLookRotation(Quaternion.RotateTowards(transform.rotation, targetQ, Character.lookTurnRate * 360 * Runner.DeltaTime));
            }
            
        }

        private void TryPickItem()
        {
            if(Object.HasStateAuthority && _pickItemTimer.ExpiredOrNotRunning(Runner))
            {
                _pickItemTimer = TickTimer.CreateFromSeconds(Runner, PickItemColdown);
                
                var hits = Runner.LagCompensation.OverlapSphere(transform.position, PickItemRadius, Object.StateAuthority, overlapResults, options: HitOptions.IncludePhysX);
                
                for(var i = 0; i < hits; i++)
                {
                    var droppedItem = overlapResults[i].Collider.GetComponent<DroppedItem>();

                    if(droppedItem == default)
                    {
                        continue;
                    }

                    RPC_Pick(Character, droppedItem.Data.Description.Id);
                    Runner.Despawn(droppedItem.Object);
                }
            }
        }
        
        [Rpc]
        private void RPC_Pick(Character character, string droppedItem)
        {
            if (Runner.IsServer)
            {
                character.Items.Add(new ItemDesc()
                {
                    Id = QuestManager.UniversalId++,
                    ItemId = droppedItem
                });
            }
        }
    }
}