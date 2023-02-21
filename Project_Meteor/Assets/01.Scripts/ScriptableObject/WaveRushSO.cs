using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "WaveRush Scriptable Object", menuName = "ScriptableObjects/WaveRush Scriptable Object")]
public class WaveRushSO : ScriptableObject
{
    public WaveTime[] waveTimes;

    [ContextMenu("SortTime")]
    public void SortTime()
    {
        waveTimes = waveTimes.OrderBy(x => x.enemySpawnTime).ToArray();
    }
}