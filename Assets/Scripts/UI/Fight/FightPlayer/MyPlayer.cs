using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayer : BasePlayer
{
    public override void Awake()
    {
        userName = transform.Find("UserInfo/Name").GetComponent<Text>();
        Bind(UIEvent.User_Leave_Room);
    }
    public override void Start()
    {
        userDto = Models.GameModel.UserDto;
        userName.text = userDto.Name;
    }
    // ×Ô¼ºÀë¿ª
    public override void OtherLeave() => Dispatch(AreaCode.UI, UIEvent.MyPlayer_Leave_Room, null);
}
