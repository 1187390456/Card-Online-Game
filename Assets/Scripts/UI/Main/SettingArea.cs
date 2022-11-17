using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingArea : UIBase
{
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartAnimation();
    }

    /// <summary>
    /// 起始动画
    /// </summary>
    private void StartAnimation()
    {
        var endPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector3(rectTransform.rect.width / 2, 0.0f, 0.0f);
        DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x, endPos, .4f);
    }
}