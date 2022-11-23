using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Protocol.Code;
using Protocol.Code.SubCode;

public class CreateArea : UIBase
{
    private PromptMsg promptMsg = new PromptMsg();
    private SocketMsg socketMsg = new SocketMsg();

    private RectTransform logoRect;
    private Transform panelTrans;
    private Button enterBtn;
    private InputField nameInput;

    private void Awake()
    {
        logoRect = transform.Find("Logo").GetComponent<RectTransform>();
        panelTrans = transform.Find("CreatePanel");
        enterBtn = transform.Find("CreatePanel/Enter").GetComponent<Button>();
        enterBtn.onClick.AddListener(OnClickEnter);

        nameInput = transform.Find("CreatePanel/UserInput").GetComponent<InputField>();
    }

    private void Start()
    {
        StartAnimation();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        enterBtn.onClick.RemoveAllListeners();
        DOTween.KillAll();
    }

    /// <summary>
    /// 开始动画
    /// </summary>
    private void StartAnimation()
    {
        var endPos = logoRect.anchoredPosition;
        logoRect.anchoredPosition = new Vector2(-900.0f, logoRect.anchoredPosition.y);
        DOTween.To(() => logoRect.anchoredPosition, x => logoRect.anchoredPosition = x, endPos, .4f);

        panelTrans.localScale = Vector3.zero;
        panelTrans.DOScale(Vector3.one, .4f);
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    private void OnClickEnter()
    {
        // TODO 服务端效验
        if (string.IsNullOrEmpty(nameInput.text))
        {
            promptMsg.Change("名称不能为空哦!", Color.red);
            Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
            return;
        }
        // 发起创建请求
        socketMsg.Change(OpCode.User, UserCode.Create_Cres, nameInput.text);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
}