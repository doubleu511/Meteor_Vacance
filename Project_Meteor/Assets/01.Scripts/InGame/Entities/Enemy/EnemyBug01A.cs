using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBug01A : EnemyBase
{
    public override EnemyType enemyType => EnemyType.BUG01A;

    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug01A>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug01A enemy = Global.Pool.GetItem<EnemyBug01A>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);

        return enemy;
    }
}
