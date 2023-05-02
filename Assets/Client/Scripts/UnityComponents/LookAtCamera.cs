using UnityEngine;

namespace LD52
{
    public class LookAtCamera : MonoBehaviour
    {
        private void Update()
        {
            if (Camera.main)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
        }
    }
}