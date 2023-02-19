using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBug03A : EnemyBug03
{
    public override EnemyType enemyType => EnemyType.BUG03A;
    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug03A>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug03A enemy = Global.Pool.GetItem<EnemyBug03A>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);

        return enemy;
    }
}
