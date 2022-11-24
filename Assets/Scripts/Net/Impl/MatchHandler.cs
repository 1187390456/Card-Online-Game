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
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case MatchCode.Enter_Bro: // 其他人进入
                break;

            case MatchCode.Enter_Sres: // 自己进入
                MatchRoomDto matchRoomDto = (MatchRoomDto)value;
                SelfEnter(matchRoomDto);
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

    private void SelfEnter(MatchRoomDto matchRoomDto)
    {
        Debug.Log("进入匹配房间成功");
    }

    private void Leave(int userId)
    {
        Debug.Log("离开的玩家id是" + userId);
    }
}