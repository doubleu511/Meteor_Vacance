using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CostUI : MonoBehaviour
{
    [SerializeField] Transform costValueTrm;
    [SerializeField] TextMeshProUGUI costText;

    public Action<int> onCostSet;

    public void SetCost(int cost)
    {
        costText.text = $"{cost}";
        onCostSet?.Invoke(cost);
    }

    public void SetCostValue(float costValueScale)
    {
        costValueTrm.localScale = new Vector3(costValueScale, 1, 1);
    }
}
