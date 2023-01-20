using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharacter
{
    NONE,
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
}