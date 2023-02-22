using DG.Tweening;
using Krivodeling.UI.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleGameRequestUI : MonoBehaviour
{
    public static TitleGameRequestUI Request;
    private CanvasGroup canvasGroup;

    [SerializeField] UIBlur blurGroup;
    [SerializeField] CanvasGroup maskingGroup;

    [SerializeField] Button titleExitBtn;

    [SerializeField] Button leftBtn;
    [SerializeField] Button rightBtn;

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;

    private void Awake()
    {
        Request = this;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void RequestPopup(bool fade)
    {
        canvasGroup.interactable = fade;
        canvasGroup.blocksRaycasts = fade;

        maskingGroup.DOComplete();
        if (fade)
        {
            Global.UI.UIFade(maskingGroup, UIFadeType.IN, 0.25f, true);
        }
        else
        {
            Global.UI.UIFade(maskingGroup, false);
        }
        DOTween.To(() => blurGroup.Intensity, value => blurGroup.Intensity = value, fade ? 1 : 0, 0.25f).SetUpdate(true);
    }

    public void SetRequestAction(Action leftAction = null, Action rightAction = null)
    {
        leftBtn.onClick.RemoveAllListeners();
        rightBtn.onClick.RemoveAllListeners();

        leftBtn.onClick.AddListener(() => leftAction?.Invoke());
        rightBtn.onClick.AddListener(() => rightAction?.Invoke());
    }

    public void SetRequestText(string title, string left, string right)
    {
        titleText.text = title;
        leftText.text = left;
        rightText.text = right;
    }
}
