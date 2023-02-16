using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    private const string nameFormat = "�Ⱙ�ݡ�Ȯ�� Lv {0}";
    private const string loreFormat = "��� ��� <color=#FB9800>{0}%</color>�� ���ݷ����� ���� �� �ִ� 5���� ������ �����ϸ�, " +
                                        "5�� ���� ��ǥ�� ������ <color=#FB9800>{1}%</color> ���ҽ�Ų��.";

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
