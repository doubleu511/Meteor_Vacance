using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Game;
    public static MapData MapData;
    public static PlayerController Player;
    public static WaveManager Wave;

    private void Awake()
    {
        if (!Game)
        {
            Game = this;
        }

        MapData = FindObjectOfType<MapData>();
        Player = FindObjectOfType<PlayerController>();
        Wave = FindObjectOfType<WaveManager>();
    }

    [HideInInspector] public int currentCost = 0;
    private const float CostRefillTime = 0.1f;
    private float costTimer = 0.0f;

    private void Start()
    {
        InGameUI.UI.Cost.SetCost(currentCost);
    }

    private void Update()
    {
        if (currentCost < 99)
        {
            costTimer += Time.deltaTime;

            if (costTimer >= CostRefillTime)
            {
                costTimer -= CostRefillTime;
                currentCost = Mathf.Clamp(currentCost + 1, 0, 99);
                InGameUI.UI.Cost.SetCost(currentCost);
            }
        }
        else
        {
            costTimer = 0;
        }

        InGameUI.UI.Cost.SetCostValue(costTimer / CostRefillTime);
    }

    public void RemoveCost(int costValue)
    {
        currentCost = Mathf.Clamp(currentCost - costValue, 0, 99);
        InGameUI.UI.Cost.SetCost(currentCost);
    }
}
