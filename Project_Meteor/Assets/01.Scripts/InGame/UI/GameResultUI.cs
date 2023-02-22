using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] CanvasGroup gameOverPanel;
    [SerializeField] CanvasGroup gameCompletePanel;
    [SerializeField] RectTransform completeMovePanel;

    [SerializeField] GameFinalResultUI finalResultUI;

    public static bool isGameEnd = false;
    private bool isDead = false;

    private void Start()
    {
        isGameEnd = false;
    }

    private void Update()
    {
        if (isDead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 결과화면으로(겜오버)
                gameOverPanel.alpha = 0;
                finalResultUI.ShowResult(0);
                isDead = false;
            }
        }
    }

    public void GameOver()
    {
        Global.Sound.Play("SFX/Battle/b_ui_lose", eSound.Effect);
        Global.UI.UIFade(gameOverPanel, UIFadeType.IN, 0.5f, true);
        isDead = true;
        isGameEnd = true;
    }

    public void GameComplete(int star)
    {
        isGameEnd = true;

        Time.timeScale = 1;
        Global.Sound.Play("SFX/Battle/b_ui_win", eSound.Effect);
        Global.UI.UIFade(gameCompletePanel, true);
        completeMovePanel.anchoredPosition = new Vector2(-1920, 0);

        Sequence seq = DOTween.Sequence();

        seq.Append(completeMovePanel.DOAnchorPosX(0, 0.5f));
        seq.AppendInterval(0.75f);
        seq.Append(completeMovePanel.DOAnchorPosX(1920, 0.5f));
        seq.Join(gameCompletePanel.DOFade(0, 0.5f).SetDelay(0.1f));
        seq.AppendInterval(1f);
        seq.AppendCallback(() => Time.timeScale = 0);
        seq.AppendCallback(() => finalResultUI.ShowResult(star));
        seq.SetUpdate(true);
    }
}
