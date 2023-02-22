using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] GameObject[] tutorialGroups;

    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Transform dialogBox;
    [TextArea(2, 3)]
    [SerializeField] string[] tutorialTexts;

    [SerializeField] Image tutorialHead;
    [SerializeField] Sprite[] tutorialHeadSprs;

    private int tutorialIndex = 0;
    [SerializeField] bool isTitleScene = false;

    [SerializeField] Button prevBtn;
    [SerializeField] Button nextBtn;
    [SerializeField] Button cancelBtn;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (!isTitleScene)
        {
            bool isSeenTutorial = SecurityPlayerPrefs.GetBool("SeenTutorial", false);
            if (!isSeenTutorial)
            {
                SetTutorialPanel(true);
                SecurityPlayerPrefs.SetBool("SeenTutorial", true);
            }
        }

        prevBtn.onClick.AddListener(() =>
        {
            tutorialIndex--;
            RefreshIndex();
        });

        nextBtn.onClick.AddListener(() =>
        {
            tutorialIndex++;
            RefreshIndex();
        });

        cancelBtn.onClick.AddListener(() =>
        {
            SetTutorialPanel(false);
        });
    }

    [ContextMenu("ResetTutorialPrefs")]
    private void ResetTutorialPrefs()
    {
        SecurityPlayerPrefs.DeleteKey("SeenTutorial");
    }

    public void SetTutorialPanel(bool fade)
    {
        Time.timeScale = fade ? 0 : 1;
        Global.UI.UIFade(canvasGroup, fade);

        if (fade)
        {
            tutorialIndex = 0;
            RefreshIndex();
        }
    }

    private void RefreshIndex()
    {
        prevBtn.gameObject.SetActive(tutorialIndex > 0);
        nextBtn.gameObject.SetActive(tutorialIndex < tutorialGroups.Length - 1);

        dialogText.text = tutorialTexts[tutorialIndex];
        UtilClass.ForceRefreshSize(dialogBox);

        tutorialHead.sprite = tutorialHeadSprs[tutorialIndex];
        for (int i = 0; i < tutorialGroups.Length; i++)
        {
            if (tutorialGroups[i] != null)
            {
                print(i);
                tutorialGroups[i].SetActive(i == tutorialIndex);
            }
        }
    }
}
