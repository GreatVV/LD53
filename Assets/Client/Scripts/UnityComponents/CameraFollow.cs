using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD52
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;

        private void LateUpdate()
        {
            if(Target != default)
            {
                transform.position = Target.position;
            }
        }
    }
}