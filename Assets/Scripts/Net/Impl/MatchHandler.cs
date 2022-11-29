using Protocol.Code.SubCode;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MatchHandler : HandlerBase
{
    PromptMsg promptMsg = new PromptMsg();
    private bool isMatchSuccess; // 是否匹配成功

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case MatchCode.Enter_Sres:
                SelfEnter((MatchRoomDto)value);

                break;
            case MatchCode.Enter_Bro:
                OtherEnter((UserDto)value);
                break;

            case MatchCode.Leave_Bro:
                Leave((UserDto)value);
                break;

            case MatchCode.Ready_Bro: // 准备 广播所有
                OtherReady((UserDto)value);
                break;

            case MatchCode.CancleReady_Bro: // 取消准备 广播所有
                OtherCancleReady((UserDto)value);
                break;

            case MatchCode.Start_Bro: // 开始游戏广播所有
                break;
        }
    }
    // 自己进入 
    private void SelfEnter(MatchRoomDto matchRoomDto)
    {
        isMatchSuccess = false;
        Models.GameModel.MatchRoomDto = matchRoomDto; // 存储当前房间数据 
        RerenderUser();
        // 人数大于1 则匹配成功
        if (matchRoomDto.uidUserDic.Count > 1 && !isMatchSuccess) MatchSuccess();
        Dispatch(AreaCode.UI, UIEvent.Check_User_Ready, null);
    }

    // 其他人进入
    private void OtherEnter(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Add(userDto);  //更新本地房间数据 添加该角色
        RerenderUser(); //先渲染数据
        Dispatch(AreaCode.UI, UIEvent.Other_User_Enter_Room, userDto.Id);
        if (!isMatchSuccess) MatchSuccess();
        Dispatch(AreaCode.UI, UIEvent.Check_User_Ready, null);
    }
    //  有人离开
    private void Leave(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Remove(userDto.Id);  //更新本地房间数据 移除该角色
        Dispatch(AreaCode.UI, UIEvent.User_Leave_Room, userDto.Id); // 离开先更新UI
        RerenderUser();
        if (Models.GameModel.MatchRoomDto.IsReady(userDto.Id)) Models.GameModel.MatchRoomDto.readyList.Remove(userDto.Id);
        Dispatch(AreaCode.UI, UIEvent.Check_User_Ready, null);
    }
    // 其他人准备
    private void OtherReady(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Ready(userDto.Id); // 更新本地
        Dispatch(AreaCode.UI, UIEvent.Check_User_Ready, null);
    }
    // 其他人取消准备
    private void OtherCancleReady(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.CancleReady(userDto.Id); // 更新本地
        Dispatch(AreaCode.UI, UIEvent.Check_User_Ready, null);
    }

    private void RerenderUser() // 重新渲染用户 
    {
        var matchDto = Models.GameModel.MatchRoomDto;
        matchDto.RefreshOrderList(Models.GameModel.UserDto.Id);
        if (matchDto.leftUserDto != null)
        {
            Dispatch(AreaCode.UI, UIEvent.Left_User_Show, matchDto.leftUserDto); // 显示左侧玩家
        }
        else
        {
            Dispatch(AreaCode.UI, UIEvent.Left_User_Hide, matchDto.leftUserDto);  // 隐藏左侧玩家
        }
        if (matchDto.rightUserDto != null)
        {
            Dispatch(AreaCode.UI, UIEvent.Right_User_Show, matchDto.rightUserDto);
        }
        else
        {
            Dispatch(AreaCode.UI, UIEvent.Right_User_Hide, matchDto.leftUserDto);
        }
    }
    private void MatchSuccess() // 匹配成功
    {
        isMatchSuccess = true;
        Dispatch(AreaCode.UI, UIEvent.Match_Success, null);
    }

}