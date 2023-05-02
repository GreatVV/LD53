using UnityEngine;

namespace LD52
{
    public class Boss : MonoBehaviour
    {
        public static Boss Instance;
        public Animator Animator;
        public void Start()
        {
            Instance = this;
        }

        public void PlayDeath()
        {
            Animator.SetTrigger("Death");
        }
    }
}