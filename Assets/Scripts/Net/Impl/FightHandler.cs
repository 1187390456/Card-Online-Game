using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FightHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case FightCode.Get_Card_Sres:
                GetCards((List<CardDto>)value);
                break;

            case FightCode.Grab_Landowner_Bro:
                GrabLandowner((int)value);
                break;

            default:
                break;
        }
    }

    // 获取手牌响应
    private void GetCards(List<CardDto> cardList)
    {
        Dispatch(AreaCode.UI, UIEvent.Dispatch_Card, cardList);  // 分发手牌
        Dispatch(AreaCode.UI, UIEvent.Set_ReadyBtn_Active, false); // 隐藏准备按钮
    }

    // 广播开始抢地主
    private void GrabLandowner(int startUid)
    {
    }
}