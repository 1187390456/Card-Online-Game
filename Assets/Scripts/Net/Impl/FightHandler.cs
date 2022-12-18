using Protocol.Code;
using Protocol.Constant;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

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
                else
                {
                    var res = GetSoundRes((DealDto)value);
                    Dispatch(AreaCode.AUDIO, AudioEvent.Play_Effect, res);
                    Dispatch(AreaCode.UI, UIEvent.Deal_Card_Sucess, (DealDto)value);
                }
                break;
            case FightCode.Pass_Bro:
                if (value == null) DispatchTools.Prompt_Msg(Dispatch, "当前是你的回合不可以不出!", Color.red);
                else
                {
                    var ran = new Random().Next(1, 4);
                    var res = "zengxiaoxian/" + "buyao" + ran.ToString();
                    Dispatch(AreaCode.AUDIO, AudioEvent.Play_Effect, res);
                    Dispatch(AreaCode.UI, UIEvent.Dont_Deal_Sucess, (int)value);
                }
                break;

            case FightCode.Pass_Round_Bro:
                PassRound((int)value);
                break;

            case FightCode.Over_Bro:
                OverDto overDto = (OverDto)value;
                DispatchTools.Prompt_Msg(Dispatch, $"获胜者id是{overDto.WinLists[0]}!", Color.red);
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
    private void GrabLandowner(GrabDto grabDto)
    {
        var res = "zengxiaoxian/" + "Rob1";
        Dispatch(AreaCode.AUDIO, AudioEvent.Play_Effect, res);
        Dispatch(AreaCode.UI, UIEvent.GrabLandowner_Success, grabDto);
    }
    // 转换抢地主
    private void TurnGrab(TurnDto turnDto)
    {
        if (!turnDto.isFirst)
        {
            var res = "zengxiaoxian/" + "NoRob";
            Dispatch(AreaCode.AUDIO, AudioEvent.Play_Effect, res);
        }
        Dispatch(AreaCode.UI, UIEvent.Turn_GrabLandowner, turnDto);
    }

    // 轮换出牌
    private void TurnDeal(int uid) => Dispatch(AreaCode.UI, UIEvent.Turn_Deal, uid);

    // 轮空 不出两人
    private void PassRound(int uid) => Dispatch(AreaCode.UI, UIEvent.Pass_Round, uid);

    // 获取声音资源路径
    private string GetSoundRes(DealDto dealDto)
    {
        var audioName = "zengxiaoxian/";
        switch (dealDto.Type)
        {
            case CardType.Single:
                audioName += dealDto.Weight;
                break;
            case CardType.Double:
                audioName += "dui" + dealDto.Weight / 2;
                break;
            case CardType.Straight:
                audioName += "shunzi";
                break;
            case CardType.Double_Straight:
                audioName += "liandui";
                break;
            case CardType.Triple_Straight:
                audioName += "feiji";
                break;
            case CardType.Three:
                audioName += "tuple" + dealDto.Weight / 3;
                break;
            case CardType.Three_One:
                audioName += "sandaiyi";
                break;
            case CardType.Three_Two:
                audioName += "sandaiyidui";
                break;
            case CardType.Boom:
                audioName += "zhadan";
                break;
            case CardType.Joker_Boom:
                audioName += "wangzha";
                break;
            default:
                break;
        }
        return audioName;
    }

}