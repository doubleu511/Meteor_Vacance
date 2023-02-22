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

    [SerializeField] TitlePopupTabPanel albumPanel;
    [SerializeField] GameObject albumObject;
    [SerializeField] TitlePopupTabPanel settingPanel;
    [SerializeField] GameObject settingObject;

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
            albumPanel.SetPanel(false);
            settingPanel.SetPanel(false);
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

        albumPanel.SetPanel(true);
        OpenPopup(true, instant);
    }

    public void OpenSetting()
    {
        if (currentObject != null)
        {
            currentObject.SetActive(false);
        }

        settingObject.SetActive(true);
        currentObject = settingObject;

        settingPanel.SetPanel(true);
        OpenPopup(true, false);
    }

    private void OpenPopup(bool fade, bool instant)
    {
        popupGroup.DOKill();
        uiblur.DOKill();
        if (!instant)
        {
            if (fade)
            {
                popupGroup.interactable = true;
                popupGroup.blocksRaycasts = true;
                popupGroup.DOFade(1, 0.25f);
            }
            else
            {
                Global.UI.UIFade(popupGroup, UIFadeType.OUT, 0.25f, true);
            }
            DOTween.To(() => uiblur.Intensity, value => uiblur.Intensity = value, fade ? 1 : 0, 0.25f).SetUpdate(true);
        }
        else
        {
            Global.UI.UIFade(popupGroup, fade);
            uiblur.Intensity = fade ? 1 : 0;
        }
    }
}
