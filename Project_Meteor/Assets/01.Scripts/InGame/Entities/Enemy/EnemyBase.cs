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

    [Header("Particles")]
    [SerializeField] ParticleSystem dieParticle;
    [SerializeField] Transform hitTransform;

    public HealthSystem Health { get; private set; }

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        Health = GetComponent<HealthSystem>();
        enemySpriteRenderer = enemyAnimator.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Health.OnDamaged += () =>
        {
            enemySpriteRenderer.color = Color.red;
            enemySpriteRenderer.DOColor(Color.white, 0.1f);
        };

        Health.OnDied += () =>
        {
            Die();
        }; // 테스트
    }

    public abstract void CreatePool(EnemyBase enemyPrefab);
    public abstract EnemyBase PoolInit(Vector2Int[] wayPoints);

    public virtual void Init(Vector2Int[] wayPoints)
    {
        coll.enabled = true;
        enemyAnimator.transform.localScale = Vector3.one;
        shadowSprite.color = Color.white;
        enemySpriteRenderer.color = Color.white;

        Vector3 targetPinPos = GameManager.MapData.Position2D[wayPoints[0].y, wayPoints[0].x].position;
        transform.position = targetPinPos;

        moveCoroutine = StartCoroutine(MoveCoroutine(wayPoints));
    }

    private IEnumerator MoveCoroutine(Vector2Int[] wayPoints)
    {
        int currentPlayIndex = 0;

        while (true)
        {
            if (wayPoints.Length > currentPlayIndex)
            {
                Vector3 targetPinPos = GameManager.MapData.Position2D[wayPoints[currentPlayIndex].y, wayPoints[currentPlayIndex].x].position;
                Vector3 dir = targetPinPos - transform.position;

                if (dir.sqrMagnitude >= 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPinPos, moveSpeed * Time.deltaTime);
                    float scale = -0.084f * transform.position.y + 1;
                    enemyScaler.transform.localScale = new Vector3(scale, scale, 1);

                    if(dir.x > 1f)
                    {
                        enemySpriteRenderer.flipX = false;
                    }
                    else if (dir.x < -1f)
                    {
                        enemySpriteRenderer.flipX = true;
                    }
                }
                else
                {
                    currentPlayIndex++;
                }
            }
            else
            {
                break;
            }
            yield return null;
        }

        Vector3 playerDir = GameManager.Player.transform.position - transform.position;
        if (playerDir.sqrMagnitude <= 0.4f)
        {
            // 플레이어에게 도착
            GameManager.Player.TakeDamage();
            Disappear(false);
        }
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
        StopCoroutine(moveCoroutine);
        Health.Disappear();

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(0.1f);
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
}
