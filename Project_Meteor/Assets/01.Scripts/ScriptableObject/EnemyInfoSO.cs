using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo Scriptable Object", menuName = "ScriptableObjects/EnemyInfo Scriptable Object")]
public class EnemyInfoSO : ScriptableObject
{
    public EnemyType enemyType;

    public string enemyName;
    [TextArea(5, 5)]
    public string enemyLore;
    public Sprite enemyIcon;
}