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
    protected Text readyText;

    public virtual void Awake()
    {
        userName = transform.Find("UserInfo/NameBox/Name").GetComponent<Text>();
        readyText = transform.Find("Ready").GetComponent<Text>();
    }

    public virtual void Start()
    {
        readyText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        isDestory = true;
    }

    public override void Execute(int eventCode, object message)
    {
        if (isDestory) return;
    }

    // 渲染隐藏
    public void RenderHide()
    {
        userDto = null;
        gameObject.SetActive(false);
        readyText.gameObject.SetActive(false);
    }

    // 渲染显示
    public void RenderShow(UserDto userDto)
    {
        this.userDto = userDto;
        userName.text = userDto.Name;
        beanCount.text = userDto.BeanCount;
        readyText.gameObject.SetActive(Models.GameModel.MatchRoomDto.readyList.Exists(item => item == userDto.Id));
        gameObject.SetActive(true);
    }
}