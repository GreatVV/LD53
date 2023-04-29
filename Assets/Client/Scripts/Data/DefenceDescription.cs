using UnityEngine;

namespace LD52
{
    [System.Serializable]
    public struct DefenceDescription
    {
        [Range(0f, 0.9f)]
        public float DefencePercent;
        public DamageType DamageType;
    }
}