using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TableCavansArea : UIBase
{
    private GameObject MatchTips; // 匹配提示
    private GameObject StartBtn; // 开始按钮

    private Button mingPaiStart; // 明牌开始
    private Button defaultStart; // 默认开始

    private Image shiWei; // 匹配时间十位
    private Image geWei;  // 匹配时间个位
    private int startTime = 30; // 起始计时

    private Queue<RectTransform> rectQueue = new Queue<RectTransform>(); // 动画 rectTrans队列
    public List<Sprite> imageList = new List<Sprite>(); // 0-9 的图片资源

    private void Awake()
    {
        StartBtn = transform.Find("StartBtn").gameObject;
        mingPaiStart = StartBtn.transform.Find("MingPaiStart").GetComponent<Button>();
        defaultStart = StartBtn.transform.Find("DefaultStart").GetComponent<Button>();
        mingPaiStart.onClick.AddListener(OnClickMingPaiStart);
        defaultStart.onClick.AddListener(OnClickDefaultStart);

        MatchTips = transform.Find("MatchTips").gameObject;

        shiWei = MatchTips.transform.Find("Timer/ShiWei").GetComponent<Image>();
        geWei = MatchTips.transform.Find("Timer/Gewei").GetComponent<Image>();
    }

    private void Start()
    {
        SetMathchTipsActive(false);
    }

    /// <summary>
    /// 明牌开始
    /// </summary>
    private void OnClickMingPaiStart()
    {
        SetMathchTipsActive(true);
        SetStartBtnActive(false);
        InitAllAnimation();
        StartTimer();
    }

    /// <summary>
    /// 默认开始
    /// </summary>
    private void OnClickDefaultStart()
    {
        SetMathchTipsActive(true);
        SetStartBtnActive(false);
        InitAllAnimation();
        StartTimer();
    }

    /// <summary>
    /// 初始 入队 动画
    /// </summary>

    private void InitAllAnimation()
    {
        var parent = MatchTips.transform.Find("AnimationTips");
        for (var i = 0; i < parent.childCount; i++)
        {
            var rectTrans = parent.GetChild(i).GetComponent<RectTransform>();
            rectQueue.Enqueue(rectTrans);
            if (i == parent.childCount - 1) AnimationForVertical();
        }
    }

    /// <summary>
    /// 匹配动画方法 垂直序列动画
    /// </summary>
    /// <param name="rectTrans"></param>
    public void AnimationForVertical()
    {
        if (rectQueue.Count <= 0)
        {
            Invoke(nameof(InitAllAnimation), .5f);
            return;
        }
        var rectTrans = rectQueue.Dequeue();
        Sequence sequence = DOTween.Sequence();
        var startPos = rectTrans.anchoredPosition;
        var endPos = new Vector2(rectTrans.anchoredPosition.x, rectTrans.anchoredPosition.y + 10.0f);
        var t1 = DOTween.To(() => rectTrans.anchoredPosition, x => rectTrans.anchoredPosition = x, endPos, .2f);
        var t2 = DOTween.To(() => rectTrans.anchoredPosition, x => rectTrans.anchoredPosition = x, startPos, .2f);
        sequence
            .Append(t1)
            .Append(t2);
        sequence.Play();
        sequence.onComplete = () =>
        {
            sequence.Kill();
        };
        sequence.SetEase(Ease.Flash);
        Invoke(nameof(AnimationForVertical), .1f);
    }

    /// <summary>
    /// 设置匹配提示显示
    /// </summary>
    /// <param name="value"></param>
    private void SetMathchTipsActive(bool value) => MatchTips.SetActive(value);

    /// <summary>
    /// 设置起始按钮显示
    /// </summary>
    /// <param name="value"></param>
    private void SetStartBtnActive(bool value) => StartBtn.SetActive(value);

    /// <summary>
    /// 开始计时
    /// </summary>
    private void StartTimer()
    {
        switch (startTime % 10)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                break;

            case 5:
                break;

            case 6:
                break;

            case 7:
                break;

            case 8:
                break;

            case 9:
                break;

            default:
                break;
        }
        startTime -= 1;
        Invoke(nameof(StartTimer), 1.0f);
    }
}