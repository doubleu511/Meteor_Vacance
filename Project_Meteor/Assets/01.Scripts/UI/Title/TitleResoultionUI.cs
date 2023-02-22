using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleResoultionUI : MonoBehaviour
{
    [Header("Option")]
    public TMP_Dropdown graphics_WindowMode;
    public TMP_Dropdown graphics_Resolution;

    private void Start()
    {
        // 설정 드롭다운
        graphics_WindowMode.onValueChanged.AddListener(value => ScreenMode(value));
        graphics_Resolution.onValueChanged.AddListener(value => Resolution(value));

        int fullScreenValue = SecurityPlayerPrefs.GetInt("FULL_SCREEN", 1);
        int resolutionValue = SecurityPlayerPrefs.GetInt("RESOLUTION", 1);

        graphics_WindowMode.value = fullScreenValue;
        graphics_Resolution.value = resolutionValue;
    }

    private void ScreenMode(int value)
    {
        Screen.fullScreen = (value == 0) ? true : false;
        SecurityPlayerPrefs.SetInt("FULL_SCREEN", value);
        Debug.Log($"FullScreen {((value == 0) ? true : false)}");
    }

    private void Resolution(int value)
    {
        switch (value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(960, 540, Screen.fullScreen);
                break;
        }

        SecurityPlayerPrefs.SetInt("RESOLUTION", value);
    }
}
