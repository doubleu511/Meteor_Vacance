using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WaveRush Scriptable Object", menuName = "ScriptableObjects/WaveRush Scriptable Object")]
public class WaveRushSO : ScriptableObject
{
    public WaveTime[] waveTimes;
}