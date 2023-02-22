using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleCGBtn : MonoBehaviour
{
    private UnlockCondition condition;
    private Button cgBtn;

    [SerializeField] GameObject blackScreen;
    [SerializeField] Image cgImage;

    private void Awake()
    {
        condition = GetComponent<UnlockCondition>();
        cgBtn = GetComponent<Button>();
    }

    private void Start()
    {
        cgBtn.onClick.AddListener(() =>
        {
            // CG ÄÑÁÖ±â
            TitleDetailImagePanel.Detail.SetCGImage(cgImage.sprite);
        });

        Refresh();
    }

    private void Refresh()
    {
        if (!condition.IsUnlockable())
        {
            cgBtn.interactable = false;
            blackScreen.SetActive(true);
        }
    }
}
