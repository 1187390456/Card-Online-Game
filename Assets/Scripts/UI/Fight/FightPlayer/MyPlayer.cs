using Protocol.Constant;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayer : BasePlayer
{
    public override void Awake()
    {
        base.Awake();
        beanCount = transform.Find("BeanBox/Count").GetComponent<Text>();
        Bind(UIEvent.My_User_Render, UIEvent.Send_Quick_Chat, UIEvent.Send_ZiDingYi_Chat, UIEvent.Send_Emoji_Chat);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.My_User_Render:
                RenderShow((UserDto)message);
                break;

            default:
                break;
        }
    }
}