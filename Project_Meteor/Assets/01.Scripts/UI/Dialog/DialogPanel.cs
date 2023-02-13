using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogPanel : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup dialogPanel;
    private DialogEvents dialogEvents;
    public static bool useWaitFlag = false;
    public static bool eventWaitFlag = false;

    #region 다이얼로그 변수
    [Header("Dialog")]
    [SerializeField] Image backgroundImg = null;
    [SerializeField] DialogCharacterHandler[] characterHandlers;
    private int currentHandlerIndex = 0;
    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI dialogText = null;
    [SerializeField] Text phoneDialogText = null;
    private DialogInfo beforeDialogInfo;
    private DialogInfo currentDialogInfo;

    [Space(10)]
    [SerializeField] CanvasGroup topUICanvasGroup;
    [SerializeField] Button logButton;
    [SerializeField] Button skipButton;

    private bool isPlayingDialog = false; // 현재 하나의 문단 다이얼로그가 재생중인가?
    private bool isText = false;
    private bool isTextEnd = false;

    [HideInInspector]
    public bool isClicked = false;

    private Queue<DialogInfo> dialogQueue = new Queue<DialogInfo>();

    private Coroutine textCoroutine = null;
    private Tweener textTween = null;

    [SerializeField] ActEvent testAct;
    private ActEvent currentAct;
    #endregion

    [Header("Backgrounds")]
    [SerializeField] Sprite[] backgroundSprites;
    [SerializeField] CanvasGroup blackScreen;

    #region 다이얼로그 리소스 저장
    private Dictionary<eCharacter, CharacterSO> characterDic = new Dictionary<eCharacter, CharacterSO>();
    public Dictionary<int, DialogSO> dialogDic = new Dictionary<int, DialogSO>();
    #endregion

    private void Awake()
    {
        dialogPanel = GetComponent<CanvasGroup>();
        dialogEvents = GetComponent<DialogEvents>();

        skipButton.onClick.AddListener(() =>
        {
            DialogSkip();
        });
    }

    private void Start()
    {
        CharacterSO[] allCharacterSOs = Resources.LoadAll<CharacterSO>("Character");
        foreach(CharacterSO item in allCharacterSOs)
        {
            characterDic.Add(item.characterUID, item);
        }
        characterDic.Add(eCharacter.NONE, null);

        DialogSO[] allDialogSOs = Resources.LoadAll<DialogSO>("Dialog");
        foreach (DialogSO item in allDialogSOs)
        {
            dialogDic.Add(item.dialogID, item);
        }

        StartAct(testAct);
    }

    public void StartAct(ActEvent act)
    {
        currentAct = act;
        Global.UI.UIFade(blackScreen, true);
        StartCoroutine(StartActCoroutine(act));
    }

    private IEnumerator StartActCoroutine(ActEvent act)
    {
        yield return new WaitForSeconds(0.5f);
        Global.UI.UIFade(blackScreen, UIFadeType.OUT, 1, true);
        StartDialog(act.startDialog);
    }

    public void StartDialog(DialogSO dialog)
    {
        for (int i = 0; i < dialog.dialogInfos.Count; i++)
        {
            dialogQueue.Enqueue(dialog.dialogInfos[i]);
        }

        if (!isPlayingDialog)
        {
            isPlayingDialog = true;

            //Global.UI.UIFade(dialogPanel, true);

            textCoroutine = StartCoroutine(TextCoroutine());
        }
    }

    private IEnumerator TextCoroutine()
    {
        while (dialogQueue.Count > 0)
        {
            isClicked = false;
            DialogInfo dialog = dialogQueue.Dequeue();
            currentDialogInfo = dialog;

            SetBackground(dialog.background);
            ShowText(dialog.text);
            SetCharacter(dialog);
            SetSpeakingDir(dialog.speakingDir);

            //Global.UI.UIFade(dialogPanel, true);

            useWaitFlag = false;
            // 이벤트가 걸리는지 확인
            if (EventTest(dialog))
            {
                if (useWaitFlag)
                {
                    yield return new WaitUntil(() => isTextEnd);
                    dialogEvents.OnTextEnd();

                    yield return new WaitUntil(() => isClicked);
                    dialogEvents.OnClicked();

                    yield return new WaitUntil(() => !eventWaitFlag);
                }
                else
                {
                    yield return new WaitUntil(() => isClicked);
                }
            }
            else
            {
                yield return new WaitUntil(() => isClicked);
            }

            beforeDialogInfo = dialog;
        }

        isPlayingDialog = false;
        StartCoroutine(BlackScreenFade());
    }

    private IEnumerator BlackScreenFade()
    {
        Global.UI.UIFade(blackScreen, UIFadeType.IN, 1, true);
        yield return new WaitForSeconds(0.5f);
        currentAct.onActEnded?.Invoke();
    }

    private bool EventTest(DialogInfo info)
    {
        if (info.eventMethod.Length > 0)
        {
            eventWaitFlag = true;
            dialogEvents.ThrowEvent(info.eventMethod);
            return true;
        }

        return false;
    }

    private void SetCharacter(DialogInfo dialogInfo)
    {
        if (beforeDialogInfo != null)
        {
            if (beforeDialogInfo.chracter_1.characterType != dialogInfo.chracter_1.characterType || beforeDialogInfo.chracter_2.characterType != dialogInfo.chracter_2.characterType)
            {
                characterHandlers[currentHandlerIndex].SetFade(false);
                currentHandlerIndex = (currentHandlerIndex + 1) % 2;
            }
        }

        characterHandlers[currentHandlerIndex].SetFade(true);
        characterHandlers[currentHandlerIndex].SetCharacter(
            characterDic[dialogInfo.chracter_1.characterType],
            characterDic[dialogInfo.chracter_2.characterType],
            dialogInfo.chracter_1.characterSpriteName,
            dialogInfo.chracter_2.characterSpriteName);
    }

    public void SetSpeakingDir(int dir)
    {
        characterHandlers[currentHandlerIndex].SetSpeakingDir(dir);

        if(dir == 0)
        {
            nameText.text = characterDic[currentDialogInfo.chracter_1.characterType].characterName;
        }
        else if (dir == 1)
        {
            nameText.text = characterDic[currentDialogInfo.chracter_2.characterType].characterName;
        }
        else
        {
            nameText.text = "";
        }
    }

    public void SetBackground(int backgroundIndex)
    {
        if (backgroundImg.sprite != backgroundSprites[backgroundIndex])
            backgroundImg.sprite = backgroundSprites[backgroundIndex];
    }

    private void ShowText(string text)
    {
        isText = true;
        isTextEnd = false;

        dialogText.text = "";
        phoneDialogText.text = "";
        int textLength = TextLength(text);

        textTween = phoneDialogText.DOText(text, textLength * 0.1f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        isTextEnd = true;
                    })
                    .OnUpdate(() =>
                    {
                        dialogText.text = phoneDialogText.text;
                    }).SetUpdate(true);

        int TextLength(string richText)
        {
            int len = 0;
            bool inTag = false;

            foreach (var ch in richText)
            {
                if (ch == '<')
                {
                    inTag = true;
                    continue;
                }
                else if (ch == '>')
                {
                    inTag = false;
                }
                else if (!inTag)
                {
                    len++;
                }
            }

            return len;
        }
    }

    public void DialogSkip()
    {
        if (!isPlayingDialog) return;
        if (eventWaitFlag) return;

        StopCoroutine(textCoroutine);
        textTween.Kill();

        while (dialogQueue.Count > 0)
        {
            DialogInfo dialog = dialogQueue.Peek();
            if (dialog.isStopAtSkip)
            {
                textCoroutine = StartCoroutine(TextCoroutine());
                isTextEnd = true;
                textTween.Complete();
                print("?");
                isText = false;
                isClicked = true;
                return;
            }
            else
            {
                dialogQueue.Dequeue();
            }
        }

        isPlayingDialog = false;
        StartCoroutine(BlackScreenFade());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isText) return;

        if (!isTextEnd)
        {
            isTextEnd = true;
            textTween.Complete();
        }
        else
        {
            isText = false;
            isClicked = true;
        }
    }
}
