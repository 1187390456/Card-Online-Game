using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 网络消息
/// </summary>
public class SocketMsg
{
    public SocketMsg()
    {
    }

    public SocketMsg(int opCode, int subCode, object value)
    {
        this.opCode = opCode;
        this.subCode = subCode;
        this.value = value;
    }

    public int opCode { get; set; }
    public int subCode { get; set; }
    public object value { get; set; }

    public void Change(int opCode, int subCode, object value)
    {
        this.opCode = opCode;
        this.subCode = subCode;
        this.value = value;
    }
}