using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] PlayerDetect playerDetect;

    private Vector2Int detectDir = Vector2Int.right; // 감지 방향
    private Vector2Int animationLookDir = Vector2Int.right; // 애니메이션 바라보는 방향

    [SerializeField] List<EnemyBase> detectTargets = new List<EnemyBase>();
    private EnemyBase targetEnemy = null;

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

        if(targetEnemy == null)
        {
            if (detectTargets.Count > 0)
            {
                EnemyBase detectedEnemy = UtilClass.GetClosestObject(transform, detectTargets.ToArray());
                targetEnemy = detectedEnemy;
            }
        }

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

    private void ChangeLookingDir(Vector2Int dir)
    {
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
            transform.DOKill();
            transform.DOScaleY(0.8f, 0.1f).OnComplete(() =>
            {
                transform.DOScaleY(1f, 0.1f);
            });
        }

        animationLookDir = lookDir;
        targetEnemy = null;
        playerDetect.SetDetectRange(dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            detectTargets.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            detectTargets.Remove(enemy);
        }
    }
}