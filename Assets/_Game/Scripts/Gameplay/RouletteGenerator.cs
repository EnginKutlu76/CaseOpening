using System;
using System.Collections.Generic;
using CaseOpening.Data;
using CaseOpening.Interfaces;

namespace CaseOpening.Gameplay
{
    // Creates presentation-only filler entries. The actual winner comes from CaseDropService.
    public sealed class RouletteGenerator
    {
        private readonly IRandomSource random;

        public RouletteGenerator(IRandomSource randomSource)
        {
            random = randomSource ?? throw new ArgumentNullException(nameof(randomSource));
        }

        public IReadOnlyList<ItemData> CreateSequence(CaseData caseData, ItemData winner, int count, int winnerIndex)
        {
            if (caseData == null) throw new ArgumentNullException(nameof(caseData));
            if (winner == null) throw new ArgumentNullException(nameof(winner));
            if (count < 3) throw new ArgumentOutOfRangeException(nameof(count));
            if (winnerIndex < 0 || winnerIndex >= count) throw new ArgumentOutOfRangeException(nameof(winnerIndex));

            var validItems = new List<ItemData>();
            foreach (CaseDropEntry entry in caseData.Drops)
            {
                if (entry != null && entry.Item != null && entry.Weight > 0f)
                    validItems.Add(entry.Item);
            }

            if (validItems.Count == 0)
                throw new InvalidOperationException("Case has no valid items for roulette presentation.");

            var sequence = new List<ItemData>(count);
            for (int i = 0; i < count; i++)
                sequence.Add(i == winnerIndex ? winner : validItems[random.Range(0, validItems.Count)]);

            return sequence;
        }
    }
}
