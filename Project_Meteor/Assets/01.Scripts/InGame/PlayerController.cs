using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] Transform playerScaler;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator playerHitAnimator;
    [SerializeField] PlayerDetect playerDetect;
    [SerializeField] Transform playerDirectArrow;

    [Header("Arrow Prefabs")]
    [SerializeField] Arrow playerArrowPrefab;
    [SerializeField] SpecialArrow playerSpecialArrowPrefab;
    [SerializeField] DefaultArrowEffect defaultArrowHitParticlePrefab;
    [SerializeField] SpecialArrowEffect specialArrowHitParticlePrefab;
    [SerializeField] Transform arrowStartPos;

    private PlayerAbility playerAbility;
    private PlayerStat playerStat;
    private HealthSystem playerHealth;

    private Vector2Int detectDir = Vector2Int.right; // 감지 방향
    private Vector2Int animationLookDir = Vector2Int.right; // 애니메이션 바라보는 방향
    private Quaternion directRot;

    [SerializeField] List<EnemyBase> detectTargets = new List<EnemyBase>();
    private EnemyBase targetEnemy = null;

    private int enemyKillCount = 0;
    private bool isAttackVoiceReady = true;


    private void Awake()
    {
        playerAbility = GetComponent<PlayerAbility>();
        playerStat = GetComponent<PlayerStat>();
        playerHealth = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        Global.Sound.PlayRandom(eSound.Effect, 1, "SFX/Voice/deploy1", "SFX/Voice/deploy2");
        Global.Sound.Play("SFX/Battle/b_char_set");

        directRot = playerDirectArrow.transform.localRotation;

        InGameUI.UI.Info.SetPlayerHeathText((int)playerHealth.GetHealthAmount());
        playerHealth.OnDamaged += () =>
        {
            InGameUI.UI.Info.SetPlayerHeathText((int)playerHealth.GetHealthAmount());
        };

        Global.Pool.CreatePool<Arrow>(playerArrowPrefab.gameObject, arrowStartPos, 5);
        Global.Pool.CreatePool<SpecialArrow>(playerSpecialArrowPrefab.gameObject, arrowStartPos, 5);
        Global.Pool.CreatePool<DefaultArrowEffect>(defaultArrowHitParticlePrefab.gameObject, arrowStartPos, 2);
        Global.Pool.CreatePool<SpecialArrowEffect>(specialArrowHitParticlePrefab.gameObject, arrowStartPos, 5);

        StartCoroutine(AttackVoiceCooldownCo());
    }

    private void Update()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Meteor_Appear"))
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                playerAnimator.SetBool("Appear", false);
            }
            else
            {
                return;
            }
        }

        // 키 입력
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeLookingDir(Vector2Int.left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeLookingDir(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeLookingDir(Vector2Int.up);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeLookingDir(Vector2Int.down);
            }
        }

        Attack();

        if (detectDir == Vector2.down)
        {
            if (targetEnemy != null)
            {
                Vector2 dir = targetEnemy.transform.position - transform.position;
                if (dir.x > 1)
                {
                    animationLookDir = Vector2Int.right;
                }
                else if (dir.x < -1)
                {
                    animationLookDir = Vector2Int.left;
                }
            }
        }

        playerAnimator.SetInteger("dirX", animationLookDir.x);
        playerAnimator.SetInteger("dirY", animationLookDir.y);
    }

    public void KillTargetHandle(EnemyBase target, bool disappear)
    {
        if(targetEnemy == target)
        {
            targetEnemy = null;
        }

        if (!disappear)
        {
            enemyKillCount++;
            InGameUI.UI.Info.SetEnemyKilledText(enemyKillCount);
        }
    }

    public void SetAttackSpeedMultiplier(float attackSpeed)
    {
        playerAnimator.SetFloat("AttackSpeedMultiplier", attackSpeed);
    }

    private void Attack()
    {
        if (detectTargets.Count > 0)
        {
            EnemyBase detectedEnemy = UtilClass.GetClosestObject(transform, detectTargets.ToArray());
            targetEnemy = detectedEnemy;

            playerAnimator.SetTrigger("Shoot");
            playerStat.attackWaitTime = 0;
        }

        if (targetEnemy != null)
        {
            playerStat.attackWaitTime += Time.deltaTime;
            if (playerStat.attackWaitTime > 1 / playerStat.playerAttackSpeed)
            {
                // 공격
                playerAnimator.SetTrigger("Shoot");
                playerStat.attackWaitTime = 0;
            }
        }
    }

    public void ArrowAttack()
    {
        if (targetEnemy != null)
        {
            Arrow arrow = Global.Pool.GetItem<Arrow>();
            arrow.Init(arrowStartPos.position, targetEnemy, animationLookDir, (enemy) =>
            {
                DefaultArrowEffect effect = Global.Pool.GetItem<DefaultArrowEffect>();

                effect.transform.SetParent(enemy.GetHitTransform());
                effect.transform.localPosition = Vector3.zero;
                effect.transform.localScale = Vector3.one;
                playerAbility.AddPoint(1);

                Global.Sound.Play("SFX/Battle/b_enemy_hit");
            });
            arrow.SetArrowDamage(playerStat.playerDamage);
            Global.Sound.Play("SFX/Battle/b_char_arrowshot");
        }
    }

    public void SpecialArrowAttack(EnemyBase target, float damageScale, float debuffAmount)
    {
        SpecialArrow arrow = Global.Pool.GetItem<SpecialArrow>();
        arrow.Init(arrowStartPos.position, target, animationLookDir, (enemy) =>
        {
            SpecialArrowEffect effect = Global.Pool.GetItem<SpecialArrowEffect>();

            effect.transform.SetParent(enemy.GetHitTransform());
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localScale = Vector3.one;

            target.GainDebuff(5, debuffAmount);
        });
        arrow.SetArrowDamage(playerStat.playerDamage * damageScale);
    }

    private void ChangeLookingDir(Vector2Int dir)
    {
        float turnAngle = Vector2.SignedAngle(detectDir, dir);
        directRot *= Quaternion.Euler(0, 0, turnAngle);
        playerDirectArrow.transform.DOLocalRotateQuaternion(directRot, 0.25f);

        detectDir = dir;
        Vector2Int lookDir = dir;

        if (dir == Vector2Int.down)
        {
            if (animationLookDir == Vector2Int.up)
            {
                lookDir = Vector2Int.right;
            }
        }

        if(animationLookDir != lookDir)
        {
            playerScaler.DOKill();
            playerScaler.DOScaleY(0.8f, 0.1f).OnComplete(() =>
            {
                playerScaler.DOScaleY(1f, 0.1f);
            });
        }

        if (lookDir != Vector2Int.down)
        {
            animationLookDir = lookDir;
        }

        targetEnemy = null;
        playerDetect.SetDetectRange(dir);
    }

    private Coroutine damageTakeCoroutine = null;
    public void TakeDamage()
    {
        InGameUI.UI.Info.ShowRedBlur();
        playerHealth.TakeDamage(1);
        Global.Sound.Play("SFX/Battle/b_ui_alarmenter");

        if (damageTakeCoroutine != null)
        {
            StopCoroutine(damageTakeCoroutine);
            playerHitAnimator.Rebind();
        }
        damageTakeCoroutine = StartCoroutine(TakeDamageCo());
    }

    private IEnumerator TakeDamageCo()
    {
        playerHitAnimator.gameObject.SetActive(true);
        playerAnimator.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.6f);
        playerHitAnimator.gameObject.SetActive(false);
        playerAnimator.GetComponent<SpriteRenderer>().enabled = true;
    }

    private IEnumerator AttackVoiceCooldownCo()
    {
        while(true)
        {
            float cooldown = Random.Range(30, 45);
            yield return new WaitForSeconds(cooldown);
            isAttackVoiceReady = true;
        }
    }

    public List<EnemyBase> GetDetectEnemies()
    {
        return detectTargets;
    }

    public Vector2Int GetDetectDirection()
    {
        return detectDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            detectTargets.Add(enemy);
            playerAnimator.SetBool("EnemyDetected", true);

            if(detectTargets.Count == 1)
            {
                if(isAttackVoiceReady)
                {
                    Global.Sound.PlayRandom(eSound.Effect, 1, "SFX/Voice/fight1", "SFX/Voice/fight2", "SFX/Voice/fight3", "SFX/Voice/fight4");
                    isAttackVoiceReady = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            detectTargets.Remove(enemy);
            if (detectTargets.Count == 0)
            {
                playerAnimator.SetBool("EnemyDetected", false);
            }
        }
    }
}