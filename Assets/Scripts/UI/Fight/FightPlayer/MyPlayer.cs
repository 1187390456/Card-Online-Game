using DG.Tweening;
using Protocol.Constant;
using Protocol.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyPlayer : BasePlayer
{
    private Transform CardStack; // 卡牌盒子

    private int index = 0; // 卡片动画索引
    private float cardSpace = 55.0f; // 卡牌间距

    private List<CardDto> cardList = new List<CardDto>(); // 玩家手牌

    private GameObject CardRes; // 卡牌资源
    private Sprite[] JokerSprite; // 大小王精灵

    private List<GameObject> rayList = new List<GameObject>(); // 触摸的游戏对象列表

    public override void Awake()
    {
        base.Awake();
        beanCount = transform.Find("BeanBox/Count").GetComponent<Text>();
        Bind(UIEvent.My_User_Render, UIEvent.Send_Quick_Chat, UIEvent.Send_ZiDingYi_Chat, UIEvent.Send_Emoji_Chat, UIEvent.Dispatch_Card);

        CardStack = transform.Find("CardStack");
        CardRes = Resources.Load<GameObject>("Perfabs/Card");
        JokerSprite = Resources.LoadAll<Sprite>("Image/Card");
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
                Dispatch(AreaCode.AUDIO, AudioEvent.Play_SpecialEffect_Audio, "Dispatch");  // 播放发牌音效
                CrateCardAnimation();
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        RayCast();
    }

    private void RayCast()
    {
        if (Input.GetMouseButton(0))
        {
            var gobj = GetFirstPickGameObject(Input.mousePosition);
            if (gobj != null && gobj.CompareTag("Card") && !rayList.Exists(item => gobj == item))
            {
                rayList.Add(gobj);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            foreach (var item in rayList)
            {
                var cardScript = item.GetComponent<Card>();
                cardScript.Move();
            }

            rayList.Clear();
        }
    }

    /// <summary>
    /// 点击屏幕坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject GetFirstPickGameObject(Vector2 position)
    {
        EventSystem eventSystem = EventSystem.current;
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = position;
        //射线检测ui
        List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
        if (uiRaycastResultCache.Count > 0)
            return uiRaycastResultCache[0].gameObject;
        return null;
    }

    #region 卡牌区域

    // 创建卡牌动画
    private void CrateCardAnimation()
    {
        if (CardStack.childCount == 17)
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.Stop_SpecialEffect_Audio, null); // 停止发牌音效
            return;
        }

        var card = Instantiate(CardRes, CardStack);
        card.name = $"card{index}";
        var rt = card.GetComponent<RectTransform>();

        SetCard(rt);

        StartAnimation(rt);
    }

    // 设置卡牌信息
    private void SetCard(RectTransform rt)
    {
        var script = rt.GetComponent<Card>();
        var weight = cardList[index].Weight;
        var color = cardList[index].Color;

        if (color != CardColor.None) // 不是大小王
        {
            var resColor = (color == CardColor.Heart || color == CardColor.Square) ? "Red" : "Black"; // 区分文件颜色

            var bigWeightSprite = Resources.Load<Sprite>($"Image/Card/Weight/{resColor}/Big/{weight}"); // 权重精灵
            var hugeColorSprite = Resources.Load<Sprite>($"Image/Card/Color/Huge/{color}"); // 权重颜色 巨大
            var bigColorSprite = Resources.Load<Sprite>($"Image/Card/Color/Big/{color}"); // 权重颜色 大

            script.SetCard(bigWeightSprite, hugeColorSprite, bigColorSprite); // 设置卡牌
        }
        else if (weight == CardWeight.SJoker) script.SetCard(JokerSprite[0], JokerSprite[3]);
        else if (weight == CardWeight.LJoker) script.SetCard(JokerSprite[1], JokerSprite[2], null, true);
    }

    // 开始卡牌动画
    private void StartAnimation(RectTransform rt)
    {
        float preXPos;

        if (index == 0)
        {
            preXPos = 0;
            cardSpace = 0;
        }
        else
        {
            preXPos = CardStack.Find($"card{index - 1}").GetComponent<RectTransform>().anchoredPosition.x;
            cardSpace = 55.0f;
        }

        var aurPos = rt.anchoredPosition;
        var startPos = new Vector2(preXPos, aurPos.y);
        var endPos = new Vector2(cardSpace + preXPos, aurPos.y);

        var tween = DotweenTools.DoRectMove(rt, startPos, endPos, .2f, "CardMove");

        tween.onComplete = () =>
        {
            index++;
            CrateCardAnimation();
            FixParentPos();
        };
    }

    // 修复卡牌位置 出牌后修复
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

    // 修复盒子位置 父级盒子初始值为577.5
    private void FixParentPos()
    {
        var rt = CardStack.GetComponent<RectTransform>();
        var aurPos = rt.anchoredPosition;
        var endPos = new Vector2((20 - CardStack.childCount) * cardSpace / 2 + cardSpace, aurPos.y); // 多加个左边距为卡牌间距
        DotweenTools.DoRectMove(rt, rt.anchoredPosition, endPos, .2f, "CardMove");
    }

    #endregion 卡牌区域
}