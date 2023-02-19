using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBug05 : EnemyBase
{
    public override EnemyType enemyType => EnemyType.BUG05;
    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug05>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug05 enemy = Global.Pool.GetItem<EnemyBug05>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);

        return enemy;
    }


    [SerializeField] EnemyBase respawnPrefab;
    private WaveTime respawnWaveTime;

    protected override void Start()
    {
        base.Start();
    }

    public override void Init(WaypointSO wayPoint, Vector2 wayPointOffset, bool flipX, bool flipY)
    {
        base.Init(wayPoint, wayPointOffset, flipX, flipY);

        respawnWaveTime = new WaveTime();
        respawnWaveTime.enemySpawnTime = 0;
        respawnWaveTime.enemyType = EnemyType.BUG05;
        respawnWaveTime.wayPointOffset = Vector2.zero;
        respawnWaveTime.wayPointSO = wayPoint;
    }

    protected override void Disappear(bool kill)
    {
        base.Disappear(kill);
        GameManager.Wave.StartCoroutine(GameManager.Wave.SpawnDelayCo(respawnWaveTime, EnemyType.BUG05, respawnPrefab));
    }
}
