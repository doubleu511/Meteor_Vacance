using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBug02 : EnemyBase
{
    public override EnemyType enemyType => EnemyType.BUG02;
    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug02>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug02 enemy = Global.Pool.GetItem<EnemyBug02>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);

        return enemy;
    }

    private float initSpeed;

    protected override void Start()
    {
        base.Start();
        initSpeed = moveSpeed;
    }

    private IEnumerator HealCycle()
    {
        while(true)
        {

        }
    }
}
