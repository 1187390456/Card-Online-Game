using Protocol.Constant;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections;

public class Operate
{
    public const int Ming_Pai = 1;
}

public class BasePlayer : UIBase
{
    public bool isDestory = false;

    protected UserDto userDto;
    protected Text userName;
    protected Text beanCount;
    protected Text readyText;

    protected Tween chatTween;
    protected GameObject chatBox;
    protected Text chatText;
    protected Image chatEmoji;
    protected SpriteAnimation spriteAnimation; // 控制表情帧动画

    // 左右玩家的

    protected Transform timer; // 闹钟
    protected Text cardAmout; // 手牌
    protected int count = 0; // 手牌数量

    // 操作

    protected GameObject operate;
    protected Transform dontCall;
    protected Transform dontDeal;
    protected Transform dontGrab;
    protected Transform mingPai;
    protected Transform callLandowner;
    protected Transform grabLandowner;

    protected int grabTurnCount = 0; // 抢地主轮换次数

    // 委托
    protected Action StartGrabLandowner; // 开始抢地主

    protected Transform dealArea; // 出牌区域
    protected GameObject cardDeal; //  出牌卡资源

    public virtual void Awake()
    {
        userName = transform.Find("UserInfo/NameBox/Name").GetComponent<Text>();
        readyText = transform.Find("Ready").GetComponent<Text>();

        chatBox = transform.Find("Chat").gameObject;
        chatText = chatBox.transform.Find("Text").GetComponent<Text>();
        chatEmoji = chatBox.transform.Find("Emoji").GetComponent<Image>();
        spriteAnimation = chatEmoji.gameObject.GetComponent<SpriteAnimation>();

        operate = transform.Find("Operate").gameObject;
        dontCall = transform.Find("Operate/DontCall");
        dontDeal = transform.Find("Operate/DontDeal");
        dontGrab = transform.Find("Operate/DontGrab");
        mingPai = transform.Find("Operate/MingPai");
        callLandowner = transform.Find("Operate/CallLandowner");
        grabLandowner = transform.Find("Operate/GrabLandowner");

        dealArea = transform.Find("DealArea");
        cardDeal = Resources.Load<GameObject>("Perfabs/CardDeal");
    }

    public virtual void Start()
    {
        RenderHide();

        HideChatBox();
        HideOperate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        isDestory = true;
    }

    public override void Execute(int eventCode, object message) // UI事件码在基类绑定会绑定两次
    {
        if (isDestory) return;
        switch (eventCode)
        {
            case UIEvent.Send_Quick_Chat:
                var chatDto = (ChatDto)message;
                if (userDto == null || userDto.Id != chatDto.id) return;
                var text = ChatConstant.GetChatText(chatDto.Index);
                Dispatch(AreaCode.AUDIO, AudioEvent.Play_ChatEffect_Audio, chatDto.Index); // 播放音效
                SendChat(text);
                break;

            case UIEvent.Send_ZiDingYi_Chat:
                var chatDto1 = (ChatDto)message;
                if (userDto == null || userDto.Id != chatDto1.id) return;
                Dispatch(AreaCode.AUDIO, AudioEvent.Start_Speak_Text, chatDto1.text); // 朗读文本
                SendChat(chatDto1.text);
                break;

            case UIEvent.Send_Emoji_Chat:
                var chatDto2 = (ChatDto)message;
                if (userDto == null || userDto.Id != chatDto2.id) return;
                SendEmoji(chatDto2.text);
                break;

            case UIEvent.Pass_Round:
                RemoveDealArea();
                break;

            default:
                break;
        }
    }

    // 渲染隐藏
    protected void RenderHide()
    {
        userDto = null;
        gameObject.SetActive(false);
        readyText.gameObject.SetActive(false);
    }

    // 渲染显示
    protected void RenderShow(UserDto userDto)
    {
        this.userDto = userDto;
        userName.text = userDto.Name;
        beanCount.text = userDto.BeanCount.ToString();
        readyText.gameObject.SetActive(Models.GameModel.MatchRoomDto.readyList.Exists(item => item == userDto.Id));
        gameObject.SetActive(true);
    }

    // 卡牌计数动画
    protected void StartCountAnimation()
    {
        if (count >= 17)
        {
            if (StartGrabLandowner != null) StartGrabLandowner();
            return;
        }
        count++;
        cardAmout.text = count.ToString();
        Invoke(nameof(StartCountAnimation), .2f);
    }

    #region 出牌

    // 创建出牌 自身
    protected IEnumerator CreateDealArea(List<CardDto> cardDtos)
    {
        yield return new WaitForEndOfFrame();

        float space = 35.0f;
        for (var i = 0; i < cardDtos.Count; i++)
        {
            var card = Instantiate(cardDeal, dealArea);
            card.name = $"card{i}";
            card.GetComponent<CardDeal>().SetCard(cardDtos[i]);
            var rt = card.GetComponent<RectTransform>();
            var lastIndex = i - 1 < 0 ? 0 : i - 1;
            var lastPos = dealArea.Find($"card{lastIndex}").GetComponent<RectTransform>().anchoredPosition;
            rt.anchoredPosition = new Vector2(lastPos.x + space, lastPos.y);
        }

        var dealRt = dealArea.GetComponent<RectTransform>();
        var aurPos = dealRt.anchoredPosition;
        dealRt.anchoredPosition = new Vector2(-(dealArea.childCount * space / 2), aurPos.y);
    }

