using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum eSound
{
    Bgm,
    Effect,
    Voice,
    MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
}

public class SoundManager
{
    private AudioMixer _masterAudioMixer;
    AudioSource[] _audioSources = new AudioSource[(int)eSound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound") ?? CreateNewSoundManager();
    }

    private GameObject CreateNewSoundManager()
    {
        GameObject root = new GameObject { name = "@Sound" };
        Object.DontDestroyOnLoad(root);

        string[] soundNames = System.Enum.GetNames(typeof(eSound)); // "Bgm", "Effect"

        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }

        _masterAudioMixer = Global.Resource.Load<AudioMixer>("AudioMixer/Master");

        _audioSources[(int)eSound.Bgm].outputAudioMixerGroup
            = _masterAudioMixer.FindMatchingGroups("BGM")[0];

        _audioSources[(int)eSound.Effect].outputAudioMixerGroup
            = _masterAudioMixer.FindMatchingGroups("SFX")[0];

        _audioSources[(int)eSound.Voice].outputAudioMixerGroup
            = _masterAudioMixer.FindMatchingGroups("VOICE")[0];

        _audioSources[(int)eSound.Bgm].loop
            = true; // bgm 재생기는 무한 반복 재생

        return root;
    }

    public void Clear()
    {
        // 재생기 전부 재생 스탑, 음반 빼기
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // 효과음 Dictionary 비우기
        _audioClips.Clear();
    }

    public void Play(AudioClip audioClip, eSound type = eSound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == eSound.Bgm) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)eSound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // Effect 효과음 재생
        {
            AudioSource audioSource = _audioSources[(int)type];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Play(string path, eSound type = eSound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void PlayRandom(eSound type = eSound.Effect, float pitch = 1.0f, params string[] path)
    {
        int randomIndex = Random.Range(0, path.Length);
        AudioClip audioClip = GetOrAddAudioClip(path[randomIndex], type);
        Play(audioClip, type, pitch);
    }

    public void SetVolume(eSound type, float value)
    {
        _audioSources[(int)type].volume = value;
    }

    public AudioClip GetOrAddAudioClip(string path, eSound type = eSound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";
        AudioClip audioClip = null;

        if (type == eSound.Bgm) // BGM 배경음악 클립 붙이기
        {
            audioClip = Global.Resource.Load<AudioClip>(path);
        }
        else // Effect 효과음 클립 붙이기
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Global.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}