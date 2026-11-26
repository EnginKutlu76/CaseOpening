using System;
using CaseOpening.Data;
using CaseOpening.Interfaces;

namespace CaseOpening.Gameplay
{
    // Pure gameplay service: does not depend on MonoBehaviour, UI or animation.
    public sealed class CaseDropService
    {
        private readonly IRandomSource random;

        public CaseDropService(IRandomSource randomSource)
        {
            random = randomSource ?? throw new ArgumentNullException(nameof(randomSource));
        }

        public ItemData Roll(CaseData caseData)
        {
            if (caseData == null) throw new ArgumentNullException(nameof(caseData));
            var drops = caseData.Drops;
            if (drops == null || drops.Count == 0)
                throw new InvalidOperationException("Case has no drop entries.");

            float totalWeight = 0f;
            for (int i = 0; i < drops.Count; i++)
            {
                CaseDropEntry entry = drops[i];
                if (entry == null || entry.Item == null || entry.Weight <= 0f) continue;
                totalWeight += entry.Weight;
            }

            if (totalWeight <= 0f)
                throw new InvalidOperationException("Case has no valid weighted drops.");

            float roll = random.Value * totalWeight;
            float cumulative = 0f;
            for (int i = 0; i < drops.Count; i++)
            {
                CaseDropEntry entry = drops[i];
                if (entry == null || entry.Item == null || entry.Weight <= 0f) continue;
                cumulative += entry.Weight;
                if (roll < cumulative) return entry.Item;
            }

            // Protects against floating-point edge cases when random.Value is exactly 1.
            for (int i = drops.Count - 1; i >= 0; i--)
            {
                CaseDropEntry entry = drops[i];
                if (entry != null && entry.Item != null && entry.Weight > 0f)
                    return entry.Item;
            }

            throw new InvalidOperationException("Unable to resolve case drop.");
        }
    }
}
