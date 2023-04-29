﻿using UnityEngine;

namespace LD52
{
    public static class TransformExtension
    {
        public static void DestroyChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
            
        }
    }
}