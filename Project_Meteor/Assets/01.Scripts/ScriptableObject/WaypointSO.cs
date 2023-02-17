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

    public Vector2Int GetFlipedPos(Vector2Int wayPoint, bool flipX, bool flipY)
    {
        if(flipX)
        {
            wayPoint.x = 10 - wayPoint.x; 
        }

        if(flipY)
        {
            wayPoint.y = 4 - wayPoint.y;
        }

        return wayPoint;
    }
}