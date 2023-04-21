using Protocol.Code;
using Protocol.Dto;
using System;
using UnityEngine;

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

    /// <summary>
    /// 玩家发送快捷消息
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="index"></param>
    public static void Chat_Send_Quick_Cres(Action<int, int, object> Dispatch, int index)
    {
        socketMsg.Change(OpCode.Chat, ChatCode.Send_Quick_Cres, index);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 玩家发送表情
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="name"></param>
    public static void Chat_Send_Emoji_Cres(Action<int, int, object> Dispatch, string name)
    {
        socketMsg.Change(OpCode.Chat, ChatCode.Send_Emoji_Cres, name);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 玩家发送自定义消息
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="text"></param>
    public static void Chat_Send_ZiDingYi_Cres(Action<int, int, object> Dispatch, string text)
    {
        socketMsg.Change(OpCode.Chat, ChatCode.Send_ZiDingYi_Cres, text);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 离开战斗房间
    /// </summary>
    /// <param name="Dispatch"></param>
    public static void Fight_Leave_Cres(Action<int, int, object> Dispatch)
    {
        socketMsg.Change(OpCode.Fight, FightCode.Leave_Cres, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 玩家抢地主
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="text"></param>
    public static void Fight_Grab_Landowner_Cres(Action<int, int, object> Dispatch, bool isGrabe)
    {
        socketMsg.Change(OpCode.Fight, FightCode.Grab_Landowner_Cres, isGrabe);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 玩家出牌
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="dealDto"></param>
    public static void Fight_Deal_Cres(Action<int, int, object> Dispatch, DealDto dealDto)
    {
        socketMsg.Change(OpCode.Fight, FightCode.Deal_Cres, dealDto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 玩家不出
    /// </summary>
    /// <param name="Dispatch"></param>
    /// <param name="dealDto"></param>
    public static void Fight_Pass_Cres(Action<int, int, object> Dispatch)
    {
        socketMsg.Change(OpCode.Fight, FightCode.Pass_Cres, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

}