    // 创建出牌 左右
    protected IEnumerator CreateDealArea(DealDto dealDtos, bool isLeft = true)
    {
        yield return new WaitForEndOfFrame();

        float space = 35.0f;
        for (var i = 0; i < dealDtos.SelectCardList.Count; i++)
        {
            var card = Instantiate(cardDeal, dealArea);
            card.name = $"card{i}";
            card.GetComponent<CardDeal>().SetCard(dealDtos.SelectCardList[i]);
            var rt = card.GetComponent<RectTransform>();
            var lastIndex = i - 1 < 0 ? 0 : i - 1;
            var lastPos = dealArea.Find($"card{lastIndex}").GetComponent<RectTransform>().anchoredPosition;
            rt.anchoredPosition = new Vector2(lastPos.x + space, lastPos.y);

            if (isLeft)
            {
                rt.anchorMin = new Vector2(0, .5f);
                rt.anchorMax = new Vector2(0, .5f);
            }
            else
            {
                rt.anchorMin = new Vector2(1, .5f);
                rt.anchorMax = new Vector2(1, .5f);
            }
        }
    }

    // 移除出牌
    protected void RemoveDealArea()
    {
        List<GameObject> removeList = new List<GameObject>();
        for (var i = 0; i < dealArea.childCount; i++) removeList.Add(dealArea.GetChild(i).gameObject);
        for (var i = 0; i < removeList.Count; i++) Destroy(removeList[i]);
    }

    #endregion 出牌

    #region 聊天消息相关

    // 发送消息显示动画
    private void SendChat(string text)
    {
        chatBox.SetActive(true);
        StartChatAnimation();
        chatText.text = text;
        SetTypeActive(true);

        CreateChatHistory(text);
        Dispatch(AreaCode.UI, UIEvent.Render_Chat_ScrollView, null); // 刷新滚动视图
    }

    // 发送表情显示动画
    private void SendEmoji(string name)
    {
        chatBox.SetActive(true);
        StartChatAnimation();
        spriteAnimation.SpriteFrames = Resources.LoadAll<Sprite>($"Image/Chat/Emoji/{name}");
        SetTypeActive(false);

        CreateChatHistory(null, name);
        Dispatch(AreaCode.UI, UIEvent.Render_Chat_ScrollView, null); // 刷新滚动视图
    }

    //  创建聊天历史记录
    private void CreateChatHistory(string text = null, string emojiName = null)
    {
        ChatHistoryDto chatHistoryDto = new ChatHistoryDto
        {
            Name = userDto.Name,
            Chat = text,
            EmojiName = emojiName
        };
        Dispatch(AreaCode.UI, UIEvent.Create_Chat_History, chatHistoryDto);
    }

    // 设置消息类型显示
    private void SetTypeActive(bool chatActive)
    {
        chatText.gameObject.SetActive(chatActive);
        chatEmoji.gameObject.SetActive(!chatActive);
    }

    // 聊天动画
    private void StartChatAnimation()
    {
        if (chatTween != null)
        {
            chatTween.Kill();
            CancelInvoke(nameof(HideChatBox));
        }
        chatTween = DotweenTools.DoTransScale(chatBox.transform, Vector3.zero, Vector3.one, .2f);
        chatTween.onComplete = () =>
        {
            Invoke(nameof(HideChatBox), 5.0f);
        };
    }

    // 消息隐藏
    private void HideChatBox() => chatBox.SetActive(false);

    #endregion 聊天消息相关

    #region 操作

    // 隐藏所有操作
    public void HideOperate()
    {
        for (int i = 0; i < operate.transform.childCount; i++)
        {
            operate.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // 明牌 所有
    public void MingPai()
    {
        HideOperate();
        mingPai.gameObject.SetActive(true);
    }

    // 显示抢地主 自己

    public void Show_GrabLandowner()
    {
        HideOperate();
        grabLandowner.gameObject.SetActive(true);
    }

    // 显示闹钟 左右玩家
    public void Show_Timer()
    {
        HideOperate();
        timer.gameObject.SetActive(true);
    }

    // 显示不抢
    public void Show_DontGrabe()
    {
        HideOperate();
        dontGrab.gameObject.SetActive(true);
    }

    // 隐藏不抢
    public void Hide_DontGrabe() => dontGrab.gameObject.SetActive(false);

    // 显示不出
    public void Show_DontDeal()
    {
        HideOperate();
        dontDeal.gameObject.SetActive(true);
    }

    #endregion 操作
}