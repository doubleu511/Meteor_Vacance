using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog Scriptable Object", menuName = "ScriptableObjects/Dialog Scriptable Object")]
public class DialogSO : ScriptableObject
{
    public int dialogID;
    public List<DialogInfo> dialogInfos = new List<DialogInfo>();
}

[System.Serializable]
public class DialogInfo
{
    public string text;
    public int speakingDir; // 0�̸� ����, 1�̸� ������, 2�� �Ѵ� �ƴ�
    public CharacterTexture chracter_1;
    public CharacterTexture chracter_2;

    [TextArea(3, 4)]
    public string[] eventMethod;
    // ...�� �߰�
}

[System.Serializable]
public struct CharacterTexture
{
    public eCharacter characterType;
    public string characterSpriteName;
}
