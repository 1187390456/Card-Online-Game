using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase
{
    private Button closeBtn;
    private Button registBtn;
    private InputField usernameInput;
    private InputField passwordInput;
    private InputField passwordRepeatInput;

    private void Awake()
    {
        closeBtn = transform.Find("Bg/Close").GetComponent<Button>();
        registBtn = transform.Find("Account/RegistBtn").GetComponent<Button>();
        usernameInput = transform.Find("Account/Username/Input").GetComponent<InputField>();
        passwordInput = transform.Find("Account/Password/Input").GetComponent<InputField>();
        passwordRepeatInput = transform.Find("Account/AdfirmPassword/Input").GetComponent<InputField>();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClickClose);
        registBtn.onClick.AddListener(OnClickRegist);

        Bind(UIEvent.Regist_Panel_Active);
        SetPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        closeBtn.onClick.RemoveAllListeners();
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

    private void OnClickClose()
    {
        SetPanelActive(false);
    }

    private void OnClickRegist()
    {
        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(passwordInput.text) || string.IsNullOrEmpty(passwordRepeatInput.text)) return;
        if (passwordRepeatInput.text != passwordInput.text) return;
    }
}