using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperateBtn : UIBase
{
    private Button mingPaiBtn;

    private GameObject grabLandowner;
    private Button grabLandownerBtn;
    private Button dontGrabBtn;

    private void Awake()
    {
        mingPaiBtn = transform.Find("MingPai").GetComponent<Button>();

        grabLandowner = transform.Find("GrabLandowner").gameObject;
        grabLandownerBtn = transform.Find("GrabLandowner/GrabLandowner").GetComponent<Button>();
        dontGrabBtn = transform.Find("GrabLandowner/DontGrab").GetComponent<Button>();

        grabLandownerBtn.onClick.AddListener(OnClickGrabLandowner);
        dontGrabBtn.onClick.AddListener(OnClickDontGrab);

        Bind(UIEvent.Set_MingPaiBtn_Active, UIEvent.Set_GrabLandownerBtn_Active);
    }

    private void Start() => HideOperate();

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Set_MingPaiBtn_Active:
                HideOperate();
                SetMingPaiBtnActive((bool)message);
                break;
            case UIEvent.Set_GrabLandownerBtn_Active:
                HideOperate();
                SetGrabLandownerActive((bool)message);
                break;

            default:
                break;
        }
    }

    // 操作隐藏
    private void HideOperate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetMingPaiBtnActive(bool value) => mingPaiBtn.gameObject.SetActive(value);  // 明牌

    private void SetGrabLandownerActive(bool value) => grabLandowner.gameObject.SetActive(value); // 显示抢地主

    #region 监听事件

    private void OnClickGrabLandowner() // 抢地主
    {
        HideOperate();
        DispatchTools.Fight_Grab_Landowner_Cres(Dispatch, true);
    }
    private void OnClickDontGrab() // 不抢
    {
        HideOperate();
        DispatchTools.Fight_Grab_Landowner_Cres(Dispatch, false);
    }
    #endregion
}