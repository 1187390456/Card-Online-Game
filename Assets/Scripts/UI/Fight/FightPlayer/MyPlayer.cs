using DG.Tweening;
using Protocol.Constant;
using Protocol.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyPlayer : BasePlayer
{
    private int index = 0; // 卡片动画索引
    private Transform cardStack; // 卡牌盒子
    private float cardSpace = 55.0f; // 卡牌间距

    private GameObject CardRes; // 卡牌资源
    private Sprite[] JokerSprite; // 大小王精灵资源

    private List<CardDto> cardList = new List<CardDto>(); // 玩家手牌
    private List<GameObject> rayList = new List<GameObject>(); // 触摸的游戏对象列表


    public override void Awake()
    {
        base.Awake();
        beanCount = transform.Find("BeanBox/Count").GetComponent<Text>();

        cardStack = transform.Find("CardStack");
        CardRes = Resources.Load<GameObject>("Perfabs/Card");
        JokerSprite = Resources.LoadAll<Sprite>("Image/Card");

        Bind(UIEvent.My_User_Render,
            UIEvent.Dispatch_Card,
            UIEvent.Turn_GrabLandowner,
            UIEvent.Send_Quick_Chat,
            UIEvent.GrabLandowner_Success,
            UIEvent.Send_ZiDingYi_Chat,
            UIEvent.Send_Emoji_Chat,
            UIEvent.Turn_Deal,
            UIEvent.Deal_Card,
            UIEvent.Deal_Card_Sucess,
            UIEvent.Dont_Deal_Sucess,
            UIEvent.Pass_Round
            );
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
                Dispatch(AreaCode.UI, UIEvent.Set_MingPaiBtn_Active, true);  // 显示明牌
                CrateCardAnimation(); // 创建卡牌
                break;

            case UIEvent.Turn_GrabLandowner:
                TurnDto turnDto = (TurnDto)message;
                if (turnDto.isFirst)
                {
                    if (turnDto.currentId == userDto.Id) StartGrabLandowner = () => Dispatch(AreaCode.UI, UIEvent.Set_GrabLandownerBtn_Active, true);
                }
                else if (turnDto.currentId == userDto.Id) Show_DontGrabe();
                else if (turnDto.nextId == userDto.Id) Dispatch(AreaCode.UI, UIEvent.Set_GrabLandownerBtn_Active, true);
                break;

            case UIEvent.GrabLandowner_Success:
                GrabDto grabDto = (GrabDto)message;
                HideOperate();
                Dispatch(AreaCode.UI, UIEvent.Show_TableCard, grabDto.TableCardList); // 显示底牌
                if (grabDto.Uid == userDto.Id)
                {
                    StartCoroutine(DestoryAllCard());

                    cardList = grabDto.PlayerCardList;
                    index = 0;

                    for (var i = 0; i < cardList.Count; i++)
                    {
                        var card = Instantiate(CardRes, cardStack);
                        var rt = card.GetComponent<RectTransform>();
                        card.name = $"card{index}";
                        SetCard(rt);
                        SetPos(rt);
                    }

                    StartCoroutine(FixCardPos());
                }
                break;

            case UIEvent.Turn_Deal:
                if ((int)message == userDto.Id)
                {
                    // 移除自己的上一次出牌区域 显示出牌
                    RemoveDealArea();
                    Dispatch(AreaCode.UI, UIEvent.Set_TurnPanel_Active, true);
                }
                break;

            case UIEvent.Deal_Card:
                var cardDtoList = GetCardDtoList();
                DealDto dealDto = new DealDto(cardDtoList, userDto.Id);
                DispatchTools.Fight_Deal_Cres(Dispatch, dealDto);

                break;

            case UIEvent.Deal_Card_Sucess:
                var dealDtos = (DealDto)message;
                RemoveDealArea();
                HideOperate();
                if (dealDtos.Uid != userDto.Id) return;
                RemoveCard();
                Dispatch(AreaCode.UI, UIEvent.Hide_Self_Operate, null);
                break;

            case UIEvent.Dont_Deal_Sucess:
                var uid = (int)message;
                if (uid != userDto.Id) return;
                Dispatch(AreaCode.UI, UIEvent.Hide_Self_Operate, null);
                Show_DontDeal();
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        RayCast();
    }

    // 监听手指点击
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
                var cardScript = item.GetComponent<Card>() == null ? item.GetComponentInParent<Card>() : item.GetComponent<Card>();
                cardScript.Move();
            }

            rayList.Clear();
        }
    }

    // 获取选中手牌传输数据
    private List<CardDto> GetCardDtoList()
    {
        List<CardDto> cardDtos = new List<CardDto>();
        for (var i = 0; i < cardStack.childCount; i++)
        {
            var card = cardStack.GetChild(i).GetComponent<Card>();
            if (card.isActive) cardDtos.Add(card.cardDto);
        }
        return cardDtos;
    }

    // 移除指定手牌 
    private void RemoveCard()
    {
        List<Card> removeList = new List<Card>();
        for (var i = 0; i < cardStack.childCount; i++)
        {
            var card = cardStack.GetChild(i).GetComponent<Card>();
            if (card.isActive) removeList.Add(card);
        }

        for (var i = 0; i < removeList.Count; i++) Destroy(removeList[i].gameObject);

        StartCoroutine(FixCardPos());

        var cardDtos = GetCardDtoList();

        StartCoroutine(CreateDealArea(cardDtos));
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
        if (cardStack.childCount == 17)
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.Stop_SpecialEffect_Audio, null); // 停止发牌音效
            Dispatch(AreaCode.UI, UIEvent.Set_MingPaiBtn_Active, false);  // 隐藏明牌
            if (StartGrabLandowner != null) StartGrabLandowner(); // 开始抢地主
            return;
        }

        var card = Instantiate(CardRes, cardStack);
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

        script.SetCardDto(cardList[index]);

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

    // 设置卡片位置 抢地主后增加底牌用
    private void SetPos(RectTransform rt)
    {
        float preXPos;

        if (index == 0)
        {
            preXPos = 0;
            cardSpace = 0;
        }
        else
        {
            preXPos = cardStack.Find($"card{index - 1}").GetComponent<RectTransform>().anchoredPosition.x;
            cardSpace = 55.0f;
        }
        rt.anchoredPosition = new Vector2(cardSpace + preXPos, rt.anchoredPosition.y);
        index++;
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
            preXPos = cardStack.Find($"card{index - 1}").GetComponent<RectTransform>().anchoredPosition.x;
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
    private IEnumerator FixCardPos()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < cardStack.childCount; i++)
        {
            var rt = cardStack.GetChild(i).GetComponent<RectTransform>();
            var aurPos = rt.anchoredPosition;
            var endPos = new Vector2(cardSpace * i, aurPos.y);
            DotweenTools.DoRectMove(rt, rt.anchoredPosition, endPos, .2f, "CardMove");
        }
        FixParentPos();
    }
    // 修复父级盒子 初始动画修复
    private void FixParentPos()
    {
        var rt = cardStack.GetComponent<RectTransform>();
        var aurPos = rt.anchoredPosition;
        var endPos = new Vector2((20 - cardStack.childCount) * cardSpace / 2 + cardSpace, aurPos.y); // 多加个左边距为卡牌间距
        DotweenTools.DoRectMove(rt, rt.anchoredPosition, endPos, .2f, "CardMove");
    }

    // 清除所有卡牌
    private IEnumerator DestoryAllCard()
    {
        List<GameObject> gobjList = new List<GameObject>();
        for (var i = 0; i < cardStack.childCount; i++) gobjList.Add(cardStack.GetChild(i).gameObject);
        for (var i = 0; i < gobjList.Count; i++) Destroy(gobjList[i]);
        yield return new WaitForEndOfFrame();
    }
    #endregion 卡牌区域
}