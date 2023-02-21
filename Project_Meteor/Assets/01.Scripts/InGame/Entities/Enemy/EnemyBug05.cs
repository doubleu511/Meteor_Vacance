using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

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
        enemy.respawnWaveTime = waveTime;
        return enemy;
    }


    [SerializeField] int respawnCount = 5;
    private WaveTime respawnWaveTime;
    private int teleportIndex = 0;

    protected override void Start()
    {
        base.Start();
    }

    public override void Init(WaypointSO wayPoint, Vector2 wayPointOffset, bool flipX, bool flipY)
    {
        base.Init(wayPoint, wayPointOffset, flipX, flipY);
    }

    protected override void Die()
    {
        respawnCount--;
        if (respawnCount > 0)
        {
            Disappear(false);
        }
        else
        {
            base.Die();
            teleportIndex = 0;
        }
    }

    protected override void Disappear(bool kill)
    {
        if (respawnCount > 0)
        {
            coll.enabled = false;
            enemyAnimator.enabled = false;
            GameManager.Player.KillTargetHandle(this);

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            healthSystem.Disappear();
            debuffDuration = 0f;
            if (armorBuffList.Count > 0) armorEffect.DOFade(0, 0.5f);
            armorBuffList.Clear();
            armorBonusAmount = 0f;

            Sequence seq = DOTween.Sequence();

            seq.AppendInterval(0.1f);
            seq.AppendCallback(() => armorBreakEffect.SetActive(false));
            seq.Append(enemyAnimator.transform.DOScaleX(0f, 0.4f));
            seq.Join(enemySpriteRenderer.DOColor(Color.black, 0.3f));
            seq.Join(shadowSprite.DOColor(new Color(1, 1, 1, 0), 0.3f));
            seq.AppendInterval(0.5f);
            seq.AppendCallback(() =>
            {
                EnemyWaypointTrailEffect trailEffect = Global.Pool.GetItem<EnemyWaypointTrailEffect>();
                trailEffect.Init(respawnWaveTime);
            });
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                teleportIndex++;
                if (teleportIndex > 4) teleportIndex = 1;
                Init(respawnWaveTime.wayPointSO, respawnWaveTime.wayPointOffset, teleportIndex % 2 == 1, teleportIndex > 2);
            });
        }
        else
        {
            base.Disappear(kill);
        }
    }
}
