using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAudio : AudioBase
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Bind(AudioEvent.Play_ChatEffect_Audio,
            AudioEvent.Play_SpecialEffect_Audio,
            AudioEvent.Stop_SpecialEffect_Audio,
            AudioEvent.Play_Effect
            );
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.Play_ChatEffect_Audio:
                PlayChatEffect(message.ToString());
                break;

            case AudioEvent.Play_SpecialEffect_Audio:
                PlaySpecialEffect(message.ToString());
                break;

            case AudioEvent.Stop_SpecialEffect_Audio:
                audioSource.Stop();
                break;

            case AudioEvent.Play_Effect:
                PlayEffect(message.ToString());
                break;

            default:
                break;
        }
    }

    // 播放角色聊天音效
    private void PlayChatEffect(string index)
    {
        var roleGender = "woman";
        AudioClip ac = Resources.Load<AudioClip>($"Sound/{roleGender}/Chat_{index}"); // 音效文件
        audioSource.clip = ac;
        audioSource.Play();
    }

    // 播放特别音效
    private void PlaySpecialEffect(string name)
    {
        AudioClip ac = Resources.Load<AudioClip>($"Sound/Special/Special_{name}"); // 音效文件
        audioSource.clip = ac;
        audioSource.Play();
    }
    // 播放指定路径音效
    private void PlayEffect(string res)
    {
        AudioClip ac = Resources.Load<AudioClip>($"Sound/{res}"); // 音效文件
        audioSource.clip = ac;
        audioSource.Play();
    }
}