using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EnemyBase : MonoBehaviour
{
    protected string myName;
    public float moveSpeed = 0f;
    public float damage = 3f;

    public HealthSystem Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        Health.OnDied += () =>
        {
            Destroy(gameObject);
        }; // Å×½ºÆ®
    }
}
