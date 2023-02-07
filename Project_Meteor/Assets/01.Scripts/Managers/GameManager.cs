using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Game;
    public static MapData MapData;
    public static PlayerController Player;

    private void Awake()
    {
        if (!Game)
        {
            Game = this;
        }

        MapData = FindObjectOfType<MapData>();
        Player = FindObjectOfType<PlayerController>();
    }

}
