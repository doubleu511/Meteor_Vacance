using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActEvent : MonoBehaviour
{
    public DialogSO startDialog;
    public UnityEvent onActEnded;
    public Sprite actBackground;
    public AudioClip actBGM;
}