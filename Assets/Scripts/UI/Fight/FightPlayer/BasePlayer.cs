using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePlayer : UIBase
{
    private bool isDestory = false;
    protected UserDto userDto;
    protected Text userName;
    protected Text beanCount;
    public virtual void Awake()
    {
        userName = transform.Find("UserInfo/NameBox/Name").GetComponent<Text>();
        beanCount = transform.Find("UserInfo/BeanBox/Count").GetComponent<Text>();
        Bind(UIEvent.Other_User_Enter_Room, UIEvent.User_Leave_Room);
    }
    public virtual void Start()
    {

    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        isDestory = true;
    }
    public override void Execute(int eventCode, object message)
    {
        if (isDestory) return;
        switch (eventCode)
        {
            case UIEvent.Other_User_Enter_Room:
                if (userDto != null && userDto.Id == (int)message) OtherEnter();
                break;
            case UIEvent.User_Leave_Room:
                if (userDto != null && userDto.Id == (int)message) OtherLeave();
                break;
            default:
                break;
        }
    }

    // 重新渲染用户信息
    protected void RenderUserInfo()
    {
        userName.text = userDto.Name;
        beanCount.text = userDto.BeanCount;
        gameObject.SetActive(true);
    }
    // 其他人进入
    private void OtherEnter()
    {
        gameObject.SetActive(true);
        DispatchTools.Prompt_Msg(Dispatch, $"玩家{userDto.Name}进入房间！", Color.green);
    }

    // 其他人离开
    public virtual void OtherLeave()
    {
        gameObject.SetActive(false);
        DispatchTools.Prompt_Msg(Dispatch, $"玩家{userDto.Name}离开房间！", Color.green);
        userDto = null;
    }
}
