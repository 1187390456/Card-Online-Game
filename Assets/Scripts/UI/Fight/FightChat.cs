using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class FightChat : UIBase
{
    private Toggle[] toggles = new Toggle[2];

    private GameObject chatScroll;
    private Transform chatContent;
    private GameObject emojiScroll;
    private Transform emojiContent;

    private Button[] chatBtns;
    private Button ziDingYiBtn;
    private InputField ziDingYiInput;

    private Button[] emojiBtns;

    private void Awake()
    {
        toggles[0] = transform.Find("Tabbar/Chat").GetComponent<Toggle>();
        toggles[1] = transform.Find("Tabbar/Emoji").GetComponent<Toggle>();

        chatScroll = transform.Find("ChatSroll").gameObject;
        chatContent = transform.Find("ChatSroll/Viewport/Content");
        emojiScroll = transform.Find("EmojiSroll").gameObject;
        emojiContent = transform.Find("EmojiSroll/Viewport/Content");

        Bind(UIEvent.Chat_Panel_Active);
    }

    private void Start()
    {
        AddChatBtnListen();
        AddEmojiBtnListen();
        AddToggleListen();
        ChatActive();
        SetChatPanle(false);
    }

    private void AddToggleListen()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            var temp = i;
            toggles[temp].onValueChanged.AddListener((isOn) => OnToggleTrigger(isOn, temp));
        }
    }

    private void AddChatBtnListen()
    {
        chatBtns = new Button[chatContent.childCount];

        for (int i = 0; i < chatBtns.Length; i++)
        {
            var temp = i;
            chatBtns[temp] = chatContent.GetChild(temp).GetComponent<Button>();
            chatBtns[temp].onClick.AddListener(() => OnChatBtnClick(temp));
        }

        ziDingYiBtn = chatContent.GetChild(0).Find("Send").GetComponent<Button>();
        ziDingYiInput = chatContent.GetChild(0).Find("Input").GetComponent<InputField>();
        ziDingYiBtn.onClick.AddListener(OnClickZiDingYiSend);
    }

    private void AddEmojiBtnListen()
    {
        emojiBtns = new Button[emojiContent.childCount];

        for (int i = 0; i < emojiBtns.Length; i++)
        {
            var temp = i;
            emojiBtns[temp] = emojiContent.GetChild(temp).GetComponent<Button>();
            emojiBtns[temp].onClick.AddListener(() => OnEmojiBtnClick(temp));
        }
    }

    private void OnToggleTrigger(bool isOn, int index)
    {
        if (index == 0 && isOn)
        {
            // 聊天
            ChatActive();
        }
        else if (index == 1 && isOn)
        {
            // 表情
            EmojiActive();
        }
    }

    private void OnChatBtnClick(int index)
    {
        if (index == 0) return; // 第一个是自定义
        index--;
        DispatchTools.Chat_Send_Quick_Cres(Dispatch, index);
    }

    private void OnEmojiBtnClick(int index) => DispatchTools.Chat_Send_Emoji_Cres(Dispatch, index);

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

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Chat_Panel_Active:
                SetChatPanle((bool)message);
                break;

            default:
                break;
        }
    }

    private void SetChatPanle(bool value) => gameObject.SetActive(value);

    private void ChatActive()
    {
        chatScroll.SetActive(true);
        emojiScroll.SetActive(false);
    }

    private void EmojiActive()
    {
        chatScroll.SetActive(false);
        emojiScroll.SetActive(true);
    }
}