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
                GrabLandowner((GrabDto)value);
                break;
            case FightCode.Turn_Grad_Bro:
                TurnGrab((TurnDto)value);
                break;
            case FightCode.Turn_Deal_Bro:
                TurnDeal((int)value);
                break;
            case FightCode.Deal_Bro:
                if (value == null) DispatchTools.Prompt_Msg(Dispatch, "您出的牌不符合规则!", Color.red);
                else Dispatch(AreaCode.UI, UIEvent.Deal_Card_Sucess, (DealDto)value);
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
    // 抢地主成功
    private void GrabLandowner(GrabDto grabDto) => Dispatch(AreaCode.UI, UIEvent.GrabLandowner_Success, grabDto);
    // 转换抢地主
    private void TurnGrab(TurnDto turnDto) => Dispatch(AreaCode.UI, UIEvent.Turn_GrabLandowner, turnDto);

    // 轮换出牌
    private void TurnDeal(int uid) => Dispatch(AreaCode.UI, UIEvent.Turn_Deal, uid);

}