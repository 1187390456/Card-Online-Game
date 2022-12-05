using Crosstales.RTVoice.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTVoice : AudioBase
{
    private SpeechText speechText;

    private void Awake()
    {
        speechText = transform.Find("SpeechText").GetComponent<SpeechText>();
        Bind(AudioEvent.Start_Speak_Text, AudioEvent.Stop_Speak_Text);
        DontDestroyOnLoad(this);

        InitSpeech();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.Start_Speak_Text:
                StartSpeak((string)message);
                break;

            case AudioEvent.Stop_Speak_Text:
                StopSpeak((string)message);
                break;

            default:
                break;
        }
    }

    //  初始化 防止之后卡顿 开始第一次掉会卡

    private void InitSpeech()
    {
        speechText.Volume = 0;
        speechText.Text = "0";
        speechText.Speak();
    }

    // 开始朗读
    private void StartSpeak(string text)
    {
        speechText.Volume = 1;
        speechText.Text = text;
        speechText.Speak();
    }

    // 停止朗读
    private void StopSpeak(string text) => speechText.Silence();
}