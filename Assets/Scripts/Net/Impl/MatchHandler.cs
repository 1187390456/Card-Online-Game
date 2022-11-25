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
        Models.GameModel.MatchRoomDto = matchRoomDto; // 存储当前房间数据
        Debug.Log("进入匹配房间成功");
    }

    // 其他人进入
    private void OtherEnter(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Add(userDto);  //更新本地房间数据 添加该角色
        promptMsg.Change($"玩家{userDto.Name}加入了！", Color.green);
        Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
        Debug.Log($"玩家{userDto.Name}加入了！");
    }
    //  有人离开
    private void Leave(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Remove(userDto.Id);  //更新本地房间数据 移除该角色
        // 其他人离开
        if (userDto.Id != Models.GameModel.UserDto.Id)
        {
            promptMsg.Change($"玩家{userDto.Name}离开了！", Color.green);
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            Debug.Log($"玩家{userDto.Name}离开了！");
        }
    }
}