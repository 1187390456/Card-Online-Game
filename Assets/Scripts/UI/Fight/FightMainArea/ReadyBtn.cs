using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyBtn : UIBase
{
    private Button readyBtn; // 准备按钮
    private Button cancelBtn; // 取消按钮

    private void Awake()
    {
        readyBtn = transform.Find("Ready").GetComponentInChildren<Button>();
        cancelBtn = transform.Find("Cancle").GetComponentInChildren<Button>();

        readyBtn.onClick.AddListener(OnClickReady);
        cancelBtn.onClick.AddListener(OnClickCancelReady);

        Bind(UIEvent.Set_ReadyBtn_Active, UIEvent.Match_Success);
    }

    private void Start() => SetReadyBtnActive(false);

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Set_ReadyBtn_Active:
                SetReadyBtnActive((bool)message);
                break;

            case UIEvent.Match_Success:
                SetActive(false);
                SetReadyBtnActive(true);
                break;

            default:
                break;
        }
    }

    private void OnClickReady() //准备
    {
        SetActive(true);
        DispatchTools.Match_Ready_Cres(Dispatch);
    }

    private void OnClickCancelReady() //取消准备
    {
        SetActive(false);
        DispatchTools.Match_CancleReady_Cres(Dispatch);
    }

    private void SetActive(bool isReady)
    {
        readyBtn.gameObject.SetActive(!isReady);
        cancelBtn.gameObject.SetActive(isReady);
    }

    private void SetReadyBtnActive(bool isShow) => gameObject.SetActive(isShow); // 设置准备按钮显示
}