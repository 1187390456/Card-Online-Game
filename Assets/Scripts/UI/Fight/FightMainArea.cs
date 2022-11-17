using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FightMainArea : UIBase
{
    private RectTransform npcLeft;
    private RectTransform npcRight;
    private RectTransform player;

    private void Awake()
    {
        npcLeft = transform.Find("NPCLeft").GetComponent<RectTransform>();
        npcRight = transform.Find("NPCRight").GetComponent<RectTransform>();
        player = transform.Find("Player").GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartAnimation();
    }

    /// <summary>
    /// 起始动画
    /// </summary>
    private void StartAnimation()
    {
        AnimationHorizontalMove(-500.0f, npcLeft);
        AnimationHorizontalMove(430.0f, npcRight);
        AnimationHorizontalMove(-400.0f, player);
    }

    /// <summary>
    ///  动画水平移动 修改auchoredPos
    /// </summary>
    /// <param name="startPosX">起始水平x轴位置</param>
    /// <param name="rectTransform">移动物体的rectTransform</param>
    private void AnimationHorizontalMove(float startPosX, RectTransform rectTransform)
    {
        var endPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(startPosX, rectTransform.anchoredPosition.y);
        DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x, endPos, .4f);
    }
}