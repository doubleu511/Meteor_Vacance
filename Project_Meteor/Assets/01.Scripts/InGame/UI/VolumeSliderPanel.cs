using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderPanel : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider voiceSlider;

    [SerializeField] TextMeshProUGUI bgmSliderValue;
    [SerializeField] TextMeshProUGUI sfxSliderValue;
    [SerializeField] TextMeshProUGUI voiceSliderValue;

    void Start()
    {
        SetVolumeSlider(bgmSlider, Global.Sound.GetVolume(eSound.Bgm), bgmSliderValue);
        SetVolumeSlider(sfxSlider, Global.Sound.GetVolume(eSound.Effect), sfxSliderValue);
        SetVolumeSlider(voiceSlider, Global.Sound.GetVolume(eSound.Voice), voiceSliderValue);

        bgmSlider.onValueChanged.AddListener((x) => SetVolume(eSound.Bgm, (int)x, bgmSliderValue));
        sfxSlider.onValueChanged.AddListener((x) => SetVolume(eSound.Effect, (int)x, sfxSliderValue));
        voiceSlider.onValueChanged.AddListener((x) => SetVolume(eSound.Voice, (int)x, voiceSliderValue));
    }

    private void SetVolumeSlider(Slider slider, int volume, TextMeshProUGUI text)
    {
        slider.value = volume;
        text.text = $"{volume}";
    }

    private void SetVolume(eSound type, int volume, TextMeshProUGUI text)
    {
        Global.Sound.SetVolume(type, volume, true);
        text.text = $"{volume}";
    }
}
