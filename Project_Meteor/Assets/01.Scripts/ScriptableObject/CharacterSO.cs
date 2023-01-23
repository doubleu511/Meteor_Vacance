using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharacter
{
    NONE,
    DOCTOR,
    METEOR,
    KALTSIT
}

[CreateAssetMenu(fileName = "Character Scriptable Object", menuName = "ScriptableObjects/Character Scriptable Object")]
public class CharacterSO : ScriptableObject
{
    [System.Serializable]
    public struct CharacterSprite
    {
        public string spriteName;
        public Sprite sprite;
    }

    public eCharacter characterUID; // 캐릭터 구별용 enum
    public string characterName; // 실제적으로 쓰이는 이름
    
    public CharacterSprite[] characterSprites; // 캐릭터 스프라이트들

    public Sprite FindSpriteState(string stateName)
    {
        for (int i = 0; i < characterSprites.Length; i++)
        {
            if(characterSprites[i].spriteName == stateName)
            {
                return characterSprites[i].sprite;
            }
        }

        Debug.LogError("해당되는 스프라이트가 없음 : " + stateName);
        return null;
    }
}