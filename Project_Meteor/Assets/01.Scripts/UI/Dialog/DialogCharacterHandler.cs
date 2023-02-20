using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogCharacterHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] Image character_1;
    [SerializeField] Image character_1_overlay;
    [SerializeField] Image character_2;
    [SerializeField] Image character_2_overlay;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetCharacter(CharacterSO character1, CharacterSO character2, string characterState_1 = "idle", string characterState_2 = "idle")
    {
        if (character1 != null && characterState_1 != "null")
        {
            character_1.gameObject.SetActive(true);
            character_1.sprite = character1.FindSpriteState(characterState_1);
            character_1_overlay.sprite = character_1.sprite;
        }
        else
        {
            character_1.gameObject.SetActive(false);
        }

        if (character2 != null && characterState_2 != "null")
        {
            character_2.gameObject.SetActive(true);
            character_2.sprite = character2.FindSpriteState(characterState_2);
            character_2_overlay.sprite = character_2.sprite;
        }
        else
        {
            character_2.gameObject.SetActive(false);
        }
    }

    public void SetOverlayColor(Color color)
    {

        character_1_overlay.material.color = color;
        character_2_overlay.material.color = color;
    }

    public void SetFade(bool value)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(value ? 1 : 0, 0.25f);
    }

    public void SetSpeakingDir(int dir) // 0이면 왼쪽, 1이면 오른쪽, 2면 둘다 아님
    {
        character_1.color = (dir != 0) ? Color.gray : Color.white;
        character_2.color = (dir != 1) ? Color.gray : Color.white;
    }
}
