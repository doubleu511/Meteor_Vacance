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

    [Header("Each Enemy Properties")]
    [SerializeField] float healingScale = 0.5f;
    [SerializeField] float waitHealingMin = 5;
    [SerializeField] float waitHealingMax = 10;
    [SerializeField] float healingDuration = 4;
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
        healingCycleCo = StartCoroutine(HealCycle());
        isTryHeal = false;
    }

    protected override void Disappear(bool kill)
    {
        base.Disappear(kill);

        if (healingCycleCo != null)
        {
            StopCoroutine(healingCycleCo);
        }
    }

    private IEnumerator HealCycle()
    {
        while(true)
        {
            float randomWaitTime = Random.Range(waitHealingMin, waitHealingMax);
            yield return new WaitForSeconds(randomWaitTime);

            enemyAnimator.SetBool("isHeal", true);
            isTryHeal = true;
            moveSpeed = 0;

            yield return new WaitForSeconds(healingDuration);

            enemyAnimator.SetBool("isHeal", false);
            isTryHeal = false;
            moveSpeed = initSpeed;
        }
    }

    public override void TakeDamage(float amount)
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
}
