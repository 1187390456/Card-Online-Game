using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperateBtn : UIBase
{
    private GameObject mingPai;
    private GameObject grabLandowner;

    private void Awake()
    {
        mingPai = transform.Find("MingPai").gameObject;
        Bind(UIEvent.Set_MingPaiBtn_Active);
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

    // 明牌
    private void SetMingPaiBtnActive(bool value) => mingPai.SetActive(value);
}