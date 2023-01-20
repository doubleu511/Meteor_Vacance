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
    public int background;
    public int speakingDir; // 0�̸� ����, 1�̸� ������, 2�� �Ѵ�
    public eCharacter chracter_1;
    public eCharacter chracter_2;

    [TextArea(3, 4)]
    public string[] eventMethod;

    public bool isStopAtSkip = false;
    // ...�� �߰�
}
