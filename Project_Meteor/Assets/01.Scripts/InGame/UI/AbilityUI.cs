using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    private const string nameFormat = "쇄갑격·확산 Lv {0}";
    private const string loreFormat = "사용 즉시 <color=#FB9800>{0}%</color>의 공격력으로 범위 내 최대 5명의 적에게 공격하며, " +
                                        "5초 동안 목표의 방어력을 <color=#FB9800>{1}%</color> 감소시킨다.";

    [SerializeField] TextMeshProUGUI skillNameText;
    [SerializeField] TextMeshProUGUI skillLoreText;

    public void SetAbilityUI(int level, float attackPer, float armorBreakPer, bool animation)
    {
        skillNameText.text = string.Format(nameFormat, level);
        skillLoreText.text = string.Format(loreFormat, attackPer, armorBreakPer);

        if (animation)
        {
            skillNameText.transform.localScale = new Vector3(1.25f, 1.25f, 1);
            skillNameText.transform.DOScale(1, 0.5f);
        }
    }
}
