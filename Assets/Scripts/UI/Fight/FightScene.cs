using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : UIBase
{
    private void Start()
    {
        Dispatch(AreaCode.AUDIO, AudioEvent.Play_Music_Audio, BackGroundMuscicType.Normal.ToString()); // 播放背景音乐
    }
}