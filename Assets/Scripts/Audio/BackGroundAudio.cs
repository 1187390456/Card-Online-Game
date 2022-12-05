using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackGroundMuscicType
{
    Exciting,
    Lose,
    Normal,
    Normal2,
    Welcome,
    Win
}

public class BackGroundAudio : AudioBase
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Bind(AudioEvent.Play_Music_Audio);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.Play_Music_Audio:
                SetBackGroundMuscic((string)message);
                break;

            default:
                break;
        }
    }

    private void SetBackGroundMuscic(string type)
    {
        AudioClip ac = Resources.Load<AudioClip>($"Sound/MusicEx/MusicEx_{type}");
        audioSource.clip = ac;
        audioSource.Play();
    }
}