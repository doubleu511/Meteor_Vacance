using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class InfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyKilledText;
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] CanvasGroup redBlur;

    private void Awake()
    {
        InGameUI.Info = this;
    }

    public void ShowRedBlur()
    {
        redBlur.DOKill();
        redBlur.alpha = 0;
        redBlur.DOFade(1, 0.75f).OnComplete(() =>
        {
            redBlur.DOFade(0, 0.75f);
        });
    }

    public void SetEnemyKilledText(int value)
    {
        enemyKilledText.text = $"{value}";
    }

    public void SetPlayerHeathText(int value)
    {
        playerHealthText.text = $"{value}";
    }
}
