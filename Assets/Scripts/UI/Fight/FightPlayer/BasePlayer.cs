using Protocol.Constant;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    protected Text cardAmout; // 手牌
    protected int count = 0; // 手牌数量

    public virtual void Awake()
    {
        userName = transform.Find("UserInfo/NameBox/Name").GetComponent<Text>();
        readyText = transform.Find("Ready").GetComponent<Text>();

        chatBox = transform.Find("Chat").gameObject;
        chatText = chatBox.transform.Find("Text").GetComponent<Text>();
        chatEmoji = chatBox.transform.Find("Emoji").GetComponent<Image>();
        spriteAnimation = chatEmoji.gameObject.GetComponent<SpriteAnimation>();
    }

    public virtual void Start()
    {
        RenderHide();
        chatBox.SetActive(false);
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
        if (count >= 17) return;
        count++;
        cardAmout.text = count.ToString();
        Invoke(nameof(StartCountAnimation), .2f);
    }

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
            CancelInvoke(nameof(SendQuickChatHide));
        }
        chatTween = DotweenTools.DoTransScale(chatBox.transform, Vector3.zero, Vector3.one, .2f);
        chatTween.onComplete = () =>
        {
            Invoke(nameof(SendQuickChatHide), 5.0f);
        };
    }

    // 消息隐藏
    private void SendQuickChatHide() => chatBox.SetActive(false);
}