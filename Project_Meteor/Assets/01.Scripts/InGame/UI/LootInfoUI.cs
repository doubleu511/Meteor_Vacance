using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LootInfoUI : MonoBehaviour
{
    [SerializeField] CanvasGroup lootCanvasGroup;
    [SerializeField] Image bandRect;
    [SerializeField] TextMeshProUGUI bandText;

    [SerializeField] CanvasGroup lootInfoCanvasGroup;

    public void PlayAnimation()
    {
        lootCanvasGroup.DOComplete();
        bandRect.rectTransform.DOComplete();
        bandText.DOComplete();
        lootInfoCanvasGroup.DOComplete();

        lootCanvasGroup.alpha = 0;
        lootInfoCanvasGroup.alpha = 0;
        bandRect.rectTransform.anchoredPosition = new Vector2(0, 140);
        bandRect.rectTransform.sizeDelta = new Vector2(bandRect.rectTransform.sizeDelta.x, 40);
        bandText.color = Color.white;

        Color bandTextColor = bandRect.color;
        bandTextColor.a = 1;

        Sequence seq = DOTween.Sequence();
        seq.Append(lootCanvasGroup.DOFade(1, 0.5f));
        seq.Join(bandRect.rectTransform.DOAnchorPosY(0, 0.5f).SetEase(Ease.Linear));
        seq.Join(bandRect.rectTransform.DOSizeDelta(new Vector2(bandRect.rectTransform.sizeDelta.x, 2), 0.1f).SetDelay(0.35f).SetEase(Ease.Linear));
        seq.Join(bandText.DOColor(bandTextColor, 0.1f));
        seq.Append(lootInfoCanvasGroup.DOFade(1, 1));
        seq.SetUpdate(true);
    }
}
