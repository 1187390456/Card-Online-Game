using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ModelArea : UIBase
{
    private Button classifyModel;
    private Sequence closeSequence;

    private void Awake()
    {
        classifyModel = transform.Find("Area/Classic").GetComponent<Button>();

        classifyModel.onClick.AddListener(OnClickClassifyModel);
    }

    private void Start()
    {
        StartAnimation();

        //TODO 标记测试账号2
        OnClickClassifyModel();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        classifyModel.onClick.RemoveAllListeners();
        DOTween.KillAll();
    }

    /// <summary>
    /// 经典模式按钮点击
    /// </summary>
    private void OnClickClassifyModel() => DispatchTools.Load_Scence(Dispatch, 2);

    /// <summary>
    /// 起始动画
    /// </summary>
    private void StartAnimation()
    {
        transform.localPosition = new Vector3(800.0f, 0.0f, 0.0f);
        if (closeSequence != null)
        {
            closeSequence.Kill();
        }
        closeSequence = DOTween.Sequence();
        closeSequence
            .Append(transform.DOLocalMove(new Vector3(-60.0f, 0.0f, 0.0f), .3f)).SetEase(Ease.InOutSine)
            .Join(transform.DOScale(new Vector3(1.1f, 1.0f, 1.0f), .3f)).SetDelay(.2f)
            .Append(transform.DOLocalMove(new Vector3(40.0f, 0.0f, 0.0f), .3f))
            .Join(transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), .3f));

        closeSequence.Play();
    }
}