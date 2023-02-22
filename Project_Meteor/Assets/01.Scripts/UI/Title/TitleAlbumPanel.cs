using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleAlbumPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] Button dialogBtn;
    [SerializeField] Button cgBtn;
    [SerializeField] Button characterBtn;

    [SerializeField] CanvasGroup dialogGroup;
    [SerializeField] CanvasGroup cgGroup;
    [SerializeField] CanvasGroup characterGroup;

    private CanvasGroup currentGroup;
    private Button currentBtn;
 
    [SerializeField] Color disabledColor;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        dialogBtn.onClick.AddListener(() =>
        {
            ButtonClickEvent(dialogGroup, dialogBtn);
        });

        cgBtn.onClick.AddListener(() =>
        {
            ButtonClickEvent(cgGroup, cgBtn);
        });

        characterBtn.onClick.AddListener(() =>
        {
            ButtonClickEvent(characterGroup, characterBtn);
        });

        currentGroup = dialogGroup;
        currentBtn = dialogBtn;
    }

    public void SetPanel()
    {
        currentGroup = dialogGroup;
        currentBtn = dialogBtn;

        SetGroupFade(dialogGroup, dialogBtn, true);
        SetGroupFade(cgGroup, cgBtn, false);
        SetGroupFade(characterGroup, characterBtn, false);
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
