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
    [SerializeField] Image iconEffect;
    [SerializeField] CanvasGroup startBtnGroup;

    [Header("Title")]
    [SerializeField] ParticleSystem lobbyParticle;
    [SerializeField] TitlePopupPanel popupPanel;
    [SerializeField] Button startBtn;
    [SerializeField] Button albumBtn;
    [SerializeField] Button settingBtn;

    [SerializeField] Button titleExitBtn;

    private void Start()
    {
        Global.Sound.Play("BGM/systitle2022", eSound.Bgm);

        startBtn.onClick.AddListener(() =>
        {
            DialogPanel.startActIndex = 0;
            Global.LoadScene.LoadScene("DialogScene");
        });

        albumBtn.onClick.AddListener(() =>
        {
            popupPanel.OpenAlbum(false);
        });

        settingBtn.onClick.AddListener(() =>
        {
            popupPanel.OpenSetting();
        });

        titleExitBtn.onClick.AddListener(() =>
        {
            TitleGameRequestUI.Request.SetRequestText("게임을 종료하시겠습니까?", "돌아가기", "게임 종료");
            TitleGameRequestUI.Request.SetRequestAction(() =>
                {
                    TitleGameRequestUI.Request.RequestPopup(false);
                },
                () =>
                {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            });
            TitleGameRequestUI.Request.RequestPopup(true);
        });

        if (TitleActBtn.isSimulateAct)
        {
            TitleActBtn.isSimulateAct = false;
            SkipLoadingEndOpenAlbum();
        }
        else
        {
            StartCoroutine(StartCo());
        }
    }

    private IEnumerator StartCo()
    {
        loadingPanelGroup.alpha = 1;
        Global.UI.UIFade(titlePanelGroup, false);
        Global.UI.UIFade(startBtnGroup, false);
        yield return new WaitForSeconds(4);
        loadingText.gameObject.SetActive(false);
        Global.UI.UIFade(startBtnGroup, UIFadeType.IN, 0.75f, true);
        AbilityEffectAnimation();
    }

    private void AbilityEffectAnimation()
    {
        iconEffect.DOKill();
        iconEffect.transform.DOKill();

        iconEffect.color = Color.white;
        iconEffect.transform.localScale = Vector3.one;

        iconEffect.DOFade(0, 1f).SetLoops(-1);
        iconEffect.transform.DOScale(2.25f, 1f).SetLoops(-1);
    }

    public void LobbyStart()
    {
        startBtnGroup.interactable = false;
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
        Global.UI.UIFade(loadingPanelGroup, false);
        Global.UI.UIFade(titlePanelGroup, true);
        popupPanel.OpenAlbum(true);
        lobbyParticle.Play();
    }

    [ContextMenu("ClearSave")]
    private void ClearSave()
    {
        SecurityPlayerPrefs.SetBool("NormalEndingClear", true);
        SecurityPlayerPrefs.SetBool("HappyEndingClear", true);
    }

    [ContextMenu("ResetSave")]
    private void ResetSave()
    {
        SecurityPlayerPrefs.DeleteKey("NormalEndingClear");
        SecurityPlayerPrefs.DeleteKey("HappyEndingClear");
    }
}
