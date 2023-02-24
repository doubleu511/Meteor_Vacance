using DG.Tweening;
using Krivodeling.UI.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] TopButtonPanel topButtonPanel;
    [SerializeField] UIBlur blurGroup;
    [SerializeField] CanvasGroup maskingGroup;

    [SerializeField] Button resumeBtn;
    [SerializeField] Button restartBtn;
    [SerializeField] Button titleBtn;

    [SerializeField] RectTransform continueText;

    private bool isFade = false;
    private int restartCount = 0;
    private int titleCount = 0;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        resumeBtn.onClick.AddListener(() => Setting(false));
        restartBtn.onClick.AddListener(() =>
        {
            ContinueCycle(restartBtn, ref restartCount, () => Global.LoadScene.LoadScene(SceneManager.GetActiveScene().name));
            titleCount = 0;
        });

        titleBtn.onClick.AddListener(() =>
        {
            ContinueCycle(titleBtn, ref titleCount, () => Global.LoadScene.LoadScene("TitleScene"));
            restartCount = 0;
        });
    }

    private void Update()
    {
        if (GameResultUI.isGameEnd) return;
        if (TutorialPanel.isTutorialOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isFade = !isFade;
            Setting(isFade);
        }
    }

    public void Setting(bool fade)
    {
        isFade = fade;

        topButtonPanel.PauseEvent(fade, false);

        canvasGroup.interactable = fade;
        canvasGroup.blocksRaycasts = fade;

        continueText.gameObject.SetActive(false);
        restartCount = 0;
        titleCount = 0;

        maskingGroup.DOComplete();
        if (fade)
        {
            Global.UI.UIFade(maskingGroup, UIFadeType.IN, 0.25f, true);
        }
        else
        {
            topButtonPanel.SkipBtnEvent(false);
            Global.UI.UIFade(maskingGroup, false);
        }
        DOTween.To(() => blurGroup.Intensity, value => blurGroup.Intensity = value, fade ? 1 : 0, 0.25f).SetUpdate(true);
    }

    private void ContinueCycle(Button button, ref int count, Action nextAction)
    {
        count++;
        Global.Sound.Play("SFX/Battle/b_ui_popup", eSound.Effect);
        if(count == 1)
        {
            continueText.gameObject.SetActive(true);
            continueText.anchoredPosition = new Vector2(button.targetGraphic.rectTransform.anchoredPosition.x, -415);
        }
        else if (count == 2)
        {
            nextAction?.Invoke();
        }
    }
}
