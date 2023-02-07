using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerScaler;
    [SerializeField] Animator playerAnimator;
    [SerializeField] PlayerDetect playerDetect;
    [SerializeField] Transform playerDirectArrow;

    [SerializeField] Arrow playerArrowPrefab;
    [SerializeField] Transform arrowStartPos;

    private PlayerStat playerStat;

    private Vector2Int detectDir = Vector2Int.right; // 감지 방향
    private Vector2Int animationLookDir = Vector2Int.right; // 애니메이션 바라보는 방향
    private Quaternion directRot;

    [SerializeField] List<EnemyBase> detectTargets = new List<EnemyBase>();
    private EnemyBase targetEnemy = null;


    private void Awake()
    {
        playerStat = GetComponent<PlayerStat>();
        playerAnimator.SetFloat("AttackSpeedMultiplier", playerStat.playerAttackSpeed);
    }

    private void Start()
    {
        directRot = playerDirectArrow.transform.localRotation;

        Global.Pool.CreatePool<Arrow>(playerArrowPrefab.gameObject, arrowStartPos, 5);
    }

    private void Update()
    {
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

    public void SetTargetNull(EnemyBase target)
    {
        if(targetEnemy == target)
        {
            targetEnemy = null;
        }
    }

    private void Attack()
    {
        if (targetEnemy == null)
        {
            if (detectTargets.Count > 0)
            {
                EnemyBase detectedEnemy = UtilClass.GetClosestObject(transform, detectTargets.ToArray());
                targetEnemy = detectedEnemy;

                playerAnimator.SetTrigger("Shoot");
                playerStat.attackWaitTime = 0;
            }
        }
        else
        {
            playerStat.attackWaitTime += Time.deltaTime;
            if(playerStat.attackWaitTime > 1 / playerStat.playerAttackSpeed)
            {
                // 공격
                playerAnimator.SetTrigger("Shoot");
                playerStat.attackWaitTime = 0;
            }
        }
    }

    public void ArrowAttack()
    {
        Arrow arrow = Global.Pool.GetItem<Arrow>();
        arrow.Init(arrowStartPos.position, targetEnemy, animationLookDir);
        arrow.SetArrowDamage(playerStat.playerDamage);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            detectTargets.Add(enemy);
            playerAnimator.SetBool("EnemyDetected", true);
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