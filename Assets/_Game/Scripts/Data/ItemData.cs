using UnityEngine;

namespace CaseOpening.Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Case Opening/Item")]
    public sealed class ItemData : ScriptableObject
    {
        [SerializeField] private string itemId;
        [SerializeField] private string displayName;
        [SerializeField] private ItemRarity rarity;
        [SerializeField] private Sprite icon;
        [SerializeField, Min(0f)] private float marketValue;

        public string ItemId => itemId;
        public string DisplayName => displayName;
        public ItemRarity Rarity => rarity;
        public Sprite Icon => icon;
        public float MarketValue => marketValue;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(itemId))
                itemId = name.Trim().ToLowerInvariant().Replace(" ", "_");
            if (string.IsNullOrWhiteSpace(displayName)) displayName = name;
            marketValue = Mathf.Max(0f, marketValue);
        }
#endif
    }
}
