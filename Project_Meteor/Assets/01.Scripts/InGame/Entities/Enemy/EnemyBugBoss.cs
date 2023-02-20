using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEditor.PackageManager;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

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
    [SerializeField] float healingScale = 0.5f;
    [SerializeField] float waitHealingTime = 10;
    [SerializeField] float healingDuration = 4;
    [SerializeField] int teleportConditionCount = 5;
    [SerializeField] Transform[] teleportWaypoints;
    private int teleportWaypointIndex = 0;
    private int directHitCount = 0;
    private float initSpeed;
    private Coroutine healingCycleCo;
    private bool isTryHeal = false;

    protected override void Start()
    {
        base.Start();
        initSpeed = moveSpeed;
    }

    public override void Init(WaypointSO wayPoint, Vector2 wayPointOffset, bool flipX, bool flipY)
    {
        base.Init(wayPoint, wayPointOffset, flipX, flipY);
        teleportWaypoints = new Transform[]
{
            GameManager.MapData.Position2D[2, 2],
            GameManager.MapData.Position2D[2, 8],
};

        StopCoroutine(moveCoroutine);
        StartCoroutine(MoveWaypoint(teleportWaypoints[teleportWaypointIndex]));

        healingCycleCo = StartCoroutine(HealCycle());
        isTryHeal = false;
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
            directHitCount = 0;
            teleportWaypointIndex = (teleportWaypointIndex + 1) % teleportWaypoints.Length;
            Teleport(teleportWaypoints[teleportWaypointIndex]);
        }

        HealOrDamage(damage);
    }

    public override void TakeDamage(float amount)
    {
        BossHitSystem(amount , true);
    }

    private void HealOrDamage(float amount)
    {
        if (!isTryHeal)
        {
            base.TakeDamage(amount);
        }
        else
        {
            healthSystem.HealHealth(amount * healingScale);
        }
    }

    private IEnumerator HealCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitHealingTime);

            enemyAnimator.SetBool("isHeal", true);
            isTryHeal = true;

            yield return new WaitForSeconds(healingDuration);

            enemyAnimator.SetBool("isHeal", false);
            isTryHeal = false;
        }
    }

    private void Teleport(Transform waypoint)
    {
        coll.enabled = false;
        GameManager.Player.KillTargetHandle(this);
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(0.1f);
        seq.Append(enemyAnimator.transform.DOScaleX(0f, 0.4f));
        seq.Join(enemySpriteRenderer.DOColor(Color.black, 0.3f));
        seq.Join(shadowSprite.DOColor(new Color(1, 1, 1, 0), 0.3f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            transform.position = waypoint.position;
        });
        seq.Append(enemyAnimator.transform.DOScaleX(1, 0.4f));
        seq.Join(enemySpriteRenderer.DOColor(Color.white, 0.3f));
        seq.Join(shadowSprite.DOColor(new Color(1, 1, 1, 1), 0.3f));
        seq.AppendCallback(() =>
        {
            coll.enabled = true;
        });
    }

    private IEnumerator MoveWaypoint(Transform wayPoint)
    {
        while (true)
        {
            Vector3 dir = wayPoint.position - transform.position;

            if(dir.sqrMagnitude < 0.01f)
            {
                break;
            }

            transform.position = Vector3.MoveTowards(transform.position, wayPoint.position, moveSpeed * Time.deltaTime);
            speedScale = -0.084f * transform.position.y + 1;
            enemyScaler.transform.localScale = new Vector3(speedScale, speedScale, 1);

            if (dir.x > 1f)
            {
                enemySpriteRenderer.flipX = false;
            }
            else if (dir.x < -1f)
            {
                enemySpriteRenderer.flipX = true;
            }

            yield return null;
        }

        Vector3 playerDir = GameManager.Player.transform.position - transform.position;
        if (playerDir.sqrMagnitude <= 0.4f)
        {
            // 플레이어에게 도착
            GameManager.Player.TakeDamage(5);
            Disappear(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null && enemy != this)
        {
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
