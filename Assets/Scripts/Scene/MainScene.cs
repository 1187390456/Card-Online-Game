using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : UIBase
{
    private void Start()
    {
        //TODO 测试用
        DispatchTools.Load_Scence(Dispatch, 2);
        Dispatch(AreaCode.AUDIO, AudioEvent.Play_Music_Audio, BackGroundMuscicType.Welcome.ToString()); // 播放背景音乐
    }
}