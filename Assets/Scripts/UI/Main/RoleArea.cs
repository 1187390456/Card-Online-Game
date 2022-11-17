using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleArea : UIBase
{
    private Sequence roleSequence;

    private void Start()
    {
        StartAnimation();
    }

    /// <summary>
    /// 起始动画
    /// </summary>
    private void StartAnimation()
    {
        transform.localPosition = new Vector3(-1100.0f, -30.0f, 0.0f); //  起点
        if (roleSequence != null)
        {
            roleSequence.Kill();
        }
        roleSequence = DOTween.Sequence();
        roleSequence
            .Append(transform.DOLocalMove(new Vector3(-668.0f, -30.0f, 0.0f), .3f)).SetEase(Ease.InOutSine) // 经过点
            .Join(transform.DOScale(new Vector3(1.1f, 1.0f, 1.0f), .3f)).SetDelay(.2f)
            .Append(transform.DOLocalMove(new Vector3(-768.0f, -30.0f, 0.0f), .3f)) // 终点
            .Join(transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), .3f));

        roleSequence.Play();
    }
}