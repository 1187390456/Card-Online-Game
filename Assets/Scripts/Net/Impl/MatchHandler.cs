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
                MatchRoomDto matchRoomDto = (MatchRoomDto)value;
                SelfEnter(matchRoomDto);

                break;
            case MatchCode.Enter_Bro: // 其他人进入
                UserDto userDto = (UserDto)value;
                OtherEnter(userDto);
                break;

            case MatchCode.Leave_Bro: // 离开房间 有人离开就会触发 广播所有
                Leave((int)value);
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
        Debug.Log($"玩家{userDto.Name}加入了！");
        Models.GameModel.MatchRoomDto.Add(userDto);  //更新本地房间数据 添加该角色
        promptMsg.Change($"玩家{userDto.Name}加入了！", Color.green);
        Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
    }

    private void Leave(int userId)
    {
        Debug.Log("离开的玩家id是" + userId);

    }
}