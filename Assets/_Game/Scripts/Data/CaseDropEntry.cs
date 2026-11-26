using System;
using UnityEngine;

namespace CaseOpening.Data
{
    [Serializable]
    public sealed class CaseDropEntry
    {
        [SerializeField] private ItemData item;
        [SerializeField, Min(0f)] private float weight = 1f;

        public ItemData Item => item;
        public float Weight => weight;
    }
}
