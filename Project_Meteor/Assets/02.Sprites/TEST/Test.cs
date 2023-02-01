using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Image BGImg = null;
    private Vector2 offset = Vector2.zero;

    [SerializeField] float speed = 1;

    [SerializeField] Material testMat;

    // Start is called before the first frame update
    void Start()
    {
        BGImg = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += speed * Time.deltaTime;

        testMat.SetTextureOffset("_MainTex", offset);
        Material mat = new Material(testMat);
        BGImg.material = mat;
    }
}
