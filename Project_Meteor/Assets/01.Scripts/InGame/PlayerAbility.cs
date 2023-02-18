using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerAbility : MonoBehaviour
{
    public Action OnChanged;
    public Action OnUsed;

    [SerializeField] GameObject abilityIcon;
    [SerializeField] SpriteRenderer abilityIconEffect;
    [SerializeField] SkillUsePanel skillUsePanel;
    [SerializeField] Image abilitySlash;

    [SerializeField]
    private int skillLevel = 1;

    [SerializeField]
    private int abilityPointAmountMax = 100;
    private int curAbilityPoint = 0;

    private Dictionary<int, float> damageScaleDic = new Dictionary<int, float>()
    {
        { 1 , 1.4f },
        { 2 , 1.55f },
        { 3 , 1.7f },
        { 4 , 2f },
    };

    private Dictionary<int, float> debuffAmountDic = new Dictionary<int, float>()
    {
        { 1 , 0.25f },
        { 2 , 0.30f },
        { 3 , 0.35f },
        { 4 , 0.40f },
    };

    private Dictionary<int, int> abilityMaxDic = new Dictionary<int, int>()
    {
        { 1 , 20 },
        { 2 , 19 },
        { 3 , 18 },
        { 4 , 15 },
    };

    private void Start()
    {
        Debug.LogFormat("{0}", 1.4f * 100);
        InGameUI.UI.Ability.SetAbilityUI(skillLevel, damageScaleDic[skillLevel] * 100, debuffAmountDic[skillLevel] * 100, false);

        SetPointAmountMax(abilityMaxDic[skillLevel], false);
    }

    private void Update()
    {
        if (!PlayerController.Interactable) return;

        if (Input.GetKeyDown(KeyCode.Space))
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
        Vector2Int detectDir = GameManager.Player.GetDetectDirection();
        bool isLeftDir = detectDir.x == -1 && detectDir.y == 0;
        skillUsePanel.UseSkill(!isLeftDir);

        List<EnemyBase> detectEnemies = GameManager.Player.GetDetectEnemies();
        int repeatCount = Mathf.Min(detectEnemies.Count, 5);

        for (int i = 0; i < repeatCount; i++)
        {
            GameManager.Player.SpecialArrowAttack(detectEnemies[i], damageScaleDic[skillLevel], debuffAmountDic[skillLevel]);
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

    private void SetPointAmountMax(int pointAmountMax, bool updatePointAmount)
    {
        abilityPointAmountMax = pointAmountMax;
        if (updatePointAmount)
        {
            curAbilityPoint = pointAmountMax;
        }
    }

    public void AddAbilityLevel()
    {
        SetAbilityLevel(skillLevel + 1);
    }

    private void SetAbilityLevel(int level)
    {
        skillLevel = level;
        SetPointAmountMax(abilityMaxDic[level], false);
        SetPoint(curAbilityPoint);

        InGameUI.UI.Ability.SetAbilityUI(skillLevel, damageScaleDic[skillLevel] * 100, debuffAmountDic[skillLevel] * 100, true);
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

        if(IsFullPoint())
        {
            abilityIcon.SetActive(true);
            AbilityEffectAnimation();

            abilitySlash.DOFade(1, 1f);
        }
        else
        {
            abilityIcon.SetActive(false);

            abilitySlash.DOKill();
            abilitySlash.DOFade(0, 0.25f);
        }
    }
}
