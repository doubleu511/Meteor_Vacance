using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBug01 : EnemyBase
{
    public override EnemyType enemyType => EnemyType.BUG01;

    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug01>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug01 enemy = Global.Pool.GetItem<EnemyBug01>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset);

        return enemy;
    }
}
