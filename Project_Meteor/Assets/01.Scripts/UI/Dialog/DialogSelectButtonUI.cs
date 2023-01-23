using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSelectButtonUI : MonoBehaviour
{
    private Button button;
    [SerializeField] TextMeshProUGUI buttonText;

    private Action onClick;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (onClick != null)
            {
                onClick.Invoke();
            }
        });
    }

    public void Init(string text, Action onClickEvent)
    {
        onClick = null;
        buttonText.text = text;
        onClick += onClickEvent;
        transform.SetAsLastSibling();
    }
}
