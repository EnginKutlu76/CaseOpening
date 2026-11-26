using CaseOpening.Interfaces;
using UnityEngine;

namespace CaseOpening.Core
{
    public sealed class UnityRandomSource : IRandomSource
    {
        public float Value => Random.value;
        public int Range(int minInclusive, int maxExclusive) => Random.Range(minInclusive, maxExclusive);
    }
}
