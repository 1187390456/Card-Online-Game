using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrappingAdaptive : MonoBehaviour
{
    public float WrapWidth;

    private ContentSizeFitter contentSizeFitter;

    private RectTransform rectTransform;

    private void Awake()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (rectTransform != null && rectTransform.rect.width > WrapWidth)
        {
            SetParentFitter();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            rectTransform.sizeDelta = new Vector2(WrapWidth, 0);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    private void SetParentFitter()
    {
        transform.parent.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        transform.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}