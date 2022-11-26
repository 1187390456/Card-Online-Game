using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������
/// </summary>
public static class DotweenTools
{
    /// <summary>
    /// ����Rect ��ָ��λ�õ�ָ��λ��
    /// </summary>
    /// <param name="target">Ŀ��rect</param>
    /// <param name="startPos">��ʼλ��</param>
    /// <param name="endPos">�յ�λ��</param>
    /// <param name="duration">����ʱ��</param>
    public static void DoRectMove(RectTransform targetRect, Vector2 startPos, Vector2 endPos, float duration)
    {
        targetRect.anchoredPosition = startPos;
        DOTween.To(() => targetRect.anchoredPosition, x => targetRect.anchoredPosition = x, endPos, duration);
    }
    /// <summary>
    /// ����Tran ��ָ����С���ŵ�ָ����С
    /// </summary>
    /// <param name="targetTran">Ŀ��Tran</param>
    /// <param name="startScale">��ʼ��С</param>
    /// <param name="endScale">�յ��С</param>
    /// <param name="duration">����ʱ��</param>
    public static void DoTransScale(Transform targetTrans, Vector3 startScale, Vector3 endScale, float duration)
    {
        targetTrans.localScale = startScale;
        targetTrans.DOScale(endScale, duration);
    }

}
