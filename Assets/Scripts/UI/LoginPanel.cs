using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBase
{
    private Button closeBtn;
    private Button loginBtn;
    private InputField usernameInput;
    private InputField passwordInput;

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
        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(passwordInput.text)) return;
    }
}