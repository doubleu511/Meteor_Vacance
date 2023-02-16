using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StopwatchUI : MonoBehaviour
{
    private string m_Timer = @"00:00.00";
    private bool m_IsPlaying = true;
    public float m_TotalSeconds;
    [SerializeField] TextMeshProUGUI m_Text;

    void Update()
    {
        if (m_IsPlaying)
        {
            m_Timer = StopwatchTimer();
        }

        if (m_Text)
            m_Text.text = m_Timer;
    }

    public void StopTimer()
    {
        m_IsPlaying = false;
    }

    string StopwatchTimer()
    {
        m_TotalSeconds += Time.deltaTime;
        TimeSpan timespan = TimeSpan.FromSeconds(m_TotalSeconds);
        string timer = string.Format("{0:00}:{1:00}.{2:00}",
            (int)timespan.TotalMinutes, timespan.Seconds, timespan.Milliseconds / 10);

        return timer;
    }
}