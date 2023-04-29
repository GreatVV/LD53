using UnityEngine;

namespace LD52
{
    public class Pivots : MonoBehaviour
    {
        public Pivot[] Points;

        public Transform Get(PivotType type)
        {
            foreach(var p in Points)
            {
                if(p.PivotType.name == type.name)
                {
                    return p.Transform;
                }
            }

            Debug.LogWarning($"Pivot {type.name} in {name} not found");
            return transform;
        }
    }
}