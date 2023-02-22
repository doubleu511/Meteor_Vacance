using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleActBtn : MonoBehaviour
{
    public static bool isSimulateAct = false;

    private UnlockCondition condition;
    private Button actDialogBtn;

    [SerializeField] Image blackScreen;
    [SerializeField] TextMeshProUGUI actNameText;
    [SerializeField] int targetActIndex = 0;

    private void Awake()
    {
        condition = GetComponent<UnlockCondition>();
        actDialogBtn = GetComponent<Button>();
    }

    private void Start()
    {
        actDialogBtn.onClick.AddListener(() =>
        {
            isSimulateAct = true;
            DialogPanel.startActIndex = targetActIndex;
            Global.LoadScene.LoadScene("DialogScene");
        });

        Refresh();
    }

    private void Refresh()
    {
        if(!condition.IsUnlockable())
        {
            actDialogBtn.interactable = false;
            blackScreen.color = Color.black;
            actNameText.text = "???";
        }
    }
}
