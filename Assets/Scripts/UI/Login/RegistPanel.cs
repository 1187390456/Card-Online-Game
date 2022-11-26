using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase
{
    private Button registBtn;
    private InputField usernameInput;
    private InputField passwordInput;
    private InputField passwordRepeatInput;
    private PromptMsg promptMsg = new PromptMsg();
    private SocketMsg socketMsg = new SocketMsg();

    private void Awake()
    {
        registBtn = transform.Find("Account/RegistBtn").GetComponent<Button>();
        usernameInput = transform.Find("Account/Username/Input").GetComponent<InputField>();
        passwordInput = transform.Find("Account/Password/Input").GetComponent<InputField>();
        passwordRepeatInput = transform.Find("Account/AdfirmPassword/Input").GetComponent<InputField>();
    }

    private void Start()
    {
        registBtn.onClick.AddListener(OnClickRegist);

        Bind(UIEvent.Regist_Panel_Active);
        SetPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        registBtn.onClick.RemoveAllListeners();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Regist_Panel_Active:
                SetPanelActive((bool)message);
                break;

            default:
                break;
        }
    }

    private void OnClickRegist()
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
        if (string.IsNullOrEmpty(passwordRepeatInput.text))
        {
            DispatchTools.Prompt_Msg(Dispatch, "重复不能为空!", Color.red);
            return;
        }

        if (passwordRepeatInput.text != passwordInput.text)
        {
            DispatchTools.Prompt_Msg(Dispatch, "输入的密码不一致!", Color.red);
            return;
        }

        AccountDto accountDto = new AccountDto
        {
            Account = usernameInput.text,
            Password = passwordInput.text
        };

        DispatchTools.Account_Regist_Cres(Dispatch, accountDto);
    }
}