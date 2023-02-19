using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameFinalResultUI : MonoBehaviour
{
    private const string failDialog = "미안해, 이렇게 될 리 없었는데···";
    private const string star3Dialog = "잘했어. 최고야!";
    private const string star4Dialog = "어떤 난관이 기다리고 있다 해도, 우리가 함께라면 반드시 극복할 수 있을 거야.";

    private CanvasGroup canvasGroup;

    [SerializeField] Image charaImg;
    [SerializeField] CanvasGroup friendCanvasGroup;
    [SerializeField] TextMeshProUGUI friendText;
    [SerializeField] Image[] starImgs;
    [SerializeField] TextMeshProUGUI voiceText;
    [SerializeField] RectTransform voiceSizeFitter;
    [SerializeField] LootInfoUI commonLoot;
    [SerializeField] LootInfoUI specialLoot;

    [Header("Button")]
    [SerializeField] CanvasGroup failedBtnCanvasGroup;
    [SerializeField] CanvasGroup successBtnCanvasGroup;
    [SerializeField] Button restartBtn;
    [SerializeField] Button titleBtn;
    [SerializeField] Button nextBtn;
    private bool buttonReady = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        restartBtn.onClick.AddListener(() => Global.LoadScene.LoadScene(SceneManager.GetActiveScene().name));
    }

    private void Update()
    {
        if(buttonReady)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Global.LoadScene.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void ShowResult(int star)
    {
        StartCoroutine(PlayResult(star));
    }

    private IEnumerator PlayResult(int star)
    {
        friendText.text = "호감도 상승";
        switch (star)
        {
            case 0:
                voiceText.text = failDialog;
                Global.Sound.Play("SFX/Voice/fail", eSound.Voice);
                Global.Sound.Play("BGM/bat failed", eSound.Bgm);
                break;
            case 1:
            case 2:
            case 3:
                voiceText.text = star3Dialog;
                Global.Sound.Play("SFX/Voice/3star", eSound.Voice);
                Global.Sound.Play("BGM/victory", eSound.Bgm);
                break;
            case 4:
                voiceText.text = star4Dialog;
                friendText.text = "호감도 MAX";
                Global.Sound.Play("SFX/Voice/4star", eSound.Voice);
                Global.Sound.Play("BGM/victory", eSound.Bgm);
                break;
        }
        UtilClass.ForceRefreshSize(voiceSizeFitter.transform);
        canvasGroup.DOFade(1, 1f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.5f);
        charaImg.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < starImgs.Length; i++)
        {
            starImgs[i].color = new Color(starImgs[i].color.r, starImgs[i].color.g, starImgs[i].color.b, 0);
        }

        charaImg.DOFade(1, 0.5f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < star; i++)
        {
            starImgs[i].DOFade(1, 0.5f).SetUpdate(true);
            Global.Sound.Play("SFX/Battle/b_ui_star", eSound.Effect);
            yield return new WaitForSecondsRealtime(0.5f);
        }

        yield return new WaitForSecondsRealtime(0.25f);
        friendCanvasGroup.alpha = 0;
        friendCanvasGroup.DOFade(1, 0.5f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.75f);

        if(star > 0)
        {
            commonLoot.PlayAnimation();

            if (star == 4)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                specialLoot.PlayAnimation();
            }
        }
        yield return new WaitForSecondsRealtime(0.75f);


        if (star == 0)
        {
            Global.UI.UIFade(failedBtnCanvasGroup, UIFadeType.IN, 1f, true);
            buttonReady = true;
        }
        else Global.UI.UIFade(successBtnCanvasGroup, UIFadeType.IN, 1f, true);
    }
}
