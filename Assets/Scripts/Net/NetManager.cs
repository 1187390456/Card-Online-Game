using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// IP管理
/// </summary>
public static class IpAddress
{
    public static string WAN = "xuchenming.cn";
    public static string LocalAreaNet = "127.0.0.1";
}

/// <summary>
/// 网络模块
/// </summary>
public class NetManager : ManagerBase
{
    public static NetManager Instance = null;
    private ClientPeer client = new ClientPeer(IpAddress.LocalAreaNet, 6666);

    private void Start()
    {
        client.Connect();
    }

    private void Update()
    {
        if (client == null) return;
        while (client.socketMsgQueue.Count > 0)
        {
            SocketMsg msg = client.socketMsgQueue.Dequeue();
            ProcessSocketMsg(msg);
        }
    }

    #region 客户端内部给服务器发消息

    private void Awake()
    {
        Instance = this;
        Add(0, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMsg);
                break;

            default:
                break;
        }
    }

    #endregion 客户端内部给服务器发消息

    #region 处理接收到服务器发来的消息

    private HandlerBase accountHandler = new AccountHandler(); // 账号处理
    private HandlerBase userHandler = new UserHandler(); // 角色处理
    private HandlerBase matchHandler = new MatchHandler(); // 匹配处理
    private HandlerBase chatHandler = new ChatHandler(); // 聊天处理

    /// <summary>
    /// 处理网络消息
    /// </summary>
    /// <param name="msg"></param>
    private void ProcessSocketMsg(SocketMsg msg)
    {
        switch (msg.opCode)
        {
            case OpCode.Account:
                accountHandler.OnReceive(msg.subCode, msg.value);
                break;

            case OpCode.User:
                userHandler.OnReceive(msg.subCode, msg.value);
                break;

            case OpCode.Match:
                matchHandler.OnReceive(msg.subCode, msg.value);
                break;

            case OpCode.Chat:
                chatHandler.OnReceive(msg.subCode, msg.value);
                break;

            default:
                break;
        }
    }

    #endregion 处理接收到服务器发来的消息
}