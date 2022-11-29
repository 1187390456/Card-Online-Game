using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPlayer : BasePlayer
{
    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
        Bind(UIEvent.Left_User_Show, UIEvent.MyPlayer_Leave_Room, UIEvent.Left_User_Hide);
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.Left_User_Show:
                userDto = (UserDto)message;
                RenderUserInfo();
                break;
            case UIEvent.Left_User_Hide:
                userDto = null;
                gameObject.SetActive(false);
                break;
            case UIEvent.MyPlayer_Leave_Room:
                userDto = null;
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
