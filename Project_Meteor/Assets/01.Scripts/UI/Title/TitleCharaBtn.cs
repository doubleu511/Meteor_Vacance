using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleCharaBtn : MonoBehaviour
{
    private UnlockCondition condition;
    private Button charaBtn;

    [SerializeField] Sprite[] includeSprites;
    [SerializeField] CanvasGroup otherGroup;
    [SerializeField] Image[] previewImages;
    [SerializeField] TextMeshProUGUI nameTagText;

    private void Awake()
    {
        condition = GetComponent<UnlockCondition>();
        charaBtn = GetComponent<Button>();
    }

    private void Start()
    {
        charaBtn.onClick.AddListener(() =>
        {
            // 캐릭터 켜주기
            if (otherGroup != null)
            {
                TitleDetailImagePanel.Detail.SetCanvasGroup(otherGroup);
            }
            else
            {
                TitleDetailImagePanel.Detail.SetDetailImages(includeSprites);
            }
        });

        Refresh();
    }

    private void Refresh()
    {
        if (!condition.IsUnlockable())
        {
            charaBtn.interactable = false;
            nameTagText.text = "???";
            for (int i = 0; i < previewImages.Length; i++)
            {
                previewImages[i].color = Color.black;
            }
        }
    }
}
