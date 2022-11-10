using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : UIBase
{
    private Button startBtn;
    private Button registBtn;

    private void Awake()
    {
        startBtn = transform.Find("Login").GetComponent<Button>();
        registBtn = transform.Find("Register").GetComponent<Button>();
        startBtn.onClick.AddListener(OnStartClick);
        registBtn.onClick.AddListener(OnRegistClick);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        startBtn.onClick.RemoveAllListeners();
        registBtn.onClick.RemoveAllListeners();
    }

    private void OnStartClick()
    {
        Dispatch(AreaCode.UI, UIEvent.Start_Panel_Active, true);
    }

    private void OnRegistClick()
    {
        Dispatch(AreaCode.UI, UIEvent.Regist_Panel_Active, true);
    }
}