using Protocol.Constant;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightPlayer : BasePlayer
{
    public override void Awake()
    {
        base.Awake();
        beanCount = transform.Find("UserInfo/BeanBox/Count").GetComponent<Text>();
        Bind(UIEvent.Right_User_Render, UIEvent.Right_User_Leave, UIEvent.Send_Quick_Chat, UIEvent.Send_ZiDingYi_Chat, UIEvent.Send_Emoji_Chat);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.Right_User_Render:
                RenderShow((UserDto)message);
                break;

            case UIEvent.Right_User_Leave:
                RenderHide();
                break;

            default:
                break;
        }
    }
}