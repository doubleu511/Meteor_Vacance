using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkillTreeUI : SkillTreeUI
{
    protected override void Start()
    {
        base.Start();
        GameManager.Player.GetComponent<HealthSystem>().OnDamaged += CallPlayerOnDamaged;
    }

    protected override void CallCostOnSet(int cost)
    {
        skillTreeBtn.interactable = cost >= currentCost && !GameManager.Player.GetIsFullHealth();
        skillTreeDisabledPanel.SetActive(cost < currentCost || GameManager.Player.GetIsFullHealth());
    }

    private void CallPlayerOnDamaged()
    {
        CallCostOnSet(GameManager.Game.currentCost);
    }
}
