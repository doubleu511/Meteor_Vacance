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

    [SerializeField] GameObject target = null; // test

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLookingDir(Vector2Int.left);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
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

        if (detectDir == Vector2.down)
        {
            if(target != null)
            {
                Vector2 dir = target.transform.position - transform.position;
                if(dir.x > 1)
                {
                    animationLookDir = Vector2Int.right;
                }
                else if (dir.x < - 1)
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

        playerDetect.SetDetectRange(dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print(collision);
    }
}