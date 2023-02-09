using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostUI : MonoBehaviour
{
    [SerializeField] Transform costValueTrm;
    [SerializeField] TextMeshProUGUI costText;

    private void Awake()
    {
        InGameUI.Cost = this;
    }

    public void SetCostValue(int cost, float costValueScale)
    {
        costText.text = $"{cost}";
        costValueTrm.localScale = new Vector3(costValueScale, 1, 1);
    }
}
