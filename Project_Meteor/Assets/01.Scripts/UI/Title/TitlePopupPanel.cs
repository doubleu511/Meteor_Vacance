using DG.Tweening;
using Krivodeling.UI.Effects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitlePopupPanel : MonoBehaviour
{
    private CanvasGroup popupGroup;
    [SerializeField] UIBlur uiblur;
    [SerializeField] Button prevBtn;

    [SerializeField] CanvasGroup albumPanel;
    [SerializeField] GameObject albumObject;
    private GameObject currentObject;

    private void Awake()
    {
        popupGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        prevBtn.onClick.AddListener(() =>
        {
            OpenPopup(false, false);
        });
    }

    public void OpenAlbum(bool instant)
    {
        if(currentObject != null)
        {
            currentObject.SetActive(false);
        }

        albumObject.SetActive(true);
        currentObject = albumObject;

        Global.UI.UIFade(albumPanel, true);

        OpenPopup(true, instant);
    }

    private void OpenPopup(bool fade, bool instant)
    {
        popupGroup.DOKill();
        uiblur.DOKill();
        if (!instant)
        {
            Global.UI.UIFade(popupGroup, fade ? UIFadeType.IN : UIFadeType.OUT, 0.25f, true);
            DOTween.To(() => uiblur.Intensity, value => uiblur.Intensity = value, fade ? 1 : 0, 0.25f).SetUpdate(true);
        }
        else
        {
            Global.UI.UIFade(popupGroup, fade);
            uiblur.Intensity = fade ? 1 : 0;
        }
    }
}
