using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BottomArea : UIBase
{
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        DotweenTools.DoRectMove(rectTransform, new Vector3(0.0f, -100.0f, 0.0f), rectTransform.anchoredPosition, .4f);
    }
}