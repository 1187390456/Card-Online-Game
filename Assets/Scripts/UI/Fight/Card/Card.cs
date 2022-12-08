using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private Image BigWeight;
    private Image BigColor;
    private Image HugeColor;

    private Vector2[] rectPos = new Vector2[3]; // 右下花色对齐 默认正常

    private void Awake()
    {
        BigWeight = transform.Find("LeftBox/BigWeight").GetComponent<Image>();
        BigColor = transform.Find("LeftBox/BigColor").GetComponent<Image>();
        HugeColor = transform.Find("HugeColor").GetComponent<Image>();

        rectPos[0] = new Vector2(-15.0f, 20.0f); // 正常花色
        rectPos[1] = new Vector2(-5.0f, 15.0f); // 小王
        rectPos[2] = new Vector2(-10.0f, 15.0f); // 大王
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
}