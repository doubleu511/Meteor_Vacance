using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    ATTACK = 0,
    ATTACK_SPEED = 1,
    SKILL_LEVEL = 2,
    HEAL = 3,
}

[CreateAssetMenu(fileName = "StatLevel Scriptable Object", menuName = "ScriptableObjects/StatLevel Scriptable Object")]
public class StatTreeSO : ScriptableObject
{
    [System.Serializable]
    public struct StatUpgradeInfo
    {
        public float upgradeValue;
        public int needCost;
    }

    public StatType statType;
    public StatUpgradeInfo[] statUpgradeInfos;

    [Header("Hover")]
    public Sprite iconSpr;
    public string statTreeName;
    public string statTreeLore;
    public bool isDontNeedUpgradeValue;
}
