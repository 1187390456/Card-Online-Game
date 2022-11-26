using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto;
using Protocol.Code;

public class LoginPanel : UIBase
{
    private Button loginBtn;
    private InputField usernameInput;
    private InputField passwordInput;

    private void Awake()
    {
        loginBtn = transform.Find("Account/LoginBtn").GetComponent<Button>();
        usernameInput = transform.Find("Account/Username/Input").GetComponent<InputField>();
        passwordInput = transform.Find("Account/Password/Input").GetComponent<InputField>();
    }

    private void Start()
    {
        loginBtn.onClick.AddListener(OnClickLogin);
        Bind(UIEvent.Start_Panel_Active);
        SetPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
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


    private void OnClickLogin()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            DispatchTools.Prompt_Msg(Dispatch, "账号不能为空!", Color.red);
            return;
        }
        if (string.IsNullOrEmpty(passwordInput.text))
        {
            DispatchTools.Prompt_Msg(Dispatch, "密码不能为空!", Color.red);
            return;
        }
        AccountDto accountDto = new AccountDto
        {
            Account = usernameInput.text,
            Password = passwordInput.text
        }; // 构造账号模型
        DispatchTools.Account_Login(Dispatch, accountDto);
    }
}