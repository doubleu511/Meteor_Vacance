using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUsePanel : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    private Animator skillAnimator;

    [SerializeField] Image charaImg;

    private void Awake()
    {
        skillAnimator = GetComponent<Animator>();
    }

    public void UseSkill(bool isLeft)
    {
        Vector3 offset = rectTransform.anchoredPosition;
        offset.x = isLeft ? 0 : 1245;
        rectTransform.anchoredPosition = offset;

        skillAnimator.SetTrigger("Appear");
        Global.Sound.Play("SFX/Battle/b_char_atkboost");
        Global.Sound.PlayRandom(eSound.Voice, 1, "SFX/Voice/fight1", "SFX/Voice/fight2", "SFX/Voice/fight3", "SFX/Voice/fight4");
    }

    private void SetMaskDisable()
    {
        charaImg.maskable = false;
    }
}
