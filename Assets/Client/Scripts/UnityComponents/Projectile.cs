using UnityEngine;

namespace LD52
{
    public class Projectile : MonoBehaviour
    {
        public float Speed;
        public float LifeTime;
        public Character Owner;
        private float _destroyIn;

        private void Start()
        {
            _destroyIn = LifeTime;
        }

        private void Update()
        {
            transform.position += transform.forward * (Speed * Time.deltaTime);
            _destroyIn -= Time.deltaTime;

            if(_destroyIn < 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {

        }
    }
}