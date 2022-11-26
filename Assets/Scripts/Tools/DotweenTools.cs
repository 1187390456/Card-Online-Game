using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画工具类
/// </summary>
public static class DotweenTools
{
    /// <summary>
    /// 基于Rect 从指定位置到指定位置
    /// </summary>
    /// <param name="target">目标rect</param>
    /// <param name="startPos">起始位置</param>
    /// <param name="endPos">终点位置</param>
    /// <param name="duration">持续时间</param>
    public static void DoRectMove(RectTransform targetRect, Vector2 startPos, Vector2 endPos, float duration)
    {
        targetRect.anchoredPosition = startPos;
        DOTween.To(() => targetRect.anchoredPosition, x => targetRect.anchoredPosition = x, endPos, duration);
    }
    /// <summary>
    /// 基于Tran 从指定大小缩放到指定大小
    /// </summary>
    /// <param name="targetTran">目标Tran</param>
    /// <param name="startScale">起始大小</param>
    /// <param name="endScale">终点大小</param>
    /// <param name="duration">持续时间</param>
    public static void DoTransScale(Transform targetTrans, Vector3 startScale, Vector3 endScale, float duration)
    {
        targetTrans.localScale = startScale;
        targetTrans.DOScale(endScale, duration);
    }

}
