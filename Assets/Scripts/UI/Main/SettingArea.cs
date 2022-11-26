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
        DotweenTools.DoRectMove(rectTransform, new Vector3(rectTransform.rect.width / 2, 0.0f, 0.0f), rectTransform.anchoredPosition, .4f);
    }
}