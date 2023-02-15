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
    [SerializeField] Image bottomLine;
    [SerializeField] TextMeshProUGUI skillTreeCostText;

    public Action btnClickAction;

    private static Dictionary<int, Color32> colorDic = new Dictionary<int, Color32>()
    {
        { 1, new Color32(255, 255, 255, 255) },
        { 2, new Color32(220, 229, 55, 255) },
        { 3, new Color32(1, 170, 240, 255) },
        { 4, new Color32(233, 106, 233, 255) },
        { 5, new Color32(255, 185, 0, 255) },
        { 6, new Color32(255, 113, 0, 255) },
    };

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
        if (cost != 99)
        {
            currentCost = cost;
            skillTreeCostText.text = $"{cost}";
        }
        else
        {
            currentCost = 100;
            skillTreeCostText.text = $"--";
        }
    }

    public void SetBottomLine(int level)
    {
        bottomLine.color = colorDic[level];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
