using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            detectDir = animationLookDir = Vector2Int.left;
            playerDetect.SetDetectRange(detectDir);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            detectDir = animationLookDir = Vector2Int.right;
            playerDetect.SetDetectRange(detectDir);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            detectDir = animationLookDir = Vector2Int.up;
            playerDetect.SetDetectRange(detectDir);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            detectDir = Vector2Int.down;

            if (animationLookDir == Vector2Int.up)
            {
                animationLookDir = Vector2Int.right;
            }
            playerDetect.SetDetectRange(detectDir);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print(collision);
    }
}