using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatHoverUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] Image iconImage;

    [SerializeField] TextMeshProUGUI hoverNameText;
    [SerializeField] TextMeshProUGUI hoverLoreText;

    [SerializeField] GameObject hoverUpgradeLayout;
    [SerializeField] TextMeshProUGUI hoverLevelText;
    [SerializeField] TextMeshProUGUI upgradeTreeText;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetHoverUI(Sprite icon, int level, string name, string lore, string beforeValue, string afterValue)
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 1;

        hoverUpgradeLayout.gameObject.SetActive(true);

        iconImage.sprite = icon;
        hoverNameText.text = name;
        hoverLoreText.text = lore;

        hoverLevelText.text = $"Lv.{level}";
        upgradeTreeText.text = $"{beforeValue} > <color=#FFCE00>{afterValue}</color>";

        UtilClass.ForceRefreshSize(transform);
    }

    public void SetHoverUIWithoutUpgrade(Sprite icon, string name, string lore)
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 1;

        hoverUpgradeLayout.gameObject.SetActive(false);

        iconImage.sprite = icon;
        hoverNameText.text = name;
        hoverLoreText.text = lore;

        UtilClass.ForceRefreshSize(transform);
    }

    public void PlaySetHoverAnimation(Color levelColor)
    {
        hoverNameText.transform.localScale = new Vector3(1.25f, 1.25f, 1);
        hoverNameText.transform.DOScale(1, 0.5f);

        hoverLevelText.color = levelColor;
        hoverLevelText.DOColor(Color.white, 0.5f);
    }

    public void ExitHoverUI()
    {
        canvasGroup.DOFade(0, 0.5f);
    }
}
