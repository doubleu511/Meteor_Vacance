using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SkillTreeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StatType statType;

    protected Button skillTreeBtn;
    protected int currentCost;
    private int currentlevel;

    [SerializeField] protected GameObject skillTreeDisabledPanel;
    [SerializeField] Image bottomLine;
    [SerializeField] TextMeshProUGUI skillTreeCostText;

    public Action btnClickAction;
    public Action btnEnterAction;

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

    protected virtual void Start()
    {
        skillTreeBtn.onClick.AddListener(() =>
        {
            btnClickAction?.Invoke();
            btnEnterAction?.Invoke();
            InGameUI.UI.StatHover.PlaySetHoverAnimation(colorDic[currentlevel]);
        });

        InGameUI.UI.Cost.onCostSet += CallCostOnSet;
    }

    protected virtual void CallCostOnSet(int cost)
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
        currentlevel = level;
        bottomLine.color = colorDic[level];
    }

    public void PlayAnimation()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1);
        transform.DOScale(1, 0.25f);
        Global.Sound.Play("SFX/Battle/b_char_addshield");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        btnEnterAction?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InGameUI.UI.StatHover.ExitHoverUI();
    }
}
