using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CreateArea : UIBase
{
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

        //TODO 标记测试账号1
        DispatchTools.User_Create_Cres(Dispatch, "测试账号");
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
        DotweenTools.DoRectMove(logoRect, new Vector2(-900.0f, logoRect.anchoredPosition.y), logoRect.anchoredPosition, .4f);
        DotweenTools.DoTransScale(panelTrans, Vector3.zero, Vector3.one, .4f);
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    private void OnClickEnter()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            DispatchTools.Prompt_Msg(Dispatch, "名称不能为空哦!", Color.red);
            return;
        }
        // 发起创建角色请求
        DispatchTools.User_Create_Cres(Dispatch, nameInput.text);
    }
}