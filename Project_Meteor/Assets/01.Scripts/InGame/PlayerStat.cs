using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float playerAttackSpeed = 1.0f;
    public float playerDamage = 40;
    public float attackWaitTime { get; set; }

    [SerializeField] StatTreeSO[] statTrees;
    [SerializeField] SkillTreeUI[] statTreeUIs;

    private Dictionary<StatType, StatTreeSO> statTreeDic = new Dictionary<StatType, StatTreeSO>();
    private Dictionary<StatType, SkillTreeUI> statTreeUIDic = new Dictionary<StatType, SkillTreeUI>();
    private Dictionary<StatType, int> statLevelDic = new Dictionary<StatType, int>()
    {
        {StatType.ATTACK, 1},
        {StatType.ATTACK_SPEED, 1},
        {StatType.SKILL_LEVEL, 1},
        {StatType.HEAL, 1},
    };

    private void Start()
    {
        for (int i = 0; i < statTrees.Length; i++)
        {
            statTreeDic[statTrees[i].statType] = statTrees[i];
        }

        for (int i = 0; i < statTreeUIs.Length; i++)
        {
            statTreeUIDic[statTreeUIs[i].statType] = statTreeUIs[i];
            statTreeUIs[i].SetSkillTreeCost(statTreeDic[statTreeUIs[i].statType].statUpgradeInfos[1].needCost);
        }

        playerDamage = statTreeDic[StatType.ATTACK].statUpgradeInfos[0].upgradeValue;
        InGameUI.UI.Stat.SetAttackValue((int)playerDamage);

        playerAttackSpeed = statTreeDic[StatType.ATTACK_SPEED].statUpgradeInfos[0].upgradeValue;
        InGameUI.UI.Stat.SetAttackSpeedValue(playerAttackSpeed);

        statTreeUIDic[StatType.ATTACK].btnClickAction += () =>
        {
            TryStatUpgrade(StatType.ATTACK, ref playerDamage);
        };

        statTreeUIDic[StatType.ATTACK_SPEED].btnClickAction += () =>
        {
            TryStatUpgrade(StatType.ATTACK_SPEED, ref playerAttackSpeed);
        };

        statTreeUIDic[StatType.HEAL].btnClickAction += () =>
        {
            if (TryStatUpgrade(StatType.HEAL))
            {
                // �����ֱ�
            }
        };
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(TryStatUpgrade(StatType.ATTACK, ref playerDamage))
            {
                InGameUI.UI.Stat.SetAttackValue((int)playerDamage);
            }
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            if(TryStatUpgrade(StatType.ATTACK_SPEED, ref playerAttackSpeed))
            {
                InGameUI.UI.Stat.SetAttackSpeedValue((int)playerAttackSpeed);
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {

        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(TryStatUpgrade(StatType.HEAL))
            {
                // �����ֱ�
            }
        }
    }

    private bool TryStatUpgrade(StatType statType)
    {
        if (IsEnoughCost(statType))
        {
            statLevelDic[statType] += 1;
            statTreeUIDic[statType].SetSkillTreeCost(statTreeDic[statType].statUpgradeInfos[statLevelDic[statType]].needCost);

            return true;
        }

        return false;
    }

    private bool TryStatUpgrade(StatType statType, ref float targetChangeValue)
    {
        if(IsEnoughCost(statType))
        {
            statLevelDic[statType] += 1;
            targetChangeValue = statTreeDic[statType].statUpgradeInfos[statLevelDic[statType] - 1].upgradeValue;
            statTreeUIDic[statType].SetSkillTreeCost(statTreeDic[statType].statUpgradeInfos[statLevelDic[statType]].needCost);

            return true;
        }

        return false;
    }

    private bool IsEnoughCost(StatType statType)
    {
        int currentCost = GameManager.Game.currentCost;
        int needCost = statTreeDic[statType].statUpgradeInfos[statLevelDic[statType]].needCost;

        if (needCost <= currentCost)
        {
            GameManager.Game.RemoveCost(needCost);
            return true;
        }
        else
        {
            return false;
        }
    }
}
