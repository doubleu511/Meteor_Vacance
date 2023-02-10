using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUsePanel : MonoBehaviour
{
    [SerializeField] Image charaImg;

    private void Start()
    {
        Global.Sound.Play("SFX/Battle/b_char_atkboost");
    }

    private void SetMaskDisable()
    {
        charaImg.maskable = false;
    }
}
