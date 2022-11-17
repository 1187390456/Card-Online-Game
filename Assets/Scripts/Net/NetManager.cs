using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ConnectFlag
{
    public static bool isConnected = false;
}

/// <summary>
/// 网络模块
/// </summary>
public class NetManager : ManagerBase
{
    public static NetManager Instance = null;
    private ClientPeer client = new ClientPeer("127.0.0.1", 6666);

    private void Start()
    {
        if (ConnectFlag.isConnected) return;
        client.Connect();
        ConnectFlag.isConnected = true;
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

    /// <summary>
    /// 处理网络消息
    /// </summary>
    /// <param name="msg"></param>
    private void ProcessSocketMsg(SocketMsg msg)
    {
        switch (msg.opCode)
        {
            case OpCode.ACCOUNT:
                accountHandler.OnReceive(msg.subCode, msg.value);
                break;

            default:
                break;
        }
    }

    #endregion 处理接收到服务器发来的消息
}