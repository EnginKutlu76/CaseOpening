using System;
using System.Collections.Generic;
using CaseOpening.Data;
using DG.Tweening;
using UnityEngine;

namespace CaseOpening.UI
{
    public sealed class RouletteController : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private RectTransform content;
        [SerializeField] private ItemCardUI itemCardPrefab;
        [SerializeField] private RectTransform viewport;
        [SerializeField, Min(80f)] private float cardWidth = 180f;
        [SerializeField, Min(0f)] private float spacing = 12f;

        [Header("Animation")]
        [SerializeField, Min(1f)] private float spinDuration = 7f;
        [SerializeField] private Ease spinEase = Ease.OutCubic;

        private readonly List<ItemCardUI> spawnedCards = new List<ItemCardUI>();
        private Tween activeTween;
        private float targetX;
        private bool isSpinning;

        public event Action OnSpinCompleted;
        public bool IsSpinning => isSpinning;

        public void Build(IReadOnlyList<ItemData> items, int winnerIndex)
        {
            if (content == null || viewport == null || itemCardPrefab == null)
                throw new InvalidOperationException("Roulette references are not assigned.");
            if (items == null || items.Count == 0)
                throw new ArgumentException("Roulette items are required.", nameof(items));
            if (winnerIndex < 0 || winnerIndex >= items.Count)
                throw new ArgumentOutOfRangeException(nameof(winnerIndex));

            ClearCards();
            float stride = cardWidth + spacing;

            for (int i = 0; i < items.Count; i++)
            {
                ItemCardUI card = Instantiate(itemCardPrefab, content);
                card.Bind(items[i]);

                RectTransform rect = (RectTransform)card.transform;
                rect.anchorMin = rect.anchorMax = new Vector2(0f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(cardWidth, rect.sizeDelta.y);
                rect.anchoredPosition = new Vector2(i * stride + cardWidth * 0.5f, 0f);
                spawnedCards.Add(card);
            }

            content.anchorMin = new Vector2(0f, 0.5f);
            content.anchorMax = new Vector2(0f, 0.5f);
            content.pivot = new Vector2(0f, 0.5f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(items.Count * stride, content.sizeDelta.y);

            float viewportCenter = viewport.rect.width * 0.5f;
            float winnerCenter = winnerIndex * stride + cardWidth * 0.5f;
            targetX = viewportCenter - winnerCenter;
        }

        public void Play()
        {
            if (isSpinning || spawnedCards.Count == 0) return;

            isSpinning = true;
            activeTween?.Kill();
            activeTween = content.DOAnchorPosX(targetX, spinDuration)
                .SetEase(spinEase)
                .OnComplete(CompleteSpin);
        }

        public void Skip()
        {
            if (!isSpinning) return;
            activeTween?.Kill();
            content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
            CompleteSpin();
        }

        private void CompleteSpin()
        {
            isSpinning = false;
            OnSpinCompleted?.Invoke();
        }

        private void ClearCards()
        {
            activeTween?.Kill();
            isSpinning = false;
            for (int i = 0; i < spawnedCards.Count; i++)
                if (spawnedCards[i] != null) Destroy(spawnedCards[i].gameObject);
            spawnedCards.Clear();
        }

        private void OnDestroy()
        {
            activeTween?.Kill();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (content == null) content = transform.Find("ui_panel_roulette_viewport/ui_panel_roulette_content") as RectTransform;
            if (viewport == null) viewport = transform.Find("ui_panel_roulette_viewport") as RectTransform;
            cardWidth = Mathf.Max(80f, cardWidth);
            spinDuration = Mathf.Max(1f, spinDuration);
        }
#endif
    }
}
