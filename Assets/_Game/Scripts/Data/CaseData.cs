using System.Collections.Generic;
using UnityEngine;

namespace CaseOpening.Data
{
    [CreateAssetMenu(fileName = "CaseData", menuName = "Case Opening/Case")]
    public sealed class CaseData : ScriptableObject
    {
        [SerializeField] private string caseId;
        [SerializeField] private string displayName = "Glove Case";
        [SerializeField] private Sprite caseIcon;
        [SerializeField, Min(0f)] private float openingPrice;
        [SerializeField] private List<CaseDropEntry> drops = new List<CaseDropEntry>();

        public string CaseId => caseId;
        public string DisplayName => displayName;
        public Sprite CaseIcon => caseIcon;
        public float OpeningPrice => openingPrice;
        public IReadOnlyList<CaseDropEntry> Drops => drops;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(caseId))
                caseId = name.Trim().ToLowerInvariant().Replace(" ", "_");
            openingPrice = Mathf.Max(0f, openingPrice);
        }
#endif
    }
}
