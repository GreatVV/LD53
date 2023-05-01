using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace LD52
{
    public class Projectile : NetworkBehaviour
    {
        public float Speed;
        public float LifeTime;
        public NetworkId Owner;
        public float Radius;
        public Transform CheckForImpactPoint;
        public Weapon Weapon {get;set;}
        public NetworkObject networkObject;

        TickTimer lifeEndTimer;
        private List<LagCompensatedHit> overlapResults = new List<LagCompensatedHit>();

        public void Fire()
        {
            lifeEndTimer = TickTimer.CreateFromSeconds(Runner, LifeTime);
        }

        public override void FixedUpdateNetwork()
        {
            transform.position += transform.forward * (Speed *  Runner.DeltaTime);

            if(Object.HasStateAuthority)
            {
                if(lifeEndTimer.Expired(Runner))
                {
                    Runner.Despawn(networkObject);
                    return;
                }

                var hits = Runner.LagCompensation.OverlapSphere(CheckForImpactPoint.position, Radius, Object.StateAuthority, overlapResults, options: HitOptions.IncludePhysX);
                for(var i = 0; i < hits; i++)
                {
                    if(overlapResults[i].Hitbox != default)
                    {
                        var otherCharacter = overlapResults[i].Hitbox.Root.GetBehaviour<Character>();
                        if(otherCharacter == default || otherCharacter == Owner)
                        {
                            continue;
                        }
                        
                        
                        var owner = Runner.FindObject(Owner).GetComponent<Character>();
                        DamageHelper.SendDamage(owner, otherCharacter, Weapon.GetData());
                        Runner.Despawn(networkObject);
                        return;
                        
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Runner.IsServer)
            {
                var owner = Runner.FindObject(Owner).GetComponent<Character>();
                DamageHelper.SendDamage(owner, other, Weapon.GetData());
            }
        }
    }
}