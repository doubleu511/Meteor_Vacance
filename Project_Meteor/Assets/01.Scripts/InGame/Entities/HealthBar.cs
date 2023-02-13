using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private HealthSystem healthSystem;
    [SerializeField]
    private bool isAlwaysShow = false;

    private Transform barTrm;
    private Transform barAnimTrm;

    private void Awake()
    {
        barTrm = transform.Find("bar");
        barAnimTrm = transform.Find("barAnim");
    }

    private void Start()
    {
        if (!healthSystem)
        {
            healthSystem = transform.parent.GetComponent<HealthSystem>();
        }

        healthSystem.OnDamaged += CallHealthSystemOnDamaged;
        healthSystem.OnDisappeared += () => gameObject.SetActive(false);

        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void CallHealthSystemOnDamaged()
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        if (healthSystem.IsDead()) return;

        barTrm.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);

        barAnimTrm.DOKill();
        barAnimTrm.DOScaleX(healthSystem.GetHealthAmountNormalized(), 0.5f);
    }

    private void UpdateHealthBarVisible()
    {
        if (isAlwaysShow) return;

        if (healthSystem.IsFullHealth())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}