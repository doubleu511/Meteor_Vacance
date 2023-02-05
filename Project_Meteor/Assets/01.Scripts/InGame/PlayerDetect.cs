using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    [System.Serializable]
    public struct DetectRange
    {
        public GameObject detectColl;
        public GameObject rangeEffect; 

        public void SetActive(bool value)
        {
            detectColl.SetActive(value);
            rangeEffect.SetActive(value);
        }
    }

    [SerializeField] DetectRange leftDetect;
    [SerializeField] DetectRange rightDetect;
    [SerializeField] DetectRange upDetect;
    [SerializeField] DetectRange downDetect;

    public void SetDetectRange(Vector2Int dir)
    {
        leftDetect.SetActive(dir.x == -1);
        rightDetect.SetActive(dir.x == 1);
        upDetect.SetActive(dir.y == 1);
        downDetect.SetActive(dir.y == -1);
    }
}
