using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBug04A : EnemyBug01
{
    public override EnemyType enemyType => EnemyType.BUG04A;
    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug04A>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug04A enemy = Global.Pool.GetItem<EnemyBug04A>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);

        return enemy;
    }
}
