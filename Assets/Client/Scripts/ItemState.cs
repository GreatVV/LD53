﻿using System;
using UnityEngine;

namespace LD52
{
    [Serializable]
    public class ItemState
    {
        public int NetworkSyncId;
        public ItemDescription ItemDescription;
        public int Amount;
        public Vector2Int Position;
        public bool CanBeDropped;

        public string GetDescription()
        {
            return $"{ItemDescription.LocalizedName}";
        }
    }
}