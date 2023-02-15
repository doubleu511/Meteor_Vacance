using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StatType statType;

    private Button skillTreeBtn;
    private int currentCost;

    [SerializeField] GameObject skillTreeDisabledPanel;
    [SerializeField] TextMeshProUGUI skillTreeCostText;

    public Action btnClickAction;

    private void Awake()
    {
        skillTreeBtn = GetComponent<Button>();
    }

    private void Start()
    {
        skillTreeBtn.onClick.AddListener(() =>
        {
            btnClickAction?.Invoke();
        });

        InGameUI.UI.Cost.onCostSet += CallCostOnSet;
    }

    private void CallCostOnSet(int cost)
    {
        skillTreeBtn.interactable = cost >= currentCost;
        skillTreeDisabledPanel.SetActive(cost < currentCost);
    }

    public void SetSkillTreeCost(int cost)
    {
        currentCost = cost;
        skillTreeCostText.text = $"{cost}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
