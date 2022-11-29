using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using Protocol.Code;
using Protocol.Code.SubCode;
using Protocol.Dto;

/// <summary>
/// 消息分发工具
/// </summary>
public static class DispatchTools
{

    private static PromptMsg promptMsg = new PromptMsg();
    private static SocketMsg socketMsg = new SocketMsg();
    private static LoadSceneMsg loadSceneMsg = new LoadSceneMsg();

    /// <summary>
    /// 分发提示消息
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="text"></param>
    /// <param name="color"></param>
    public static void Prompt_Msg(Action<int, int, object> Dispatch, string text, Color color)
    {
        promptMsg.Change(text, color);
        Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="index"></param>
    /// <param name="callback"></param>
    public static void Load_Scence(Action<int, int, object> Dispatch, int index, Action callback = null)
    {
        loadSceneMsg.Change(index, callback);
        Dispatch(AreaCode.SCENCE, SceneEvent.Load_Scence, loadSceneMsg);
    }

    /// <summary>
    /// 账号登录
    /// </summary>
    /// <param name="Dispatch"></param>
    public static void Account_Login(Action<int, int, object> Dispatch, AccountDto accountDto)
    {
        socketMsg.Change(OpCode.Account, AccountCode.Login, accountDto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 账号注册
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="accountDto"></param>
    public static void Account_Regist_Cres(Action<int, int, object> Dispatch, AccountDto accountDto)
    {
        socketMsg.Change(OpCode.Account, AccountCode.Regist_Cres, accountDto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }


    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="Dispatch"></param>
    public static void User_Create_Cres(Action<int, int, object> Dispatch, string name)
    {
        socketMsg.Change(OpCode.User, UserCode.Create_Cres, name);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 获取角色信息
    /// </summary>
    /// <param name="Dispatch"></param>
    public static void User_Get_Cres(Action<int, int, object> Dispatch)
    {
        socketMsg.Change(OpCode.User, UserCode.Get_Cres, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 开始匹配 
    /// </summary>
    /// <param name="Dispatch"></param>
    public static void Match_Enter_Cres(Action<int, int, object> Dispatch)
    {
        socketMsg.Change(OpCode.Match, MatchCode.Enter_Cres, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 取消匹配 
    /// </summary>
    /// <param name="Dispatch"></param>
    public static void Match_Leave_Cres(Action<int, int, object> Dispatch)
    {
        socketMsg.Change(OpCode.Match, MatchCode.Leave_Cres, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    /// <summary>
    /// 玩家准备
    /// </summary>
    /// <param name="Dispatch"></param>
    public static void Match_Ready_Cres(Action<int, int, object> Dispatch)
    {
        socketMsg.Change(OpCode.Match, MatchCode.Ready_Cres, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    /// <summary>
    /// 玩家准备
    /// </summary>
    /// <param name="Dispatch"></param>
    public static void Match_CancleReady_Cres(Action<int, int, object> Dispatch)
    {
        socketMsg.Change(OpCode.Match, MatchCode.CancleReady_Cres, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

}