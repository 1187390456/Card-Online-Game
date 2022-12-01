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
        Bind(UIEvent.Left_User_Render, UIEvent.Left_User_Leave);
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

            default:
                break;
        }
    }
}