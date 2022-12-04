using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ChatHistoryDto
{
    public string Name;
    public string Chat;
    public string EmojiName;
}

public class FightChat : UIBase
{
    public enum ChatType
    {
        Chat = 0,
        Emoji = 1,
        Record = 2
    };

    private GameObject chatHistory;

    private Toggle[] toggles = new Toggle[3];

    private GameObject chatScroll;
    private Transform chatContent;
    private GameObject emojiScroll;
    private Transform emojiContent;
    private GameObject recordScroll;
    private Transform recordContent;

    private Button[] chatBtns;
    private Button[] emojiBtns;

    private Button ziDingYiBtn;
    private InputField ziDingYiInput;

    private void Awake()
    {
        toggles[0] = transform.Find("Tabbar/Chat").GetComponent<Toggle>();
        toggles[1] = transform.Find("Tabbar/Emoji").GetComponent<Toggle>();
        toggles[2] = transform.Find("Tabbar/Record").GetComponent<Toggle>();

        chatScroll = transform.Find("ChatSroll").gameObject;
        chatContent = transform.Find("ChatSroll/Viewport/Content");
        emojiScroll = transform.Find("EmojiSroll").gameObject;
        emojiContent = transform.Find("EmojiSroll/Viewport/Content");

        recordScroll = transform.Find("RecordSroll").gameObject;
        recordContent = transform.Find("RecordSroll/Viewport/Content");
        ziDingYiBtn = transform.Find("RecordSroll/ZiDingYi/Send").GetComponent<Button>();
        ziDingYiInput = transform.Find("RecordSroll/ZiDingYi/Input").GetComponent<InputField>();

        chatHistory = Resources.Load<GameObject>("Perfabs/Chat/ChatHistory");

        Bind(UIEvent.Chat_Panel_Active, UIEvent.Create_Chat_History, UIEvent.Crear_All_History);
    }

    private void Start()
    {
        AddListen();
        SetToggleActive(ChatType.Chat);
        SetChatPanle(false);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Chat_Panel_Active:
                SetChatPanle((bool)message);
                break;

            case UIEvent.Create_Chat_History:
                CreateChatHistory((ChatHistoryDto)message);
                break;

            case UIEvent.Crear_All_History:
                ClearAllChatHistory();
                break;

            default:
                break;
        }
    }

    // 添加历史聊天记录
    private void CreateChatHistory(ChatHistoryDto chatHistoryDto)
    {
        var chat = Instantiate(chatHistory, recordContent);
        if (chatHistoryDto.Chat != null) chat.GetComponent<ChatHistory>().SetChatHistory(chatHistoryDto.Name, chatHistoryDto.Chat);
        if (chatHistoryDto.EmojiName != null) chat.GetComponent<ChatHistory>().SetEmojiHistory(chatHistoryDto.Name, chatHistoryDto.EmojiName);
    }

    // 清空历史记录

    private void ClearAllChatHistory()
    {
        for (int i = 0; i < recordContent.childCount; i++)
        {
            var temp = i;
            Destroy(recordContent.GetChild(temp).gameObject);
        }
    }

    // 添加监听
    private void AddListen()
    {
        //  chat 按钮
        chatBtns = new Button[chatContent.childCount];
        for (int i = 0; i < chatBtns.Length; i++)
        {
            var temp = i;
            chatBtns[temp] = chatContent.GetChild(temp).GetComponent<Button>();
            chatBtns[temp].onClick.AddListener(() => OnChatBtnClick(temp));
        }

        // emoji 按钮
        emojiBtns = new Button[emojiContent.childCount];
        for (int i = 0; i < emojiBtns.Length; i++)
        {
            var temp = i;
            emojiBtns[temp] = emojiContent.GetChild(temp).GetComponent<Button>();
            var name = emojiBtns[temp].gameObject.name;
            emojiBtns[temp].onClick.AddListener(() => OnEmojiBtnClick(name));
        }

        // 开关
        for (int i = 0; i < toggles.Length; i++)
        {
            var temp = i;
            toggles[temp].onValueChanged.AddListener((isOn) => OnToggleTrigger(isOn, temp));
        }

        // 自定义按钮
        ziDingYiBtn.onClick.AddListener(OnClickZiDingYiSend);
    }

    #region 触发

    private void OnToggleTrigger(bool isOn, int index)
    {
        if (index == 0 && isOn) SetToggleActive(ChatType.Chat);
        else if (index == 1 && isOn) SetToggleActive(ChatType.Emoji);
        else if (index == 2 && isOn) SetToggleActive(ChatType.Record);
    }

    private void OnChatBtnClick(int index) => DispatchTools.Chat_Send_Quick_Cres(Dispatch, index);

    private void OnEmojiBtnClick(string name) => DispatchTools.Chat_Send_Emoji_Cres(Dispatch, name);

    private void OnClickZiDingYiSend()
    {
        var text = ziDingYiInput.text;
        if (string.IsNullOrEmpty(text))
        {
            DispatchTools.Prompt_Msg(Dispatch, "发送消息不能为空!", Color.red);
            return;
        }
        DispatchTools.Chat_Send_ZiDingYi_Cres(Dispatch, text);
    }

    #endregion 触发

    // 设置聊天面板
    private void SetChatPanle(bool value) => gameObject.SetActive(value);

    // 设置开关显示面板
    private void SetToggleActive(ChatType chatType)
    {
        switch (chatType)
        {
            case ChatType.Chat:
                HideAllScroll();
                chatScroll.SetActive(true);
                break;

            case ChatType.Emoji:
                HideAllScroll();
                emojiScroll.SetActive(true);
                break;

            case ChatType.Record:
                HideAllScroll();
                recordScroll.SetActive(true);
                break;

            default:
                break;
        }
    }

    private void HideAllScroll()
    {
        chatScroll.SetActive(false);
        emojiScroll.SetActive(false);
        recordScroll.SetActive(false);
    }
}