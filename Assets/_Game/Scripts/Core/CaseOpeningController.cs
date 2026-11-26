using System;
using System.Collections.Generic;
using CaseOpening.Data;
using CaseOpening.Gameplay;
using CaseOpening.Interfaces;
using CaseOpening.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CaseOpening.Core
{
    public sealed class CaseOpeningController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private CaseData caseData;
        [SerializeField, Min(3)] private int rouletteItemCount = 45;
        [SerializeField, Min(0)] private int winnerIndex = 36;

        [Header("References")]
        [SerializeField] private RouletteController rouletteController;
        [SerializeField] private Button openButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private ResultPanelUI resultPanel;

        private IRandomSource randomSource;
        private CaseDropService dropService;
        private RouletteGenerator rouletteGenerator;
        private ItemData currentWinner;
        private bool opening;

        private void Awake()
        {
            randomSource = new UnityRandomSource();
            dropService = new CaseDropService(randomSource);
            rouletteGenerator = new RouletteGenerator(randomSource);

            if (rouletteController != null)
                rouletteController.OnSpinCompleted += HandleSpinCompleted;

            // Runtime listeners only. Do not wire Button OnClick in the Inspector.
            if (openButton != null) openButton.onClick.AddListener(OpenCase);
            if (skipButton != null) skipButton.onClick.AddListener(SkipOpening);
        }

        private void Start()
        {
            if (openButton != null) openButton.interactable = caseData != null;
            if (skipButton != null) skipButton.gameObject.SetActive(false);
            if (resultPanel != null) resultPanel.Hide();
        }

        public void OpenCase()
        {
            if (opening || caseData == null || rouletteController == null) return;

            try
            {
                opening = true;
                currentWinner = dropService.Roll(caseData);
                int safeWinnerIndex = Mathf.Clamp(winnerIndex, 1, rouletteItemCount - 2);
                IReadOnlyList<ItemData> sequence = rouletteGenerator.CreateSequence(
                    caseData, currentWinner, rouletteItemCount, safeWinnerIndex);

                rouletteController.Build(sequence, safeWinnerIndex);
                if (openButton != null) openButton.interactable = false;
                if (skipButton != null) skipButton.gameObject.SetActive(true);
                if (resultPanel != null) resultPanel.Hide();
                rouletteController.Play();
            }
            catch (Exception exception)
            {
                opening = false;
                Debug.LogException(exception);
                if (openButton != null) openButton.interactable = true;
                if (skipButton != null) skipButton.gameObject.SetActive(false);
            }
        }

        public void SkipOpening()
        {
            if (opening) rouletteController?.Skip();
        }

        private void HandleSpinCompleted()
        {
            opening = false;
            if (skipButton != null) skipButton.gameObject.SetActive(false);
            if (resultPanel != null) resultPanel.Show(currentWinner);
            if (openButton != null) openButton.interactable = true;
        }

        private void OnDestroy()
        {
            if (rouletteController != null)
                rouletteController.OnSpinCompleted -= HandleSpinCompleted;
            if (openButton != null) openButton.onClick.RemoveListener(OpenCase);
            if (skipButton != null) skipButton.onClick.RemoveListener(SkipOpening);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (rouletteController == null) rouletteController = GetComponentInChildren<RouletteController>(true);
            if (resultPanel == null) resultPanel = GetComponentInChildren<ResultPanelUI>(true);

            Button[] buttons = GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                string buttonName = button.name.ToLowerInvariant();
                if (openButton == null && buttonName.Contains("open")) openButton = button;
                if (skipButton == null && buttonName.Contains("skip")) skipButton = button;
            }

            rouletteItemCount = Mathf.Max(3, rouletteItemCount);
            winnerIndex = Mathf.Clamp(winnerIndex, 1, rouletteItemCount - 2);
        }
#endif
    }
}
