using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAudio : AudioBase
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Bind(AudioEvent.Play_Effect_Audio);
        DontDestroyOnLoad(this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.Play_Effect_Audio:
                PlayEffect(message.ToString());
                break;

            default:
                break;
        }
    }

    // 播放音效
    private void PlayEffect(string index)
    {
        var roleGender = "woman";
        AudioClip ac = Resources.Load<AudioClip>($"Sound/{roleGender}/Chat_{index}"); // 音效文件
        audioSource.clip = ac;
        audioSource.Play();
    }
}