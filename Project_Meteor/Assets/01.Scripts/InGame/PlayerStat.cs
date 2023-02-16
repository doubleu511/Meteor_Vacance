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

    private PlayerAbility ability;

    private void Awake()
    {
        ability = GetComponent<PlayerAbility>();
    }

    private void Start()
    {
        for (int i = 0; i < statTrees.Length; i++)
        {
            statTreeDic[statTrees[i].statType] = statTrees[i];
        }

        for (int i = 0; i < statTreeUIs.Length; i++)
        {
            statTreeUIDic[statTreeUIs[i].statType] = statTreeUIs[i];
            statTreeUIs[i].SetSkillTreeCost(statTreeDic[statTreeUIs[i].statType].statUpgradeInfos[0].needCost);
            statTreeUIs[i].SetBottomLine(1);

            StatTreeSO statTree = statTreeDic[statTreeUIs[i].statType];
            statTreeUIs[i].btnEnterAction += () =>
            {
                if (statTree.isDontNeedUpgradeValue)
                {
                    InGameUI.UI.StatHover.SetHoverUIWithoutUpgrade(
                        statTree.iconSpr,
                        statTree.statTreeName,
                        statTree.statTreeLore
                        );
                }
                else
                {
                    string afterStr = statTree.statUpgradeInfos.Length <= statLevelDic[statTree.statType]
                    ? "MAX"
                    : statTree.statUpgradeInfos[statLevelDic[statTree.statType]].upgradeValue.ToString();

                    InGameUI.UI.StatHover.SetHoverUI(
                        statTree.iconSpr,
                        statLevelDic[statTree.statType],
                        statTree.statTreeName,
                        statTree.statTreeLore,
                        statTree.statUpgradeInfos[statLevelDic[statTree.statType] - 1].upgradeValue.ToString(),
                        afterStr
                        );
                }
            };
        }

        playerDamage = statTreeDic[StatType.ATTACK].statUpgradeInfos[0].upgradeValue;
        InGameUI.UI.Stat.SetAttackValue((int)playerDamage);

        playerAttackSpeed = statTreeDic[StatType.ATTACK_SPEED].statUpgradeInfos[0].upgradeValue;
        InGameUI.UI.Stat.SetAttackSpeedValue(playerAttackSpeed);
        GameManager.Player.SetAttackSpeedMultiplier(playerAttackSpeed);

        statTreeUIDic[StatType.ATTACK].btnClickAction += () =>
        {
            TryStatUpgrade(StatType.ATTACK, ref playerDamage);
            InGameUI.UI.Stat.SetAttackValue((int)playerDamage);
        };

        statTreeUIDic[StatType.ATTACK_SPEED].btnClickAction += () =>
        {
            TryStatUpgrade(StatType.ATTACK_SPEED, ref playerAttackSpeed);
            InGameUI.UI.Stat.SetAttackSpeedValue(playerAttackSpeed);
            GameManager.Player.SetAttackSpeedMultiplier(playerAttackSpeed);
        };

        statTreeUIDic[StatType.SKILL_LEVEL].btnClickAction += () =>
        {
            TryStatUpgrade(StatType.SKILL_LEVEL);
            ability.AddAbilityLevel();
        };

        statTreeUIDic[StatType.HEAL].btnClickAction += () =>
        {
            if (TryStatUpgrade(StatType.HEAL))
            {
                GameManager.Player.HealHealth();
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
                InGameUI.UI.Stat.SetAttackSpeedValue(playerAttackSpeed);
                GameManager.Player.SetAttackSpeedMultiplier(playerAttackSpeed);
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (TryStatUpgrade(StatType.SKILL_LEVEL))
            {
                ability.AddAbilityLevel();
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(TryStatUpgrade(StatType.HEAL))
            {
                GameManager.Player.HealHealth();
            }
        }
    }

    private bool TryStatUpgrade(StatType statType)
    {
        if (statTreeDic[statType].statUpgradeInfos.Length <= statLevelDic[statType])
        {
            return false;
        }

        if (IsEnoughCost(statType))
        {
            statTreeUIDic[statType].SetSkillTreeCost(statTreeDic[statType].statUpgradeInfos[statLevelDic[statType]].needCost);
            statLevelDic[statType] += 1;
            statTreeUIDic[statType].SetBottomLine(statLevelDic[statType]);
            statTreeUIDic[statType].PlayAnimation();
            return true;
        }

        return false;
    }

    private bool TryStatUpgrade(StatType statType, ref float targetChangeValue)
    {
        if (statTreeDic[statType].statUpgradeInfos.Length <= statLevelDic[statType])
        {
            return false;
        }

        if (IsEnoughCost(statType))
        {
            targetChangeValue = statTreeDic[statType].statUpgradeInfos[statLevelDic[statType]].upgradeValue;
            statTreeUIDic[statType].SetSkillTreeCost(statTreeDic[statType].statUpgradeInfos[statLevelDic[statType]].needCost);
            statLevelDic[statType] += 1;
            statTreeUIDic[statType].SetBottomLine(statLevelDic[statType]);
            statTreeUIDic[statType].PlayAnimation();
            return true;
        }

        return false;
    }

    private bool IsEnoughCost(StatType statType)
    {
        int currentCost = GameManager.Game.currentCost;
        int needCost = statTreeDic[statType].statUpgradeInfos[statLevelDic[statType] - 1].needCost;

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
