using Protocol.Code.SubCode;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MatchHandler : HandlerBase
{
    PromptMsg promptMsg = new PromptMsg();

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case MatchCode.Enter_Sres: // 自己进入
                SelfEnter((MatchRoomDto)value);

                break;
            case MatchCode.Enter_Bro: // 其他人进入
                OtherEnter((UserDto)value);
                break;

            case MatchCode.Leave_Bro: // 有人离开
                Leave((UserDto)value);
                break;

            case MatchCode.Ready_Bro: // 准备 广播所有
                break;

            case MatchCode.Start_Bro: // 开始游戏广播所有
                break;
        }
    }
    // 自己进入 
    private void SelfEnter(MatchRoomDto matchRoomDto)
    {
        Models.GameModel.MatchRoomDto = matchRoomDto; // 存储当前房间数据 自己的已经在里面了

        foreach (var id in matchRoomDto.uidUserDic.Keys)
        {
            if (id == Models.GameModel.UserDto.Id) continue; // 排除自己
            Dispatch(AreaCode.UI, UIEvent.User_Enter_Room, id);  // 更新存在人的UI
        }
    }

    // 其他人进入
    private void OtherEnter(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Add(userDto);  //更新本地房间数据 添加该角色
        Dispatch(AreaCode.UI, UIEvent.User_Enter_Room, userDto.Id); // 更新UI
        DispatchTools.Prompt_Msg(Dispatch, $"玩家{userDto.Name}加入了！", Color.green); // 提示进入
    }
    //  有人离开
    private void Leave(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Remove(userDto.Id);  //更新本地房间数据 移除该角色
        // 其他人离开
        if (userDto.Id != Models.GameModel.UserDto.Id)
        {
            Dispatch(AreaCode.UI, UIEvent.User_Leave_Room, userDto.Id); // 更新UI
            DispatchTools.Prompt_Msg(Dispatch, $"玩家{userDto.Name}离开了！", Color.green); // 提示离开
        }
    }

}