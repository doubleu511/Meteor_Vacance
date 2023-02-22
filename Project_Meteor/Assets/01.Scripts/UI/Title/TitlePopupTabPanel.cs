using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitlePopupTabPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] Button[] tabButtons;
    [SerializeField] CanvasGroup[] tabGroups;

    private CanvasGroup currentGroup;
    private Button currentBtn;
 
    [SerializeField] Color disabledColor;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int j = i;
            tabButtons[j].onClick.AddListener(() =>
            {
                ButtonClickEvent(tabGroups[j], tabButtons[j]);
            });
        }

        currentGroup = tabGroups[0];
        currentBtn = tabButtons[0];
    }

    public void SetPanel(bool fade)
    {
        if (fade)
        {
            currentGroup = tabGroups[0];
            currentBtn = tabButtons[0];

            SetGroupFade(tabGroups[0], tabButtons[0], true);
            for (int i = 1; i < tabButtons.Length; i++)
            {
                SetGroupFade(tabGroups[i], tabButtons[i], false);
            }
        }
        Global.UI.UIFade(canvasGroup, fade);
    }

    private void ButtonClickEvent(CanvasGroup group, Button button)
    {
        SetGroupFade(currentGroup, currentBtn, false);
        currentGroup = group;
        currentBtn = button;
        SetGroupFade(currentGroup, currentBtn, true);
    }

    private void SetGroupFade(CanvasGroup group, Button button, bool value)
    {
        Global.UI.UIFade(group, value);

        button.GetComponent<Image>().color = value ? Color.white : disabledColor;

        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.color = value ? Color.black : Color.white;
    }
}
