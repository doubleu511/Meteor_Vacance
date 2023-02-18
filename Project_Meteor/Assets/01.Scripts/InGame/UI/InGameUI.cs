using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public static InGameUI UI;

    public InfoUI Info;
    public CostUI Cost;
    public StatUI Stat;
    public AbilityUI Ability;
    public StatHoverUI StatHover;
    public EnemyInfoUI EnemyInfo;
    public GameResultUI GameResult;

    private void Awake()
    {
        UI = this;
    }
}
