using CaseOpening.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseOpening.UI
{
    public sealed class ResultPanelUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panelGroup;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemName_value;
        [SerializeField] private TMP_Text itemRarity_value;
        [SerializeField] private TMP_Text itemPrice_value;
        [SerializeField] private RectTransform animatedContent;

        private Tween showTween;
        private Tween scaleTween;

        public void Show(ItemData item)
        {
            if (item == null) return;

            if (itemImage != null)
            {
                itemImage.sprite = item.Icon;
                itemImage.preserveAspect = true;
                itemImage.raycastTarget = false;
                itemImage.maskable = false;
            }
            if (itemName_value != null)
            {
                itemName_value.text = item.DisplayName;
                itemName_value.raycastTarget = false;
            }
            if (itemRarity_value != null)
            {
                itemRarity_value.text = item.Rarity.ToString();
                itemRarity_value.raycastTarget = false;
            }
            if (itemPrice_value != null)
            {
                itemPrice_value.text = item.MarketValue.ToString("F2") + " $";
                itemPrice_value.raycastTarget = false;
            }

            gameObject.SetActive(true);
            if (panelGroup != null)
            {
                panelGroup.alpha = 0f;
                panelGroup.interactable = true;
                panelGroup.blocksRaycasts = true;
                showTween?.Kill();
                showTween = panelGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);
            }
            if (animatedContent != null)
            {
                scaleTween?.Kill();
                animatedContent.localScale = Vector3.one * 0.85f;
                scaleTween = animatedContent.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
            }
        }

        public void Hide()
        {
            showTween?.Kill();
            scaleTween?.Kill();
            if (panelGroup != null)
            {
                panelGroup.alpha = 0f;
                panelGroup.interactable = false;
                panelGroup.blocksRaycasts = false;
            }
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            showTween?.Kill();
            scaleTween?.Kill();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (panelGroup == null) panelGroup = GetComponent<CanvasGroup>();
            if (itemImage == null) itemImage = transform.Find("ui_image_item")?.GetComponent<Image>();
            if (animatedContent == null) animatedContent = transform.Find("ui_panel_result_content") as RectTransform;
            if (itemName_value == null || itemRarity_value == null || itemPrice_value == null)
            {
                TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);
                foreach (TMP_Text text in texts)
                {
                    if (text.name.Contains("item_name")) itemName_value = text;
                    else if (text.name.Contains("item_rarity")) itemRarity_value = text;
                    else if (text.name.Contains("item_price")) itemPrice_value = text;
                }
            }
        }
#endif
    }
}
