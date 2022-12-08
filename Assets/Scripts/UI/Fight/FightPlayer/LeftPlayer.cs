using Protocol.Constant;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPlayer : BasePlayer
{
    public override void Awake()
    {
        base.Awake();
        beanCount = transform.Find("UserInfo/BeanBox/Count").GetComponent<Text>();
        cardAmout = transform.Find("CardStack/Amount").GetComponent<Text>();
        Bind(UIEvent.Left_User_Render, UIEvent.Left_User_Leave, UIEvent.Send_Quick_Chat, UIEvent.Send_ZiDingYi_Chat, UIEvent.Send_Emoji_Chat, UIEvent.Dispatch_Card);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.Left_User_Render:
                RenderShow((UserDto)message);
                break;

            case UIEvent.Left_User_Leave:
                RenderHide();
                break;

            case UIEvent.Dispatch_Card:
                StartCountAnimation();
                break;

            default:
                break;
        }
    }
}