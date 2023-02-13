using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAbility : MonoBehaviour
{
    public Action OnChanged;
    public Action OnUsed;

    [SerializeField] GameObject abilityIcon;
    [SerializeField] SpriteRenderer abilityIconEffect;
    [SerializeField] SkillUsePanel skillUsePanel;

    [SerializeField]
    private int skillLevel = 1;

    [SerializeField]
    private int abilityPointAmountMax = 100;
    private int curAbilityPoint = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(IsFullPoint())
            {
                UseSkill();

                OnUsed?.Invoke();
                SetPoint(0);
            }
        }
    }

    private void UseSkill()
    {
        skillUsePanel.UseSkill();
        List<EnemyBase> detectEnemies = GameManager.Player.GetDetectEnemies();
        int repeatCount = Mathf.Min(detectEnemies.Count, 5);

        for (int i = 0; i < repeatCount; i++)
        {
            GameManager.Player.SpecialArrowAttack(detectEnemies[i], 1.5f);
        }
    }

    private void AbilityEffectAnimation()
    {
        abilityIconEffect.DOKill();
        abilityIconEffect.transform.DOKill();

        abilityIconEffect.color = Color.white;
        abilityIconEffect.transform.localScale = Vector3.one;

        abilityIconEffect.DOFade(0, 0.5f).SetLoops(-1);
        abilityIconEffect.transform.DOScale(2.25f, 0.5f).SetLoops(-1);
    }

    public bool IsFullPoint()
    {
        return curAbilityPoint == abilityPointAmountMax;
    }

    public float GetAbiltyPointAmountNormalized()
    {
        return (float)curAbilityPoint / abilityPointAmountMax;
    }

    public void SetPointAmountMax(int pointAmountMax, bool updatePointAmount)
    {
        abilityPointAmountMax = pointAmountMax;
        if (updatePointAmount)
        {
            curAbilityPoint = pointAmountMax;
        }
    }

    public void AddPoint(int pointAmount)
    {
        SetPoint(curAbilityPoint + pointAmount);
    }

    public void SetPoint(int settingPoint)
    {
        curAbilityPoint = settingPoint;
        curAbilityPoint = Mathf.Clamp(curAbilityPoint, 0, abilityPointAmountMax);
        OnChanged?.Invoke();

        if(curAbilityPoint == abilityPointAmountMax)
        {
            abilityIcon.SetActive(true);
            AbilityEffectAnimation();
        }
        else
        {
            abilityIcon.SetActive(false);
        }
    }
}
