using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUsePanel : MonoBehaviour
{
    private Animator skillAnimator;
    [SerializeField] Image charaImg;

    private void Awake()
    {
        skillAnimator = GetComponent<Animator>();
    }

    public void UseSkill()
    {
        skillAnimator.SetTrigger("Appear");
        Global.Sound.Play("SFX/Battle/b_char_atkboost");
    }

    private void SetMaskDisable()
    {
        charaImg.maskable = false;
    }
}
