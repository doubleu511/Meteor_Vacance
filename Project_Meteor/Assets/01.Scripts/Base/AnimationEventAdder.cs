using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventAdder : MonoBehaviour
{
    [SerializeField] UnityEvent callback;

    private void PlayEvent()
    {
        if (callback != null)
        {
            callback.Invoke();
        }
    }
}
