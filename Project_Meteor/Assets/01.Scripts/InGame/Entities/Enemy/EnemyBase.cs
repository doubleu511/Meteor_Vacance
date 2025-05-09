using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EnemyType
{
    BUG01 = 1,
    BUG01A = 2,
    BUG02 = 3,
    BUG02A = 4,
    BUG03 = 5,
    BUG03A = 6,
    BUG04 = 7,
    BUG04A = 8,
    BUG05 = 9,
    BUGBOSS = 10,
}

public abstract class EnemyBase : MonoBehaviour
{
    public virtual EnemyType enemyType => EnemyType.BUG01;

    protected Collider2D coll;
    protected SpriteRenderer enemySpriteRenderer;
    protected Coroutine moveCoroutine;

    [Header("Enemy Components")]
    [SerializeField] protected Transform enemyScaler;
    [SerializeField] protected Animator enemyAnimator;
    [SerializeField] protected SpriteRenderer shadowSprite;

    [SerializeField] protected float moveSpeed = 5f;
    protected float speedScale = 1f;

    [SerializeField] float initArmor = 10f;
    protected float debuffArmorScale = 1f;
    protected float Armor { get { return initArmor + armorBonusAmount; } }

    [Header("Particles")]
    [SerializeField] protected GameObject armorBreakEffect;
    [SerializeField] protected SpriteRenderer armorEffect;
    [SerializeField] ParticleSystem dieParticle;
    [SerializeField] Transform hitTransform;

    protected HealthSystem healthSystem;
    protected List<EnemyBug03> armorBuffList = new List<EnemyBug03>();
    protected EnemyBugBoss protectedBoss;
    protected float armorBonusAmount = 0f;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        healthSystem = GetComponent<HealthSystem>();
        enemySpriteRenderer = enemyAnimator.GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
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

    public virtual void Init(WaypointSO wayPoint, Vector2 wayPointOffset, bool flipX, bool flipY)
    {
        coll.enabled = true;
        enemyAnimator.enabled = true;
        enemyAnimator.transform.localScale = Vector3.one;
        shadowSprite.color = new Color(1, 1, 1, 0);
        shadowSprite.DOFade(1, 0.5f);

        enemySpriteRenderer.color = new Color(0, 0, 0, 0);
        enemySpriteRenderer.DOFade(1, 0.5f);
        enemySpriteRenderer.DOColor(Color.white, 0.25f).SetDelay(0.1f);
        armorEffect.color = new Color(armorEffect.color.r, armorEffect.color.g, armorEffect.color.b, 0);
        debuffArmorScale = 1f;

        Vector2Int flipPin = wayPoint.GetFlipedPos(wayPoint.enemyWayPoints[0].enemyWayPoint, flipX, flipY);
        Vector3 targetPinPos = GameManager.MapData.Position2D[flipPin.y, flipPin.x].position;
        targetPinPos.x += wayPointOffset.x;
        targetPinPos.y += wayPointOffset.y;
        transform.position = targetPinPos;

        moveCoroutine = StartCoroutine(MoveCoroutine(wayPoint, wayPointOffset, flipX, flipY));
        healthSystem.SetHealthAmountMax();
    }

    protected IEnumerator MoveCoroutine(WaypointSO wayPoint, Vector2 wayPointOffset, bool flipX, bool flipY)
    {
        int currentPlayIndex = 0;

        while (true)
        {
            if (wayPoint.enemyWayPoints.Length <= currentPlayIndex) break;

            Vector2Int flipPin = wayPoint.GetFlipedPos(wayPoint.enemyWayPoints[currentPlayIndex].enemyWayPoint, flipX, flipY);
            Vector3 targetPinPos = GameManager.MapData.Position2D[flipPin.y, flipPin.x].position;
            targetPinPos.x += wayPointOffset.x;
            targetPinPos.y += wayPointOffset.y;
            Vector3 dir = targetPinPos - transform.position;

            if (dir.sqrMagnitude < 0.01f)
            {
                yield return new WaitForSeconds(wayPoint.enemyWayPoints[currentPlayIndex].waitTime);
                currentPlayIndex++;
                continue;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPinPos, moveSpeed * speedScale * Time.deltaTime);
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

        Vector3 playerDir = GameManager.Player.transform.position - transform.position + (Vector3)wayPointOffset;
        if (playerDir.sqrMagnitude <= 0.4f)
        {
            // 플레이어에게 도착
            AttackPlayer();
        }
    }

    protected virtual void AttackPlayer()
    {
        GameManager.Player.TakeDamage(1);
        Disappear(false);
    }

    public virtual void TakeDamage(float amount)
    {
        if (protectedBoss != null)
        {
            protectedBoss.TakeDamage((amount - Armor * debuffArmorScale) * 0.8f);
            healthSystem.TakeDamage((amount - Armor * debuffArmorScale) * 0.2f);
        }
        else
        {
            healthSystem.TakeDamage(amount - Armor * debuffArmorScale);
        }
    }

    protected virtual void Die()
    {
        dieParticle.Play();
        Global.Sound.Play("SFX/Battle/b_enemy_dead_n", eSound.Effect);
        Disappear(true);
    }

    protected virtual void Disappear(bool kill)
    {
        coll.enabled = false;
        enemyAnimator.enabled = false;
        GameManager.Player.KillTargetHandle(this, !kill);

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        healthSystem.Disappear();
        debuffDuration = 0f;
        if(armorBuffList.Count > 0) armorEffect.DOFade(0, 0.5f);
        armorBuffList.Clear();
        armorBonusAmount = 0f;

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

    protected float debuffDuration = 0f;
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
        debuffArmorScale = 1 - amount;
        armorBreakEffect.SetActive(true);

        while (debuffDuration > 0f)
        {
            debuffDuration -= Time.deltaTime;

            yield return null;
        }

        debuffArmorScale = 1f;
        armorBreakEffect.SetActive(false);
    }

    protected internal void AddArmorBuffWard(EnemyBug03 wardBug)
    {
        armorBuffList.Add(wardBug);
        SetArmorBuffLevel();
    }

    protected internal void RemoveArmorBuffWard(EnemyBug03 wardBug)
    {
        armorBuffList.Remove(wardBug);
        SetArmorBuffLevel();
    }

    protected internal void AddBossGift(EnemyBugBoss boss)
    {
        protectedBoss = boss;
    }

    protected internal void RemoveBossGift()
    {
        protectedBoss = null;
    }

    private void SetArmorBuffLevel()
    {
        float maxArmorBonus = 0f;
        for (int i = 0; i < armorBuffList.Count; i++)
        {
            if(armorBuffList[i].gainArmorBonus > maxArmorBonus)
            {
                maxArmorBonus = armorBuffList[i].gainArmorBonus;
            }
        }

        armorEffect.DOComplete();
        if (armorBuffList.Count == 0)
        {
            armorEffect.DOFade(0, 0.5f);
        }
        else
        {
            armorEffect.DOFade(1, 0.5f);
        }

        armorBonusAmount = maxArmorBonus;
    }
}
