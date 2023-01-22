using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogCharacterHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] Image character_1;
    [SerializeField] Image character_2;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetCharacter(CharacterSO character1, CharacterSO character2, string characterState_1 = "idle", string characterState_2 = "idle")
    {
        character_1.gameObject.SetActive(character1 != null);
        character_2.gameObject.SetActive(character2 != null);

        if (character1 != null)
        {
            character_1.sprite = character1.FindSpriteState(characterState_1);
        }

        if(character2 != null)
        {
            character_2.sprite = character2.FindSpriteState(characterState_2);
        }
    }

    public void SetFade(bool value)
    {
        canvasGroup.DOFade(value ? 1 : 0, 0.5f);
    }

    public void SetSpeakingDir(int dir) // 0이면 왼쪽, 1이면 오른쪽, 2면 둘다 아님
    {
        character_1.color = (dir != 0) ? Color.gray : Color.white;
        character_2.color = (dir != 1) ? Color.gray : Color.white;
    }
}
