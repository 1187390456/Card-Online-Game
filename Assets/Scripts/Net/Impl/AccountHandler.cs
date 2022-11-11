using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AccountHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.Regist_Sres:
                break;

            case AccountCode.LOGIN:
                break;

            default:
                break;
        }
    }
}