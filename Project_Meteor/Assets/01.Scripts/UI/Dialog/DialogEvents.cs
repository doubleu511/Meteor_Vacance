using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvents : MonoBehaviour
{
    [SerializeField] CanvasGroup blackScreen;
    [SerializeField] CanvasGroup whiteScreen;

    [Header("Choice")]
    [SerializeField] DialogSelectButtonUI dialogSelectButtonPrefab;
    [SerializeField] CanvasGroup choicePanelTrm;

    private Action onTextEndAction;

    public void ThrowEvent(string[] _eventMethod)
    {
        // 이곳에 _eventMethod을 해석하는 코드 작성
        for (int i = 0; i < _eventMethod.Length; i++)
        {
            string[] methodParameters = _eventMethod[i].Split('\n');

            if (methodParameters.Length > 0)
            {
                switch (methodParameters[0])
                {
                    case "CHOOSE":
                        ExtractCHOOSEParameters(methodParameters[1], methodParameters[2]);
                        break;
                }
            }
        }
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

        if (choices.Length != affectResults.Length)
        {
            Debug.LogError("에러 : CHOOSE의 파라미터 길이가 각각 다릅니다.");
            return;
        }

        for (int i = 0; i < choicePanelTrm.transform.childCount; i++)
        {
            choicePanelTrm.transform.GetChild(i).gameObject.SetActive(false);
        }

        choicePanelTrm.gameObject.SetActive(true);
        Global.UI.UIFade(choicePanelTrm, false);

        for (int i = 0; i < choices.Length; i++)
        {
            DialogSelectButtonUI selectButton = Global.Pool.GetItem<DialogSelectButtonUI>();
            int affectResult = affectResults[i];
            selectButton.Init(choices[i], () =>
            {
                //CookingManager.Counter.AddDialog(affectResult);
                choicePanelTrm.gameObject.SetActive(false);
                DialogPanel.eventWaitFlag = false;
            });
        }
    }

    private void FADE_BLACK()
    {
        Global.UI.UIFade(blackScreen, UIFadeType.IN, 1, true, () =>
        {
            DialogPanel.eventWaitFlag = false;
            Global.UI.UIFade(blackScreen, UIFadeType.OUT, 1, true);
        });
    }

    public void OnTextEnd()
    {
        if (onTextEndAction != null)
        {
            onTextEndAction.Invoke();
        }

        onTextEndAction = null;
    }
}
