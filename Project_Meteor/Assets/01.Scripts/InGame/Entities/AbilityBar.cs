using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBar : MonoBehaviour
{
    [SerializeField] PlayerAbility ability;
    [SerializeField] SpriteRenderer barImg;

    private Transform barTrm;

    private bool isUsingAbility = false;
    [SerializeField] Color abilityColor;
    [SerializeField] Color abilityUseColor;

    private void Awake()
    {
        barTrm = transform.Find("bar");
    }

    private void Start()
    {
        ability.OnChanged += CallAbilityPointOnChanged;
        ability.OnUsed += CallAbilityPointOnUsed;

        UpdateBar();
    }

    private void CallAbilityPointOnChanged()
    {
        UpdateBar();
    }

    private void CallAbilityPointOnUsed()
    {
        UseBar();
    }

    private void UpdateBar()
    {
        if (!isUsingAbility)
        {
            barTrm.localScale = new Vector3(ability.GetAbiltyPointAmountNormalized(), 1, 1);
        }
    }

    private void UseBar()
    {
        barTrm.DOKill();
        isUsingAbility = true;
        barImg.color = abilityUseColor;
        barTrm.DOScaleX(0, 0.75f).SetEase(Ease.Linear).OnComplete(() =>
        {
            isUsingAbility = false;
            barImg.color = abilityColor;
            UpdateBar();
        });
    }
}
