using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [SerializeField] Image iconImage;

    [SerializeField] TextMeshProUGUI enemyNameText;
    [SerializeField] TextMeshProUGUI enemyLoreText;

    [SerializeField] Button pauseBtn;
    [SerializeField] Image pauseStatus;
    [SerializeField] Image pauseWaitFillAmount;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] Sprite resumeSprite;

    private Queue<EnemyInfoSO> enemyInfoQueue = new Queue<EnemyInfoSO>();
    private bool isPause = false;
    private Sequence currentSeq;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        pauseBtn.onClick.AddListener(ButtonInteraction);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentSeq.IsActive())
            {
                ButtonInteraction();
            }
        }
    }

    public void AddInfoQueue(EnemyInfoSO enemyInfo)
    {
        enemyInfoQueue.Enqueue(enemyInfo);

        if(enemyInfoQueue.Count == 1)
        {
            canvasGroup.alpha = 1;
            PlayInfo();
        }
    }

    private void PlayInfo()
    {
        if (enemyInfoQueue.Count > 0)
        {
            EnemyInfoSO info = enemyInfoQueue.Peek();

            iconImage.sprite = info.enemyIcon;
            enemyNameText.text = info.enemyName;
            enemyLoreText.text = info.enemyLore;

            pauseStatus.sprite = pauseSprite;
            isPause = false;

            UtilClass.ForceRefreshSize(transform);

            Vector3 anchor = rectTransform.anchoredPosition;
            anchor.x = rectTransform.sizeDelta.x + 10;
            rectTransform.anchoredPosition = anchor;

            pauseWaitFillAmount.fillAmount = 1;

            currentSeq = DOTween.Sequence();
            currentSeq.Append(rectTransform.DOAnchorPosX(0, 0.3f));
            currentSeq.Append(pauseWaitFillAmount.DOFillAmount(0, 5).SetEase(Ease.Linear));
            currentSeq.Append(rectTransform.DOAnchorPosX(anchor.x, 0.3f));
            currentSeq.AppendInterval(1);
            currentSeq.AppendCallback(() =>
            {
                enemyInfoQueue.Dequeue();
                PlayInfo();
            });
        }
    }

    private void ButtonInteraction()
    {
        isPause = !isPause;

        if (isPause)
        {
            currentSeq.Pause();
            pauseStatus.sprite = resumeSprite;
        }
        else
        {
            currentSeq.Play();
            pauseStatus.sprite = pauseSprite;
        }
    }
}
