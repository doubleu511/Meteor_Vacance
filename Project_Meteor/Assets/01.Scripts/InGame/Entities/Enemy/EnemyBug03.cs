using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBug03 : EnemyBase
{
    public override EnemyType enemyType => EnemyType.BUG03;
    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBug03>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBug03 enemy = Global.Pool.GetItem<EnemyBug03>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);

        return enemy;
    }

    [Header("Each Enemy Properties")]
    public float gainArmorBonus = 10f;
    [SerializeField] ParticleSystem wardParticle;
    [SerializeField] GameObject wardColl;

    public override void Init(WaypointSO wayPoint, Vector2 wayPointOffset, bool flipX, bool flipY)
    {
        base.Init(wayPoint, wayPointOffset, flipX, flipY);
        AddArmorBuffWard(this);
        wardParticle.Play();
        wardColl.SetActive(true);
    }

    protected override void Disappear(bool kill)
    {
        base.Disappear(kill);
        wardParticle.Stop();
        wardColl.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.AddArmorBuffWard(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.RemoveArmorBuffWard(this);
        }
    }
}
