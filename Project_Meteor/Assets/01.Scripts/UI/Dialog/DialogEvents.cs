using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvents : MonoBehaviour
{
    private DialogPanel dialogPanel;

    [Header("Choice")]
    [SerializeField] DialogSelectButtonUI dialogSelectButtonPrefab;
    [SerializeField] CanvasGroup choicePanelTrm;
    [SerializeField] CanvasGroup dialogLayout;

    [Header("Effect")]
    [SerializeField] CanvasGroup oldFlimGroup;

    private Action onTextEndAction;
    private Action onClickedAction;

    private bool instantInvoke = false;

    private void Awake()
    {
        dialogPanel = GetComponent<DialogPanel>();
    }

    private void Start()
    {
        Global.Pool.CreatePool<DialogSelectButtonUI>(dialogSelectButtonPrefab.gameObject, choicePanelTrm.transform, 3);
    }

    public void ThrowEvent(string[] _eventMethod)
    {
        // 이곳에 _eventMethod을 해석하는 코드 작성
        for (int i = 0; i < _eventMethod.Length; i++)
        {
            string[] methodParameters = _eventMethod[i].Split('\n');

            if (methodParameters.Length > 0)
            {
                if(methodParameters[0][0] == '$')
                {
                    instantInvoke = true;
                    methodParameters[0] = methodParameters[0].Replace("$", "");
                }

                switch (methodParameters[0])
                {
                    case "ADDDIALOG":
                        ExtractADDDIALOGParameters(methodParameters[1]);
                        break;
                    case "CHOOSE":
                        ExtractCHOOSEParameters(methodParameters[1], methodParameters[2]);
                        break;
                    case "OLDFLIM":
                        ExtractOLDFLIMParameters(methodParameters[1]);
                        break;
                }
            }
            instantInvoke = false;
        }
    }

    private void ExtractADDDIALOGParameters(string param1)
    {
        ADDDIALOG(int.Parse(param1));
    }

    private void ADDDIALOG(int dialogId)
    {
        dialogPanel.StartDialog(dialogPanel.dialogDic[dialogId]);
        DialogPanel.eventWaitFlag = false;
    }

    private void ExtractCHOOSEParameters(string param1, string param2)
    {
        string[] choicesSplit = param1.Split(',');
        string[] affectResultSplit = param2.Split(',');

        int[] affectResults = Array.ConvertAll(affectResultSplit, (e) => int.Parse(e));

        CHOOSE(choicesSplit, affectResults);
    }

    private void CHOOSE(string[] choices, int[] affectResults)
    {
        DialogPanel.useWaitFlag = true;

        if (instantInvoke)
        {
            onTextEndAction += () =>
            {
                ChooseEvent();
            };
        }
        else
        {
            onClickedAction += () =>
            {
                ChooseEvent();
            };
        }

        void ChooseEvent()
        {
            dialogPanel.SetSpeakingDir(2);
            dialogLayout.alpha = 0;

            if (choices.Length != affectResults.Length)
            {
                Debug.LogError("에러 : CHOOSE의 파라미터 길이가 각각 다릅니다.");
                return;
            }

            for (int i = 0; i < choicePanelTrm.transform.childCount; i++)
            {
                choicePanelTrm.transform.GetChild(i).gameObject.SetActive(false);
            }

            Global.UI.UIFade(choicePanelTrm, true);

            for (int i = 0; i < choices.Length; i++)
            {
                DialogSelectButtonUI selectButton = Global.Pool.GetItem<DialogSelectButtonUI>();
                int affectResult = affectResults[i];
                selectButton.Init(choices[i], () =>
                {
                    if (affectResult != -1)
                    {
                        dialogPanel.StartDialog(dialogPanel.dialogDic[affectResult]);
                    }
                    Global.UI.UIFade(choicePanelTrm, false);
                    dialogLayout.alpha = 1;
                    dialogPanel.isClicked = true;
                    DialogPanel.eventWaitFlag = false;
                });
            }
        }
    }

    private void ExtractOLDFLIMParameters(string param1)
    {
        OLDFLIM(bool.Parse(param1));
    }

    public void OLDFLIM(bool fade)
    {
        oldFlimGroup.DOFade(fade ? 1 : 0, 0.25f);
    }

    public void LoadScene(string name)
    {
        Global.LoadScene.LoadScene(name);
    }

    public void OnTextEnd()
    {
        if (onTextEndAction != null)
        {
            onTextEndAction.Invoke();
        }

        onTextEndAction = null;
    }

    public void OnClicked()
    {
        if (onClickedAction != null)
        {
            onClickedAction.Invoke();
        }

        onClickedAction = null;
    }
}
