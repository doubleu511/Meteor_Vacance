using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private HealthSystem healthSystem;

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
        healthSystem.OnDied += () => gameObject.SetActive(false);

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
        barTrm.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);

        barAnimTrm.DOKill();
        barAnimTrm.DOScaleX(healthSystem.GetHealthAmountNormalized(), 0.5f);
    }

    private void UpdateHealthBarVisible()
    {
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