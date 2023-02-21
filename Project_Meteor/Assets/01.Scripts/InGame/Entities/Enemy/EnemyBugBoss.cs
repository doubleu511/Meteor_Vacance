using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class EnemyBugBoss : EnemyBase
{
    public override EnemyType enemyType => EnemyType.BUGBOSS;
    public override void CreatePool(EnemyBase enemyPrefab)
    {
        Global.Pool.CreatePool<EnemyBugBoss>(enemyPrefab.gameObject, GameManager.Wave.transform, 5);
    }

    public override EnemyBase PoolInit(WaveTime waveTime)
    {
        EnemyBugBoss enemy = Global.Pool.GetItem<EnemyBugBoss>();
        enemy.Init(waveTime.wayPointSO, waveTime.wayPointOffset, waveTime.flipX, waveTime.flipY);
        return enemy;
    }

    [Header("Each Enemy Properties")]
    [SerializeField] int teleportConditionCount = 10;
    [SerializeField] WaypointSO newWayPoint;
    private int directHitCount = 0;
    private bool isTeleported = false;
    private float initSpeed;

    protected override void Start()
    {
        base.Start();
        initSpeed = moveSpeed;
    }

    public override void Init(WaypointSO wayPoint, Vector2 wayPointOffset, bool flipX, bool flipY)
    {
        base.Init(wayPoint, wayPointOffset, flipX, flipY);
    }

    public void HitInstead(float damage)
    {
        BossHitSystem(damage, false);
    }

    private void BossHitSystem(float damage, bool isDirectHit)
    {
        if(isDirectHit)
        {
            directHitCount++;
        }

        if(directHitCount >= teleportConditionCount)
        {
            if (!isTeleported)
            {
                isTeleported = true;
                StopCoroutine(moveCoroutine);
                Teleport();
            }
        }

        base.TakeDamage(damage);
    }

    public override void TakeDamage(float amount)
    {
        BossHitSystem(amount , true);
    }

    private void Teleport()
    {
        coll.enabled = false;
        enemyAnimator.enabled = false;
        GameManager.Player.KillTargetHandle(this);
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(0.1f);
        seq.Append(enemyAnimator.transform.DOScaleX(0f, 0.4f));
        seq.Join(enemySpriteRenderer.DOColor(Color.black, 0.3f));
        seq.Join(shadowSprite.DOColor(new Color(1, 1, 1, 0), 0.3f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            transform.position = GameManager.MapData.Position2D[newWayPoint.enemyWayPoints[0].enemyWayPoint.y, newWayPoint.enemyWayPoints[0].enemyWayPoint.x].position;
            moveCoroutine = StartCoroutine(MoveCoroutine(newWayPoint, Vector2.zero, false, false));
        });
        seq.Append(enemyAnimator.transform.DOScaleX(1, 0.4f));
        seq.Join(enemySpriteRenderer.DOColor(Color.white, 0.3f));
        seq.Join(shadowSprite.DOColor(new Color(1, 1, 1, 1), 0.3f));
        seq.AppendCallback(() =>
        {
            coll.enabled = true;
            enemyAnimator.enabled = true;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null && enemy != this)
        {
            print("asd");
            enemy.AddBossGift(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null && enemy != this)
        {
            enemy.RemoveBossGift();
        }
    }
}
