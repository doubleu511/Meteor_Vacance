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

    public eCharacter characterUID; // ĳ���� ������ enum
    public string characterName; // ���������� ���̴� �̸�
    
    public CharacterSprite[] characterSprites; // ĳ���� ��������Ʈ��

    public Sprite FindSpriteState(string stateName)
    {
        for (int i = 0; i < characterSprites.Length; i++)
        {
            if(characterSprites[i].spriteName == stateName)
            {
                return characterSprites[i].sprite;
            }
        }

        Debug.LogError("�ش�Ǵ� ��������Ʈ�� ���� : " + stateName);
        return null;
    }
}