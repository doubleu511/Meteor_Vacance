using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogPanel : MonoBehaviour
{
    private CanvasGroup dialogPanel;
    private DialogEvents dialogEvents;
    public static bool useWaitFlag = false;
    public static bool eventWaitFlag = false;

    [Header("Dialog")]
    [SerializeField] Image backgroundImg = null;
    [SerializeField] TextMeshProUGUI dialogText = null;
    [SerializeField] Text phoneDialogText = null;
    public static DialogInfo currentDialog;

    [Space(10)]
    [SerializeField] CanvasGroup topUICanvasGroup;
    [SerializeField] Button logButton;
    [SerializeField] Button skipButton;

    private bool isPlayingDialog = false; // 현재 하나의 문단 다이얼로그가 재생중인가?
    private bool isText = false;
    private bool isTextEnd = false;

    private bool isClicked = false;

    private Queue<DialogInfo> dialogQueue = new Queue<DialogInfo>();

    private Coroutine textCoroutine = null;
    private Tweener textTween = null;

    private string textString = "";

    private void Awake()
    {
        dialogPanel = GetComponent<CanvasGroup>();
        dialogEvents = GetComponent<DialogEvents>();

        skipButton.onClick.AddListener(() =>
        {
            DialogSkip();
        });
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

            Global.UI.UIFade(dialogPanel, true);

            textCoroutine = StartCoroutine(TextCoroutine());
        }
    }

    private IEnumerator TextCoroutine()
    {
        while (dialogQueue.Count > 0)
        {
            isClicked = false;
            DialogInfo dialog = dialogQueue.Dequeue();
            currentDialog = dialog;
            ShowText(dialog.text);

            Global.UI.UIFade(dialogPanel, true);

            // 이벤트가 걸리는지 확인
            if (EventTest(dialog))
            {
                yield return new WaitUntil(() => isTextEnd);
                if (useWaitFlag)
                {
                    dialogEvents.OnTextEnd();
                    Global.UI.UIFade(dialogPanel, false);
                    yield return new WaitUntil(() => !eventWaitFlag);
                }
                else
                {
                    yield return new WaitUntil(() => isClicked);
                }
            }
            else
            {
                yield return new WaitUntil(() => isTextEnd);
                yield return new WaitUntil(() => isClicked);
            }
        }

        Global.UI.UIFade(dialogPanel, false);
        isPlayingDialog = false;
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

    public void SetBackground(Sprite background)
    {
        if (backgroundImg.sprite != background)
            backgroundImg.sprite = background;
    }

    public void SetCharacterColor(Image chara, Color color)
    {
        Image[] charaImgs = chara.GetComponentsInChildren<Image>();

        for (int i = 0; i < charaImgs.Length; i++)
        {
            charaImgs[i].color = color;
        }
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
                        textString = "";
                        isTextEnd = true;
                    })
                    .OnUpdate(() =>
                    {
                        dialogText.text = phoneDialogText.text;
                        string parsed_textString = textString.Replace(" ", "");
                        string parsed_dialogText = dialogText.text.Replace(" ", "");

                        if (parsed_textString != parsed_dialogText)
                        {
                            Global.Sound.Play("SFX/talk", eSound.Effect);
                        }

                        textString = dialogText.text;
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
        if (isPlayingDialog)
        {
            StopCoroutine(textCoroutine);
            textTween.Complete();

            textString = "";

            isPlayingDialog = false;

            Global.UI.UIFade(dialogPanel, UIFadeType.OUT, 0.5f, true);
        }
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
