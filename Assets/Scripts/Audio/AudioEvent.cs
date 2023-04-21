using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音事件码
/// </summary>
public class AudioEvent
{
    // 聊天音效

    public const int Play_ChatEffect_Audio = 0;
    public const int Stop_ChatEffect_Audio = 1;

    // 音乐

    public const int Play_Music_Audio = 2;
    public const int Stop_Music_Audio = 3;

    // 朗读

    public const int Start_Speak_Text = 4;
    public const int Stop_Speak_Text = 5;

    // 特别音效

    public const int Play_SpecialEffect_Audio = 6;
    public const int Stop_SpecialEffect_Audio = 7;

    // 播放指定音效
    public const int Play_Effect = 8;
}