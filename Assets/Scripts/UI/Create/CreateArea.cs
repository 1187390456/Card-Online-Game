using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreateArea : UIBase
{
    private RectTransform logoRect;
    private Transform panelTrans;
    private Button enterBtn;

    private void Awake()
    {
        logoRect = transform.Find("Logo").GetComponent<RectTransform>();
        panelTrans = transform.Find("CreatePanel");
        enterBtn = transform.Find("CreatePanel/Enter").GetComponent<Button>();
        enterBtn.onClick.AddListener(OnClickEnter);
    }

    private void Start()
    {
        StartAnimation();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
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
        LoadSceneMsg loadSceneMsg = new LoadSceneMsg(1, () =>
        {
        });
        Dispatch(AreaCode.SCENCE, SceneEvent.Load_Scence, loadSceneMsg);
    }
}