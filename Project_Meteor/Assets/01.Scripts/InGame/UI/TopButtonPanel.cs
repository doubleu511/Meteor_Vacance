using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopButtonPanel : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] Button settingBtn;
    [SerializeField] SettingUI settingUI;

    [Header("Skip")]
    [SerializeField] Button skipBtn;
    [SerializeField] TextMeshProUGUI skipText;
    [SerializeField] Image skipImage;
    [SerializeField] Sprite one_scale_spr;
    [SerializeField] Sprite two_scale_spr;
    private bool isSkipping = false;
    private int savedTimeScale = 1;

    [Header("Pause")]
    [SerializeField] Button pauseBtn;
    [SerializeField] CanvasGroup pausePanel;
    [SerializeField] Image pauseImage;
    [SerializeField] Sprite resume_spr;
    [SerializeField] Sprite pause_spr;
    private bool isPause = false;

    void Start()
    {
        settingBtn.onClick.AddListener(() => settingUI.Setting(true));
        skipBtn.onClick.AddListener(SkipBtnEvent);
        pauseBtn.onClick.AddListener(() => PauseEvent(true));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            SkipBtnEvent();
        }

        if(Input.GetKeyDown(KeyCode.F5))
        {
            PauseEvent(true);
        }
    }

    public void SetButtonActive(bool value)
    {
        settingBtn.interactable = value;
        skipBtn.interactable = value;
        pauseBtn.interactable = value;
    }

    public void SkipBtnEvent(bool value)
    {
        isSkipping = !value;
        SkipBtnEvent();
    }

    private void SkipBtnEvent()
    {
        isSkipping = !isSkipping;
        Global.Sound.Play("SFX/Battle/b_ui_popup", eSound.Effect);

        if (isSkipping)
        {
            if(!isPause) Time.timeScale = 2;
            savedTimeScale = 2;
            skipText.text = "2X";
            skipImage.sprite = two_scale_spr;
        }
        else
        {
            if (!isPause) Time.timeScale = 1;
            savedTimeScale = 1;
            skipText.text = "1X";
            skipImage.sprite = one_scale_spr;
        }
    }

    public void PauseEvent(bool value, bool panelInteract)
    {
        isPause = !value;
        PauseEvent(panelInteract);
    }

    private void PauseEvent(bool panelInteract)
    {
        isPause = !isPause;
        PlayerController.Interactable = !isPause;
        Global.Sound.Play("SFX/Battle/b_ui_popup", eSound.Effect);

        if (isPause)
        {
            Time.timeScale = 0;
            pauseImage.sprite = resume_spr;
            if (panelInteract) Global.UI.UIFade(pausePanel, isPause);
        }
        else
        {
            Time.timeScale = savedTimeScale;
            pauseImage.sprite = pause_spr;
            Global.UI.UIFade(pausePanel, false);
        }
    }
}
