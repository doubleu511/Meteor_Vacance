using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBug04 : EnemyBug01
{
    public override EnemyType enemyType => EnemyType.BUG04;
    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug04>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug04 enemy = Global.Pool.GetItem<EnemyBug04>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);

        return enemy;
    }
}
