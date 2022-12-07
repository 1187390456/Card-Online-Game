using DG.Tweening;
using Protocol.Constant;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayer : BasePlayer
{
    private Transform CardStack; // 卡牌盒子
    private GameObject CardRes; // 卡牌资源

    private int index = 0; // 卡片动画索引
    private float cardSpace = 55.0f; // 卡牌间距

    private List<CardDto> cardList = new List<CardDto>(); // 玩家手牌

    public override void Awake()
    {
        base.Awake();
        beanCount = transform.Find("BeanBox/Count").GetComponent<Text>();
        Bind(UIEvent.My_User_Render, UIEvent.Send_Quick_Chat, UIEvent.Send_ZiDingYi_Chat, UIEvent.Send_Emoji_Chat, UIEvent.Dispatch_Card);

        CardStack = transform.Find("CardStack");
        CardRes = Resources.Load<GameObject>("Perfabs/Card");
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.My_User_Render:
                RenderShow((UserDto)message);
                break;

            case UIEvent.Dispatch_Card:
                cardList = (List<CardDto>)message;
                CrateCardAnimation();
                break;

            default:
                break;
        }
    }

    // 创建卡牌动画
    private void CrateCardAnimation()
    {
        if (CardStack.childCount == cardList.Count) return;

        float preXPos;

        var card = Instantiate(CardRes, CardStack);
        var rt = card.GetComponent<RectTransform>();

        if (index == 0)
        {
            preXPos = 0;
            cardSpace = 0;
        }
        else
        {
            preXPos = CardStack.GetChild(index).GetComponent<RectTransform>().anchoredPosition.x;
            cardSpace = 55.0f;
        }

        var aurPos = rt.anchoredPosition;
        var startPos = new Vector2(preXPos, aurPos.y);
        var endPos = new Vector2(cardSpace + preXPos, aurPos.y);

        var tween = DotweenTools.DoRectMove(rt, startPos, endPos, .2f, "CardMove");

        FixParentPos();

        tween.onComplete = () =>
        {
            index = rt.GetSiblingIndex();
            CrateCardAnimation();
        };
    }

    // 修复卡牌位置
    private void FixCardPos()
    {
        for (int i = 0; i < CardStack.childCount; i++)
        {
            var rt = CardStack.GetChild(i).GetComponent<RectTransform>();
            var aurPos = rt.anchoredPosition;
            var endPos = new Vector2(cardSpace * i, aurPos.y);
            DotweenTools.DoRectMove(rt, rt.anchoredPosition, endPos, .2f, "CardMove");
        }
        FixParentPos();
    }

    // 修复盒子位置
    private void FixParentPos()
    {
        var rt = CardStack.GetComponent<RectTransform>();
        var aurPos = rt.anchoredPosition;
        var endPos = new Vector2((20 - CardStack.childCount) * cardSpace / 2 + cardSpace, aurPos.y); // 多加个左边距为卡牌间距
        DotweenTools.DoRectMove(rt, rt.anchoredPosition, endPos, .2f, "CardMove");
    }
}