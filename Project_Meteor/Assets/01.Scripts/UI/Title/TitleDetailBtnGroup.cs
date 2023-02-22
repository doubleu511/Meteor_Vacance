using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleDetailBtnGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetFade(bool value)
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = value ? 1 : 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(1, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, 1).SetDelay(2f);
    }
}
