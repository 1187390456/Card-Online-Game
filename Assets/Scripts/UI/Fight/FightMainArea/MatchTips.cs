using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchTips : UIBase
{
    private Sprite[] imageList; // 0-9 的图片资源

    private Queue<RectTransform> rectQueue = new Queue<RectTransform>(); // 动画队列
    private int sequenceIndex = 1; // 系列动画索引

    private Image shiWei; // 匹配时间十位
    private Image geWei;  // 匹配时间个位
    private int startTime = 30; // 起始计时

    private Button cancleMatch; // 取消匹配

    private void Awake()
    {
        imageList = Resources.LoadAll<Sprite>("Image/Scence/Fight/MatchTips");
        shiWei = transform.Find("Timer/ShiWei").GetComponent<Image>();
        geWei = transform.Find("Timer/Gewei").GetComponent<Image>();
        cancleMatch = transform.Find("Cancle").GetComponent<Button>();

        cancleMatch.onClick.AddListener(OnClickCancleMatch);

        Bind(UIEvent.Set_MatchTips_Active, UIEvent.Match_Success);
    }

    private void Start() => SetMatchTipsActive(false);

    public override void OnDestroy()
    {
        base.OnDestroy();
        StopAnimation();
        cancleMatch.onClick.RemoveAllListeners();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Set_MatchTips_Active:
                SetMatchTipsActive((bool)message);
                break;

            case UIEvent.Match_Success:
                SetMatchTipsActive(false);
                break;

            default:
                break;
        }
    }

    private void OnClickCancleMatch() // 取消匹配
    {
        Dispatch(AreaCode.UI, UIEvent.Set_StartBtn_Active, true);
        SetMatchTipsActive(false);
        DispatchTools.Match_Leave_Cres(Dispatch);
    }

    private void StartAnimation() // 开始动画
    {
        GetAnimationQueue();
        TimerAnimation();
        WordAnimation();
    }

    private void StopAnimation() // 停止动画
    {
        CancleAllInvoke();
        rectQueue.Clear();
        startTime = 30;
        sequenceIndex = 1;
        DOTween.KillAll();
    }

    private void GetAnimationQueue() // 获取动画队列
    {
        var parent = transform.Find("AnimationTips");
        for (var i = 0; i < parent.childCount; i++)
        {
            var rectTrans = parent.GetChild(i).GetComponent<RectTransform>();
            rectQueue.Enqueue(rectTrans);
        }
    }

    #region 文字动画

    public void WordAnimation() // 垂直动画方法 队列动画
    {
        var rectTrans = rectQueue.Dequeue();

        StartSequenceAnimation(rectTrans);

        rectQueue.Enqueue(rectTrans);

        if (sequenceIndex == 9)
        {
            sequenceIndex = 1;
            Invoke(nameof(WordAnimation), .5f);
        }
        else
        {
            sequenceIndex++;
            Invoke(nameof(WordAnimation), .1f);
        }
    }

    private void StartSequenceAnimation(RectTransform rectTrans) // 序列动画方法 来回
    {
        Sequence sequence = DOTween.Sequence();
        var startPos = rectTrans.anchoredPosition;
        var endPos = new Vector2(rectTrans.anchoredPosition.x, rectTrans.anchoredPosition.y + 12.0f);
        Tween t1 = DotweenTools.DoRectMove(rectTrans, endPos, .2f);
        Tween t2 = DotweenTools.DoRectMove(rectTrans, startPos, .2f);
        sequence.Append(t1).Append(t2);
        sequence.SetEase(Ease.Flash);
        sequence.onKill = () => rectTrans.anchoredPosition = startPos; // 被杀了就重置位置
    }

    #endregion 文字动画

    #region 计时器动画

    private void TimerAnimation() //  开始计时
    {
        if (startTime <= 0) startTime = 30;
        SwitchCase(startTime % 10, geWei);  // 个位
        SwitchCase((startTime % 100) / 10, shiWei); // 十位
        startTime -= 1;
        Invoke(nameof(TimerAnimation), 1.0f);
    }

    private void SwitchCase(int condition, Image targerImage) // 判断
    {
        switch (condition)
        {
            case 0:
                targerImage.overrideSprite = imageList[0];
                break;

            case 1:
                targerImage.overrideSprite = imageList[1];
                break;

            case 2:
                targerImage.overrideSprite = imageList[2];
                break;

            case 3:
                targerImage.overrideSprite = imageList[3];
                break;

            case 4:
                targerImage.overrideSprite = imageList[4];
                break;

            case 5:
                targerImage.overrideSprite = imageList[5];
                break;

            case 6:
                targerImage.overrideSprite = imageList[6];
                break;

            case 7:
                targerImage.overrideSprite = imageList[7];
                break;

            case 8:
                targerImage.overrideSprite = imageList[8];
                break;

            case 9:
                targerImage.overrideSprite = imageList[9];
                break;

            default:
                break;
        }
        targerImage.SetNativeSize();
    }

    #endregion 计时器动画

    private void SetMatchTipsActive(bool isShow) // 设置匹配提示显示
    {
        if (isShow)
        {
            gameObject.SetActive(true);
            StartAnimation();
        }
        else
        {
            gameObject.SetActive(false);
            StopAnimation();
        }
    }

    private void CancleAllInvoke() // 取消所有异步
    {
        CancelInvoke(nameof(WordAnimation));
        CancelInvoke(nameof(TimerAnimation));
    }
}