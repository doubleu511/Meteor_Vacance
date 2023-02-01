using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlashRangeEffect : MonoBehaviour
{
    [SerializeField] Image[] slashImgs = null;
    [SerializeField] float speed = 1;
    [SerializeField] Material slashMat;

    private Vector2 offset = Vector2.zero;
    private Material tempSlashMat;
    private bool slashFlag = false;

    void Start()
    {
        tempSlashMat = new Material(slashMat);
    }

    void Update()
    {
        offset.x += speed * Time.deltaTime;

        slashMat.SetTextureOffset("_MainTex", offset);
        tempSlashMat.CopyPropertiesFromMaterial(slashMat);

        if (slashFlag)
        {
            for(int i = 0; i<slashImgs.Length;i++)
            {
                if (slashImgs[i].gameObject.activeSelf)
                {
                    slashImgs[i].material = tempSlashMat;
                }
            }
        }
        else
        {
            for (int i = 0; i < slashImgs.Length; i++)
            {
                if (slashImgs[i].gameObject.activeSelf)
                {
                    slashImgs[i].material = slashMat;
                }
            }
        }

        slashFlag = !slashFlag;
    }
}
