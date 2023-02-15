using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class StatUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI attackValueText;
    [SerializeField] TextMeshProUGUI attackSpeedValueText;

    private void Start()
    {
        UtilClass.ForceRefreshSize(transform);
    }

    public void SetAttackValue(int attack)
    {
        attackValueText.text = $"{attack}";
        attackValueText.transform.DOKill();
        attackValueText.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        attackValueText.transform.DOScale(1, 0.5f);

        UtilClass.ForceRefreshSize(transform);
    }

    public void SetAttackSpeedValue(float attackSpeed)
    {
        attackSpeedValueText.text = $"{attackSpeed}";
        attackSpeedValueText.transform.DOKill();
        attackSpeedValueText.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        attackSpeedValueText.transform.DOScale(1, 0.5f);

        UtilClass.ForceRefreshSize(transform);
    }
}
