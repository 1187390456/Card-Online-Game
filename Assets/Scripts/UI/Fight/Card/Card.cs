using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private RectTransform rect;

    private Image BigWeight;
    private Image BigColor;
    private Image HugeColor;

    private Vector2[] rectPos = new Vector2[3]; // 右下花色对齐 默认正常

    private float yPos; // 记录初始Y位置
    private Tween t1 = null;
    private Tween t2 = null;

    private void Awake()
    {
        BigWeight = transform.Find("LeftBox/BigWeight").GetComponent<Image>();
        BigColor = transform.Find("LeftBox/BigColor").GetComponent<Image>();
        HugeColor = transform.Find("HugeColor").GetComponent<Image>();
        rect = transform.GetComponent<RectTransform>();

        rectPos[0] = new Vector2(-15.0f, 20.0f); // 正常花色
        rectPos[1] = new Vector2(-5.0f, 15.0f); // 小王
        rectPos[2] = new Vector2(-10.0f, 15.0f); // 大王

        yPos = rect.anchoredPosition.y;
    }

    public void SetCard(Sprite bigWeight, Sprite hugeColor, Sprite bigColor = null, bool isLJoker = false)
    {
        BigWeight.overrideSprite = bigWeight;
        HugeColor.overrideSprite = hugeColor;
        if (bigColor == null) // 大小王对齐位置 隐藏花色
        {
            if (isLJoker) HugeColor.GetComponent<RectTransform>().anchoredPosition = rectPos[2];
            else HugeColor.GetComponent<RectTransform>().anchoredPosition = rectPos[1];
            BigColor.gameObject.SetActive(false);
        }
        else BigColor.overrideSprite = bigColor;

        BigWeight.SetNativeSize();
        BigColor.SetNativeSize();
        HugeColor.SetNativeSize();
    }

    public void Move() // 卡牌移动
    {
        if (t1 != null || t2 != null) return;
        if (rect.anchoredPosition.y != yPos) // 不在初始位置
        {
            var startPos = rect.anchoredPosition;
            var endPos = new Vector2(startPos.x, yPos);
            t1 = DotweenTools.DoRectMove(rect, startPos, endPos, .2f);
            t1.onComplete = () => t1 = null;
        }
        else
        {
            var startPos = rect.anchoredPosition;
            var endPos = new Vector2(startPos.x, yPos + 15.0f);
            t2 = DotweenTools.DoRectMove(rect, startPos, endPos, .2f);
            t2.onComplete = () => t2 = null;
        }
    }
}