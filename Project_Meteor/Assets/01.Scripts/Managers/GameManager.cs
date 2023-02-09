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
    private const float CostRefillTime = 1;
    private float costTimer = 0.0f;

    private void Update()
    {
        costTimer += Time.deltaTime;

        if(costTimer >= CostRefillTime)
        {
            costTimer -= CostRefillTime;
            currentCost = Mathf.Clamp(currentCost + 1, 0, 99);
        }

        InGameUI.Cost.SetCostValue(currentCost, costTimer / CostRefillTime);
    }
}
