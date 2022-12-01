using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleBox : UIBase
{
    private GameObject LeftRole;
    private GameObject RightRole;

    private void Awake()
    {
        LeftRole = transform.Find("LeftRole").gameObject;
        RightRole = transform.Find("RightRole").gameObject;
        Bind(UIEvent.Left_User_Render, UIEvent.Left_User_Leave, UIEvent.Right_User_Render, UIEvent.Right_User_Leave);
    }

    private void Start()
    {
        LeftRole.SetActive(false);
        RightRole.SetActive(false);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Left_User_Render:
                LeftRole.SetActive(true);
                break;

            case UIEvent.Left_User_Leave:
                LeftRole.SetActive(false);
                break;

            case UIEvent.Right_User_Render:
                RightRole.SetActive(true);
                break;

            case UIEvent.Right_User_Leave:
                RightRole.SetActive(false);
                break;

            default:
                break;
        }
    }
}