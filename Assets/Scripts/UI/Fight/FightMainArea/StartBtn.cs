using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBtn : UIBase
{
    private Button mingPaiStart; // 明牌开始
    private Button defaultStart; // 默认开始

    private void Awake()
    {
        mingPaiStart = transform.Find("MingPaiStart").GetComponent<Button>();
        defaultStart = transform.Find("DefaultStart").GetComponent<Button>();

        mingPaiStart.onClick.AddListener(OnClickMingPaiStart);
        defaultStart.onClick.AddListener(OnClickDefaultStart);

        Bind(UIEvent.Set_StartBtn_Active);
    }

    private void Start()
    {
        //TODO 测试用
        // OnClickDefaultStart();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        mingPaiStart.onClick.RemoveAllListeners();
        defaultStart.onClick.RemoveAllListeners();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Set_StartBtn_Active:
                SetStartBtnActive((bool)message);
                break;

            default:
                break;
        }
    }

    private void OnClickMingPaiStart() // 明牌开始
    {
        Dispatch(AreaCode.UI, UIEvent.Set_MatchTips_Active, true);
        SetStartBtnActive(false);
        DispatchTools.Match_Enter_Cres(Dispatch);
    }

    private void OnClickDefaultStart() // 默认开始
    {
        Dispatch(AreaCode.UI, UIEvent.Set_MatchTips_Active, true);
        SetStartBtnActive(false);
        DispatchTools.Match_Enter_Cres(Dispatch);
    }

    private void SetStartBtnActive(bool isShow) => gameObject.SetActive(isShow); // 设置开始按钮显示
}