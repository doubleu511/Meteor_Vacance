using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public Action OnDamaged;
    public Action OnDied;
    public Action OnDisappeared;

    [SerializeField]
    private int healthAmountMax = 100;
    private float curHealthAmount;

    private void Awake()
    {
        curHealthAmount = healthAmountMax;
    }

    public void Damage(float damageAmount)
    {
        curHealthAmount -= damageAmount;
        curHealthAmount = Mathf.Clamp(curHealthAmount, 0, healthAmountMax);
    }

    public bool IsDead()
    {
        return curHealthAmount == 0;
    }

    public bool IsFullHealth()
    {
        return curHealthAmount == healthAmountMax;
    }

    public float GetHealthAmount()
    {
        return curHealthAmount;
    }

    public float GetHealthAmountNormalized()
    {
        return curHealthAmount / healthAmountMax;
    }

    public void SetHealthAmountMax(int hpAmountMax, bool updateHpAmount)
    {
        healthAmountMax = hpAmountMax;
        if (updateHpAmount)
        {
            curHealthAmount = hpAmountMax;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        Damage(damageAmount);
        OnDamaged?.Invoke();

        if (IsDead())
        {

            OnDied?.Invoke();
        }
    }

    public void Disappear()
    {
        OnDisappeared?.Invoke();
    }
}
