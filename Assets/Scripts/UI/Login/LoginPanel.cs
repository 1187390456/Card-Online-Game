using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto;
using Protocol.Code;

public class LoginPanel : UIBase
{
    private Button closeBtn;
    private Button loginBtn;
    private InputField usernameInput;
    private InputField passwordInput;
    private PromptMsg promptMsg = new PromptMsg();
    private SocketMsg socketMsg = new SocketMsg();

    private void Awake()
    {
        closeBtn = transform.Find("Bg/Close").GetComponent<Button>();
        loginBtn = transform.Find("Account/LoginBtn").GetComponent<Button>();
        usernameInput = transform.Find("Account/Username/Input").GetComponent<InputField>();
        passwordInput = transform.Find("Account/Password/Input").GetComponent<InputField>();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClickClose);
        loginBtn.onClick.AddListener(OnClickLogin);

        Bind(UIEvent.Start_Panel_Active);
        SetPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        closeBtn.onClick.RemoveAllListeners();
        loginBtn.onClick.RemoveAllListeners();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Start_Panel_Active:
                SetPanelActive((bool)message);
                break;

            default:
                break;
        }
    }

    private void OnClickClose()
    {
        SetPanelActive(false);
    }

    private void OnClickLogin()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            promptMsg.Change("账号不能为空!", Color.red);
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(passwordInput.text))
        {
            promptMsg.Change("密码不能为空!", Color.red);
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        AccountDto accountDto = new AccountDto
        {
            Account = usernameInput.text,
            Password = passwordInput.text
        }; // 构造账号模型
        socketMsg.Change(OpCode.Account, AccountCode.Login, accountDto); // 根据账号模型生成消息类
        Dispatch(AreaCode.NET, 0, socketMsg); // 分发
    }
}