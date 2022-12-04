using Protocol.Constant;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Spine.Unity;

public class BasePlayer : UIBase
{
    private bool isDestory = false;

    protected UserDto userDto;
    protected Text userName;
    protected Text beanCount;
    protected Text readyText;

    protected Tween chatTween;
    protected GameObject chatBox;
    protected Text chatText;
    protected Image chatEmoji;
    protected SpriteAnimation spriteAnimation; // 控制表情帧动画


    public virtual void Awake()
    {
        userName = transform.Find("UserInfo/NameBox/Name").GetComponent<Text>();
        readyText = transform.Find("Ready").GetComponent<Text>();

        chatBox = transform.Find("Chat").gameObject;
        chatText = chatBox.transform.Find("Text").GetComponent<Text>();
        chatEmoji = chatBox.transform.Find("Emoji").GetComponent<Image>();
        spriteAnimation = chatEmoji.gameObject.GetComponent<SpriteAnimation>();

        Bind(UIEvent.Send_Quick_Chat, UIEvent.Send_ZiDingYi_Chat, UIEvent.Send_Emoji_Chat);
    }

    public virtual void Start()
    {
        readyText.gameObject.SetActive(false);
        gameObject.SetActive(false);
        chatBox.SetActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        isDestory = true;
    }

    public override void Execute(int eventCode, object message)
    {
        if (isDestory) return;
        switch (eventCode)
        {
            case UIEvent.Send_Quick_Chat:
                var chatDto = (ChatDto)message;
                if (userDto == null || chatDto.id != userDto.Id) return;
                var text = ChatConstant.GetChatText(chatDto.Index);
                SendChat(text);
                break;

            case UIEvent.Send_ZiDingYi_Chat:
                var chatDto1 = (ChatDto)message;
                if (userDto == null || chatDto1.id != userDto.Id) return;
                SendChat(chatDto1.text);
                break;

            case UIEvent.Send_Emoji_Chat:
                var chatDto2 = (ChatDto)message;
                if (userDto == null || chatDto2.id != userDto.Id) return;
                SendEmoji(chatDto2.text);
                break;

            default:
                break;
        }
    }

    // 渲染隐藏
    public void RenderHide()
    {
        userDto = null;
        gameObject.SetActive(false);
        readyText.gameObject.SetActive(false);
    }

    // 渲染显示
    public void RenderShow(UserDto userDto)
    {
        this.userDto = userDto;
        userName.text = userDto.Name;
        beanCount.text = userDto.BeanCount;
        readyText.gameObject.SetActive(Models.GameModel.MatchRoomDto.readyList.Exists(item => item == userDto.Id));
        gameObject.SetActive(true);
    }

    // 发送消息显示动画
    public void SendChat(string text, Action action = null)
    {
        chatBox.SetActive(true);
        StartChatAnimation();

        chatText.text = text;

        SetTypeActive(true);
    }
    // 发送表情显示动画
    public void SendEmoji(string name)
    {
        chatBox.SetActive(true);
        StartChatAnimation();

        spriteAnimation.SpriteFrames = Resources.LoadAll<Sprite>($"Image/Chat/Emoji/{name}");

        SetTypeActive(false);
    }

    // 设置消息类型显示
    public void SetTypeActive(bool chatActive)
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
            CancelInvoke(nameof(SendQuickChatHide));
        }
        chatTween = DotweenTools.DoTransScale(chatBox.transform, Vector3.zero, Vector3.one, .2f);
        chatTween.onComplete = () =>
        {
            Invoke(nameof(SendQuickChatHide), 3.0f);
        };
    }

    // 消息隐藏
    private void SendQuickChatHide() => chatBox.SetActive(false);

}