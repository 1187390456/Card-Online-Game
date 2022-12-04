using Protocol.Code;
using Protocol.Dto;
using UnityEngine;

public class MatchHandler : HandlerBase
{
    private PromptMsg promptMsg = new PromptMsg();
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

            case MatchCode.Ready_Bro:
                Ready((UserDto)value);
                break;

            case MatchCode.CancleReady_Bro:
                CancleReady((UserDto)value);
                break;

            case MatchCode.Start_Bro:
                break;
        }
    }

    // 自己进入
    private void SelfEnter(MatchRoomDto matchRoomDto)
    {
        Models.GameModel.MatchRoomDto = matchRoomDto; // 更新本地
        if (matchRoomDto.uidUserDic.Count > 1 && !isMatchSuccess) MatchSuccess();         // 人数大于1 则匹配成功
        RerenderUser();
    }

    // 其他人进入
    private void OtherEnter(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Add(userDto);  // 更新本地
        if (!isMatchSuccess)
        {
            MatchSuccess();
        }
        else
        {
            DispatchTools.Prompt_Msg(Dispatch, $"玩家{userDto.Name}进入房间！", Color.green);
        }
        RerenderUser();
    }

    //  有人离开
    private void Leave(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Remove(userDto.Id);  //更新本地
        if (Models.GameModel.MatchRoomDto.IsReady(userDto.Id)) Models.GameModel.MatchRoomDto.readyList.Remove(userDto.Id);
        if (userDto.Id == Models.GameModel.UserDto.Id)
        {
            Dispatch(AreaCode.UI, UIEvent.Crear_All_History, null);
            isMatchSuccess = false;
            return;
        }
        else
        {
            DispatchTools.Prompt_Msg(Dispatch, $"玩家{userDto.Name}离开房间！", Color.green);
        }
        RerenderUser();
    }

    // 有人准备
    private void Ready(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.Ready(userDto.Id); // 更新本地
        RerenderUser();
    }

    // 有人取消准备
    private void CancleReady(UserDto userDto)
    {
        Models.GameModel.MatchRoomDto.CancleReady(userDto.Id);
        RerenderUser();
    }

    private void RerenderUser() // 重新渲染用户
    {
        var matchDto = Models.GameModel.MatchRoomDto;
        matchDto.RefreshOrderList(Models.GameModel.UserDto.Id);

        if (matchDto.leftUserDto != null)
        {
            Dispatch(AreaCode.UI, UIEvent.Left_User_Render, matchDto.leftUserDto);
        }
        else
        {
            Dispatch(AreaCode.UI, UIEvent.Left_User_Leave, null);
        }

        if (matchDto.rightUserDto != null)
        {
            Dispatch(AreaCode.UI, UIEvent.Right_User_Render, matchDto.rightUserDto);
        }
        else
        {
            Dispatch(AreaCode.UI, UIEvent.Right_User_Leave, null);
        }

        Dispatch(AreaCode.UI, UIEvent.My_User_Render, Models.GameModel.UserDto); // 自己渲染 自己一定在
    }

    private void MatchSuccess() // 匹配成功
    {
        isMatchSuccess = true;
        Dispatch(AreaCode.UI, UIEvent.Match_Success, null);
    }
}