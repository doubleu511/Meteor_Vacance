using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBug02A : EnemyBug02
{
    public override EnemyType enemyType => EnemyType.BUG02A;
    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug02A>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug02A enemy = Global.Pool.GetItem<EnemyBug02A>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);

        return enemy;
    }
}
