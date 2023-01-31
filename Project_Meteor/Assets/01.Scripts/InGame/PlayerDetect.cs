using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    [SerializeField] Transform leftDetect;
    [SerializeField] Transform rightDetect;
    [SerializeField] Transform upDetect;
    [SerializeField] Transform downDetect;

    public void SetDetectRange(Vector2Int dir)
    {
        leftDetect.gameObject.SetActive(dir.x == -1);
        rightDetect.gameObject.SetActive(dir.x == 1);
        upDetect.gameObject.SetActive(dir.y == 1);
        downDetect.gameObject.SetActive(dir.y == -1);
    }
}
