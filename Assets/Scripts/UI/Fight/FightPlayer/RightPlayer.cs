using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPlayer : BasePlayer
{
    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
        Bind(UIEvent.Right_User_Show, UIEvent.MyPlayer_Leave_Room, UIEvent.Right_User_Hide);
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.Right_User_Show:
                userDto = (UserDto)message;
                RenderUserInfo();
                break;
            case UIEvent.Right_User_Hide:
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
