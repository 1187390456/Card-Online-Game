using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 游戏数据的存储类
/// </summary>
public class GameModel
{
    public UserDto UserDto { get; set; } // 角色数据

    public MatchRoomDto MatchRoomDto { get; set; } // 匹配房价数据
}