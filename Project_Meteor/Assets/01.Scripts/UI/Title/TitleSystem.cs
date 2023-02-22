using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSystem : MonoBehaviour
{
    [SerializeField] CanvasGroup loadingPanelGroup;
    [SerializeField] CanvasGroup titlePanelGroup;
    [SerializeField] CanvasGroup blackScreen;

    [Header("Loading")]
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] CanvasGroup startBtn;

    [Header("Title")]
    [SerializeField] ParticleSystem lobbyParticle;
    [SerializeField] TitlePopupPanel popupPanel;
    [SerializeField] Button albumBtn;

    private void Start()
    {
        StartCoroutine(StartCo());
        Global.Sound.Play("BGM/systitle2022", eSound.Bgm);

        albumBtn.onClick.AddListener(() =>
        {
            popupPanel.OpenAlbum();
        });
    }

    private IEnumerator StartCo()
    {
        loadingPanelGroup.alpha = 1;
        Global.UI.UIFade(titlePanelGroup, false);
        Global.UI.UIFade(startBtn, false);
        yield return new WaitForSeconds(4);
        loadingText.gameObject.SetActive(false);
        Global.UI.UIFade(startBtn, UIFadeType.IN, 0.75f, true);
    }

    public void LobbyStart()
    {
        startBtn.interactable = false;
        StartCoroutine(BlackLoading());
    }

    private IEnumerator BlackLoading()
    {
        Global.UI.UIFade(blackScreen, UIFadeType.IN, 1f, true);
        yield return new WaitForSeconds(1);
        Global.UI.UIFade(loadingPanelGroup, false);
        Global.UI.UIFade(titlePanelGroup, true);
        yield return new WaitForSeconds(1);
        Global.UI.UIFade(blackScreen, UIFadeType.OUT, 1f, true);
        lobbyParticle.Play();
    }

    private void SkipLoadingEndOpenAlbum()
    {

    }
}
