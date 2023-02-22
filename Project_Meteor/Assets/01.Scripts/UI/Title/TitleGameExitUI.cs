using DG.Tweening;
using Krivodeling.UI.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.UI;

public class TitleGameExitUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] UIBlur blurGroup;
    [SerializeField] CanvasGroup maskingGroup;

    [SerializeField] Button titleExitBtn;

    [SerializeField] Button resumeBtn;
    [SerializeField] Button exitBtn;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        titleExitBtn.onClick.AddListener(() => GameExit(true));
        resumeBtn.onClick.AddListener(() => GameExit(false));

        exitBtn.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    public void GameExit(bool fade)
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
}
