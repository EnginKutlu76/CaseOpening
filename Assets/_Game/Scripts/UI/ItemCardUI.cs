using CaseOpening.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseOpening.UI
{
    public sealed class ItemCardUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Image rarityBar;
        [SerializeField] private TMP_Text itemName_value;

        public void Bind(ItemData item)
        {
            if (item == null) return;

            if (itemImage != null)
            {
                itemImage.sprite = item.Icon;
                itemImage.preserveAspect = true;
                itemImage.raycastTarget = false;
                itemImage.maskable = true; // Required so RectMask2D can clip roulette cards.
            }

            if (itemName_value != null)
            {
                itemName_value.text = item.DisplayName;
                itemName_value.raycastTarget = false;
            }

            if (rarityBar != null)
            {
                rarityBar.color = GetRarityColor(item.Rarity);
                rarityBar.raycastTarget = false;
                rarityBar.maskable = true; // Required so RectMask2D can clip roulette cards.
            }
        }

        private static Color GetRarityColor(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Industrial: return new Color(0.25f, 0.45f, 0.95f);
                case ItemRarity.MilSpec: return new Color(0.28f, 0.48f, 0.95f);
                case ItemRarity.Restricted: return new Color(0.58f, 0.30f, 0.85f);
                case ItemRarity.Classified: return new Color(0.85f, 0.35f, 0.72f);
                case ItemRarity.Covert: return new Color(0.85f, 0.20f, 0.22f);
                case ItemRarity.Extraordinary: return new Color(0.90f, 0.68f, 0.18f);
                default: return new Color(0.65f, 0.68f, 0.72f);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (itemImage == null) itemImage = transform.Find("ui_image_item")?.GetComponent<Image>();
            if (rarityBar == null) rarityBar = transform.Find("ui_image_rarity")?.GetComponent<Image>();
            if (itemName_value == null) itemName_value = GetComponentInChildren<TMP_Text>(true);
        }
#endif
    }
}
