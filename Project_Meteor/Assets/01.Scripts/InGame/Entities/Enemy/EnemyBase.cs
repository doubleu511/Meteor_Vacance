using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EnemyType
{
    BUG01 = 1,
}

public abstract class EnemyBase : MonoBehaviour
{
    public virtual EnemyType enemyType => EnemyType.BUG01;

    private Collider2D coll;
    private SpriteRenderer enemySpriteRenderer;
    private Coroutine moveCoroutine;

    [Header("Enemy Components")]
    [SerializeField] Transform enemyScaler;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] SpriteRenderer shadowSprite;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject armorBreakEffect;
    [SerializeField] float initArmor = 10f;
    private float currentArmor = 10f;

    [Header("Particles")]
    [SerializeField] ParticleSystem dieParticle;
    [SerializeField] Transform hitTransform;

    private HealthSystem healthSystem;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        healthSystem = GetComponent<HealthSystem>();
        enemySpriteRenderer = enemyAnimator.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        healthSystem.OnDamaged += () =>
        {
            enemySpriteRenderer.color = Color.red;
            enemySpriteRenderer.DOColor(Color.white, 0.1f);
        };

        healthSystem.OnDied += () =>
        {
            Die();
        }; // 테스트
    }

    public abstract void CreatePool(EnemyBase enemyPrefab);
    public abstract EnemyBase PoolInit(WaveTime waveTime);

    public virtual void Init(WaypointSO wayPoint, Vector2 wayPointOffset)
    {
        coll.enabled = true;
        enemyAnimator.transform.localScale = Vector3.one;
        shadowSprite.color = Color.white;
        enemySpriteRenderer.color = Color.white;
        currentArmor = initArmor;

        Vector3 targetPinPos = GameManager.MapData.Position2D[wayPoint.enemyWayPoints[0].enemyWayPoint.y, wayPoint.enemyWayPoints[0].enemyWayPoint.x].position;
        targetPinPos.x += wayPointOffset.x;
        targetPinPos.y += wayPointOffset.y;
        transform.position = targetPinPos;

        moveCoroutine = StartCoroutine(MoveCoroutine(wayPoint, wayPointOffset));
    }

    private IEnumerator MoveCoroutine(WaypointSO wayPoint, Vector2 wayPointOffset)
    {
        int currentPlayIndex = 0;

        while (true)
        {
            if (wayPoint.enemyWayPoints.Length <= currentPlayIndex) break;

            Vector3 targetPinPos = GameManager.MapData.Position2D[wayPoint.enemyWayPoints[currentPlayIndex].enemyWayPoint.y, wayPoint.enemyWayPoints[currentPlayIndex].enemyWayPoint.x].position;
            targetPinPos.x += wayPointOffset.x;
            targetPinPos.y += wayPointOffset.y;
            Vector3 dir = targetPinPos - transform.position;

            if (dir.sqrMagnitude < 0.01f)
            {
                yield return new WaitForSeconds(wayPoint.enemyWayPoints[currentPlayIndex].waitTime);
                currentPlayIndex++;
                continue;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPinPos, moveSpeed * Time.deltaTime);
            float scale = -0.084f * transform.position.y + 1;
            enemyScaler.transform.localScale = new Vector3(scale, scale, 1);

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

        Vector3 playerDir = GameManager.Player.transform.position - transform.position + (Vector3)wayPointOffset;
        if (playerDir.sqrMagnitude <= 0.4f)
        {
            // 플레이어에게 도착
            GameManager.Player.TakeDamage();
            Disappear(false);
        }
    }

    public void TakeDamage(float amount)
    {
        healthSystem.TakeDamage(amount - currentArmor);
    }

    private void Die()
    {
        dieParticle.Play();
        Disappear(true);
    }

    private void Disappear(bool kill)
    {
        coll.enabled = false;
        GameManager.Player.KillTargetHandle(this, !kill);

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        healthSystem.Disappear();

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => armorBreakEffect.SetActive(false));
        seq.Append(enemyAnimator.transform.DOScaleY(0.85f, 0.4f));
        seq.Join(enemySpriteRenderer.DOColor(Color.black, 0.3f));
        seq.Append(enemySpriteRenderer.DOColor(new Color(0, 0, 0, 0), 0.3f));
        seq.Join(shadowSprite.DOColor(new Color(1, 1, 1, 0), 0.3f));
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public Transform GetHitTransform()
    {
        return hitTransform;
    }

    private float debuffDuration = 0f;
    public void GainDebuff(float duration, float amount)
    {
        if (debuffDuration > 0f)
        {
            debuffDuration = duration;
        }
        else
        {
            debuffDuration = duration;
            StartCoroutine(DebuffCo(amount));
        }
    }

    private IEnumerator DebuffCo(float amount)
    {
        currentArmor = Mathf.Clamp(currentArmor - currentArmor * amount, 0, float.MaxValue);
        armorBreakEffect.SetActive(true);

        while (debuffDuration > 0f)
        {
            debuffDuration -= Time.deltaTime;

            yield return null;
        }

        currentArmor = initArmor;
        armorBreakEffect.SetActive(false);
    }
}
