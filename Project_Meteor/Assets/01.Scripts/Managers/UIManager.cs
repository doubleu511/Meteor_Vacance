using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public enum UIFadeType
{
    FADE = 1,
    FLOAT = 2,
    //==================================
    OUT = 0,
    IN = FADE,
    FLOATOUT = FLOAT,
    FLOATIN = FADE | FLOAT
}

public class UIManager
{
    public void UIFade(CanvasGroup group, UIFadeType fadeType, float duration, bool setUpdate, UnityAction callback = null)
    {
        RectTransform rect = group.GetComponent<RectTransform>();

        bool _fade = UtilClass.IsIncludeFlag(fadeType, UIFadeType.FADE);
        bool _float = UtilClass.IsIncludeFlag(fadeType, UIFadeType.FLOAT);
        Vector2 floatPos = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - 100);

        if (_fade)
        {
            if (_float)
            {
                rect.anchoredPosition = floatPos;
                rect.DOAnchorPosY(100, duration).SetUpdate(setUpdate).SetEase(Ease.Linear);
            }

            group.DOFade(1, duration).SetUpdate(setUpdate).OnComplete(() =>
            {
                group.interactable = true;
                group.blocksRaycasts = true;

                if (callback != null)
                    callback.Invoke();
            });
        }
        else
        {
            if (_float)
            {
                rect.DOAnchorPos(floatPos, duration).SetUpdate(setUpdate).SetEase(Ease.Linear);
            }

            group.interactable = false;
            group.blocksRaycasts = false;

            group.DOFade(0, duration).SetUpdate(setUpdate).OnComplete(() =>
            {
                if (callback != null)
                    callback.Invoke();
            });
        }
    }

    public void UIFade(CanvasGroup group, bool fade)
    {
        group.alpha = fade ? 1 : 0;
        group.blocksRaycasts = fade;
        group.interactable = fade;
    }
}
