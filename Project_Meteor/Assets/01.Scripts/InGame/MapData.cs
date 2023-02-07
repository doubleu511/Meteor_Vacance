using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public Transform[,] Position3D;
    public Transform[,] Position2D;

    [SerializeField] Transform mapPin3D;

    private void Awake()
    {
        InitPins(mapPin3D, ref Position3D);
    }

    private void InitPins(Transform pinParent, ref Transform[,] transforms)
    {
        int rowCount = pinParent.childCount;
        int colCount = pinParent.GetChild(0).childCount;

        transforms = new Transform[rowCount, colCount];
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                transforms[i, j] = pinParent.GetChild(i).GetChild(j);
            }
        }
    }
}
