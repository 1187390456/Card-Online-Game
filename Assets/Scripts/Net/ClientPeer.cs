using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// 客户端Socket的封装
/// </summary>
public class ClientPeer
{
    private Socket socket;

    /// <summary>
    ///  构造连接对象
    /// </summary>
    /// <param name="ip">ip地址</param>
    /// <param name="port">端口号</param>
    public ClientPeer(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip, port);
            Debug.Log("连接服务器成功 !");

            StartReceive();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    #region 接收数据

    private byte[] receiveBuffer = new byte[1024]; // 接收数据缓冲区
    private List<byte> dataCache = new List<byte>(); // 数据缓存
    private bool isProcessReceive = false; // 处理节流阀
    public Queue<SocketMsg> socketMsgQueue = new Queue<SocketMsg>(); // 消息列表

    /// <summary>
    /// 开始异步接收数据
    /// </summary>
    public void StartReceive()
    {
        if (socket == null && socket.Connected)
        {
            Debug.LogError("未连接成功 !");
            return;
        }
        socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallBack, socket);
    }

    /// <summary>
    /// 接受数据的回调
    /// </summary>
    /// <param name="iar"></param>
    private void ReceiveCallBack(IAsyncResult iar)
    {
        try
        {
            var len = socket.EndReceive(iar); // 返回消息长度
            byte[] tmpByteArray = new byte[len];
            Buffer.BlockCopy(receiveBuffer, 0, tmpByteArray, 0, len); // 拷贝数据缓冲区的数据

            // 缓存数据
            dataCache.AddRange(tmpByteArray);
            if (!isProcessReceive) ProcessReceive();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    /// <summary>
    /// 处理收到的数据
    /// </summary>
    private void ProcessReceive()
    {
        isProcessReceive = true;

        byte[] data = EncodeTool.DecodePacket(ref dataCache); // 将缓存数据包进行解包

        if (data == null)
        {
            isProcessReceive = false;
            return;
        }

        SocketMsg msg = EncodeTool.DecodeMsg(data); // 将解包的数据构造成消息类

        // 存储消息 等待处理
        socketMsgQueue.Enqueue(msg);

        ProcessReceive();
    }

    #endregion 接收数据

    #region 发送数据

    public void Send(int opCode, int subCode, object value)
    {
        SocketMsg msg = new SocketMsg(opCode, subCode, value); // 构造消息类
        byte[] data = EncodeTool.EncodeMsg(msg); // 将消息类转成字节数组
        byte[] packet = EncodeTool.EncodePacket(data); // 将字节数据按指定包构造
        try
        {
            socket.Send(packet);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    #endregion 发送数据
}