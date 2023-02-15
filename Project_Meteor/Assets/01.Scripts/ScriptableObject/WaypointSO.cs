using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waypoint Scriptable Object", menuName = "ScriptableObjects/Waypoint Scriptable Object")]
public class WaypointSO : ScriptableObject
{
    [System.Serializable]
    public struct EnemyWayPoint
    {
        public Vector2Int enemyWayPoint;
        public float waitTime;
    }

    public EnemyWayPoint[] enemyWayPoints;
}