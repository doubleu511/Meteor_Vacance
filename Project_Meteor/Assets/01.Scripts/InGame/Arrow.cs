using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Arrow : MonoBehaviour
{
    public float m_Speed = 10;
    public float m_HeightArc = 1;

    [SerializeField] int m_ArrowDamage = 40;
    private EnemyBase m_Target;
    private Vector3 m_StartPosition;
    private bool m_IsStart;
    private bool m_IsBasedX;
    private int m_ArcDirScale = 1;
    private float m_UpArrowSpeedScale = 1f;

    public void Init(Vector3 initPos, EnemyBase target, Vector2Int playerAnimationDir)
    {
        transform.position = initPos;
        m_StartPosition = initPos;
        m_Target = target;
        m_IsStart = true;

        Vector2 dirVec = target.transform.position - initPos;
        m_IsBasedX = Mathf.Abs(dirVec.x) > Mathf.Abs(dirVec.y);

        if (dirVec.y <= 0)
        {
            if (playerAnimationDir == Vector2Int.left)
            {
                m_ArcDirScale = -1;
            }
            else if (playerAnimationDir == Vector2Int.right)
            {
                m_ArcDirScale = 1;
            }
            m_UpArrowSpeedScale = 1;
        }
        else
        {
            m_ArcDirScale = dirVec.x < 0 ? 1 : -1;
            m_UpArrowSpeedScale = 0.65f;
        }
    }

    public void SetArrowDamage(int arrowDamage)
    {
        m_ArrowDamage = arrowDamage;
    }

    void Update()
    {
        if (m_IsStart)
        {
            SetMove(m_IsBasedX);
        }
    }

    private void SetMove(bool x)
    {
        Vector3 nextPosition = Vector3.zero;

        if (x)
        {
            float x0 = m_StartPosition.x;
            float x1 = m_Target.transform.position.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.deltaTime);
            float baseY = Mathf.Lerp(m_StartPosition.y, m_Target.transform.position.y, (nextX - x0) / distance);
            float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);
        }
        else
        {
            float y0 = m_StartPosition.y;
            float y1 = m_Target.transform.position.y;
            float distance = y1 - y0;
            float nextY = Mathf.MoveTowards(transform.position.y, y1, m_Speed * m_UpArrowSpeedScale * Time.deltaTime);
            float baseX = Mathf.Lerp(m_StartPosition.x, m_Target.transform.position.x, (nextY - y0) / distance);
            float arc = m_HeightArc * (nextY - y0) * (nextY - y1) / (-0.25f * m_ArcDirScale * distance * distance);
            nextPosition = new Vector3(baseX + arc, nextY, transform.position.z);
        }


        transform.rotation = LookAt2D(nextPosition - transform.position);
        transform.position = nextPosition;

        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (nextPosition.x == m_Target.transform.position.x && nextPosition.y == m_Target.transform.position.y)
            Arrived();
    }

    private void Arrived()
    {
        m_Target.Health.TakeDamage(m_ArrowDamage);
        SetReset();
    }

    private void SetReset()
    {
        m_IsStart = false;
        m_Target = null;
        gameObject.SetActive(false);
    }

    Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
}
