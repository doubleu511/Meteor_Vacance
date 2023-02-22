using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDetailImagePanel : MonoBehaviour
{
    public static TitleDetailImagePanel Detail;

    private CanvasGroup canvasGroup;
    private bool isOpen = false;

    [SerializeField] CanvasGroup charaCanvasGroup;
    [SerializeField] Image[] detailImageDummies;
    private Sprite[] savedSprites;
    private int detailImageIndex = 0;
    private int detailImageMaxIndex = 0;

    [SerializeField] CanvasGroup cgCanvasGroup;
    [SerializeField] Image cgImage;

    [SerializeField] TitleDetailBtnGroup topBtnGroup;
    [SerializeField] Button prevBtn;
    [SerializeField] Button nextBtn;
    [SerializeField] Button cancelBtn;


    private void Awake()
    {
        Detail = this;

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        prevBtn.onClick.AddListener(() =>
        {
            detailImageIndex--;
            RefreshCharaIndex();
        });

        nextBtn.onClick.AddListener(() =>
        {
            detailImageIndex++;
            RefreshCharaIndex();
        });

        cancelBtn.onClick.AddListener(() =>
        {
            SetDetailPanel(false);
        });
    }

    private void Update()
    {
        if(isOpen)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                isOpen = false;
                SetDetailPanel(false);
            }
        }
    }

    public void SetDetailImages(Sprite[] sprites)
    {
        savedSprites = sprites;
        detailImageIndex = 0;
        detailImageMaxIndex = (sprites.Length - 1) / 3;

        charaCanvasGroup.alpha = 1;
        topBtnGroup.SetFade(true);

        RefreshCharaIndex();
        isOpen = true;
        SetDetailPanel(true);
    }

    private void RefreshCharaIndex()
    {
        prevBtn.gameObject.SetActive(detailImageIndex > 0);
        nextBtn.gameObject.SetActive(detailImageIndex < detailImageMaxIndex);

        for (int i = 0; i < 3; i++)
        {
            detailImageDummies[i].gameObject.SetActive((detailImageIndex * 3) + i < savedSprites.Length);
            if ((detailImageIndex * 3) + i < savedSprites.Length)
            {
                detailImageDummies[i].sprite = savedSprites[(detailImageIndex * 3) + i];
            }
        }
    }

    public void SetCGImage(Sprite sprite)
    {
        cgImage.sprite = sprite;

        cgCanvasGroup.alpha = 1;
        topBtnGroup.SetFade(true);

        prevBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        isOpen = true;
        SetDetailPanel(true);
    }


    private CanvasGroup currentShowGroup;
    public void SetCanvasGroup(CanvasGroup canvasGroup)
    {
        currentShowGroup = canvasGroup;
        canvasGroup.alpha = 1;
        isOpen = true;
        SetDetailPanel(true);
    }

    public void SetDetailPanel(bool fade)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(fade ? 1 : 0, 0.5f);
        canvasGroup.interactable = fade;
        canvasGroup.blocksRaycasts = fade;

        if(!fade)
        {
            charaCanvasGroup.alpha = 0;
            cgCanvasGroup.alpha = 0;

            if (currentShowGroup != null)
            {
                currentShowGroup.alpha = 0;
                currentShowGroup = null;
            }
        }
    }
}
