using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] private float flashTime = 0f;
    Color originColor = Color.white;

    protected string myName;
    public float moveSpeed = 0f;
    public float damage = 3f;

    public float aliveTime = 0f;
    public float movedDistance = 0f;

    private int targetWayPointIndex = 0;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    protected virtual void Start()
    {
        aliveTime = 0f;
    }

    // µ¥¹ÌÁö ÀÔ¾úÀ» ¶§ ±ôºý±ôºý Ã³¸®
    public void EnemyFlashStart()
    {
        sprite.color = Color.red;
        Invoke("EnemyFlashStop", flashTime);
    }

    void EnemyFlashStop()
    {
        sprite.color = originColor;
    }

    protected virtual void Update()
    {
        aliveTime += Time.deltaTime;
        movedDistance = aliveTime * moveSpeed;
    }

    public void WaveStatControl(int wave)
    {
        float value_f = (wave * Mathf.Pow(1.5f, 0)) * 100;
        int value = (int)value_f;

        healthSystem.SetHealthAmountMax(value, true); // Ã¼·Â Á¶Àý
    }
}
