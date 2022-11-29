using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq.Expressions;
using Protocol.Code;
using Protocol.Code.SubCode;
using Protocol.Dto;

public class FightMainArea : UIBase
{
    private SocketMsg socketMsg = new SocketMsg();

    public List<Sprite> imageList = new List<Sprite>(); // 0-9 的图片资源 非动态载入 不清除

    private GameObject MatchTips; // 匹配提示
    private GameObject StartBtn; // 开始按钮
    private GameObject ReadyArea; // 准备区域

    private Button mingPaiStart; // 明牌开始
    private Button defaultStart; // 默认开始
    private Button cancleMatch; // 取消匹配

    private Image shiWei; // 匹配时间十位
    private Image geWei;  // 匹配时间个位
    private int startTime = 30; // 起始计时

    private Queue<RectTransform> rectQueue = new Queue<RectTransform>(); // 动画 rectTrans队列
    private int sequenceIndex = 1; // 系列动画索引

    private Button readyBtn; // 准备按钮
    private Button cancelBtn; // 取消按钮
    private Text myPlayerReady; // 自己准备
    private Text leftPlayerReady; // 左边玩家准备
    private Text rightPlayerReady; // 右边玩家准备


    private void Awake()
    {
        StartBtn = transform.Find("StartBtn").gameObject;

        mingPaiStart = StartBtn.transform.Find("MingPaiStart").GetComponent<Button>();
        defaultStart = StartBtn.transform.Find("DefaultStart").GetComponent<Button>();
        mingPaiStart.onClick.AddListener(OnClickMingPaiStart);
        defaultStart.onClick.AddListener(OnClickDefaultStart);

        MatchTips = transform.Find("MatchTips").gameObject;
        cancleMatch = MatchTips.transform.Find("Cancle").GetComponent<Button>();
        cancleMatch.onClick.AddListener(OnClickCancleMatch);

        shiWei = MatchTips.transform.Find("Timer/ShiWei").GetComponent<Image>();
        geWei = MatchTips.transform.Find("Timer/Gewei").GetComponent<Image>();

        ReadyArea = transform.Find("ReadyArea").gameObject;
        readyBtn = transform.Find("ReadyArea/Ready").GetComponentInChildren<Button>();
        cancelBtn = transform.Find("ReadyArea/Cancle").GetComponentInChildren<Button>();
        readyBtn.onClick.AddListener(OnClickReady);
        cancelBtn.onClick.AddListener(OnClickCancel);
        myPlayerReady = transform.Find("ReadyArea/MyPlayerReady").GetComponent<Text>();
        leftPlayerReady = transform.Find("ReadyArea/LeftPlayerReady").GetComponent<Text>();
        rightPlayerReady = transform.Find("ReadyArea/RightPlayerReady").GetComponent<Text>();

        Bind(UIEvent.Match_Success, UIEvent.Check_User_Ready);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Match_Success:
                MatchSuccess();
                break;
            case UIEvent.Check_User_Ready:
                CheckReady();
                break;

            default:
                break;
        }
    }

    private void Start()
    {
        SetMathchTipsActive(false);
        InitReadyText();
        SetReadyAreaActive(false);
    }
    // 初始化准备文字
    private void InitReadyText()
    {
        myPlayerReady.gameObject.SetActive(false);
        leftPlayerReady.gameObject.SetActive(false);
        rightPlayerReady.gameObject.SetActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        RemoveAllBtnListen();
        CancleAllInvoke();
        ClearAllCollision();

        DOTween.KillAll();
    }

    /// <summary>
    /// 明牌开始
    /// </summary>
    private void OnClickMingPaiStart()
    {
        SetMathchTipsActive(true);
        SetStartBtnActive(false);

        StartAnimation();

        DispatchTools.Match_Enter_Cres(Dispatch);

    }

    /// <summary>
    /// 默认开始
    /// </summary>
    private void OnClickDefaultStart()
    {
        SetMathchTipsActive(true);
        SetStartBtnActive(false);

        StartAnimation();

        DispatchTools.Match_Enter_Cres(Dispatch);

    }

    /// <summary>
    /// 取消匹配
    /// </summary>
    private void OnClickCancleMatch()
    {
        SetMathchTipsActive(false);
        SetStartBtnActive(true);

        ResetAnimaton();

        DispatchTools.Match_Leave_Cres(Dispatch);
    }
    /// <summary>
    /// 匹配成功
    /// </summary>
    private void MatchSuccess()
    {
        DispatchTools.Prompt_Msg(Dispatch, "匹配成功!", Color.green);

        SetMathchTipsActive(false);
        SetReadyAreaActive(true);
        ResetAnimaton();

        cancelBtn.gameObject.SetActive(false);
        readyBtn.gameObject.SetActive(true);
    }
    /// <summary>
    /// 自己准备
    /// </summary>
    private void OnClickReady()
    {
        myPlayerReady.gameObject.SetActive(true);
        readyBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(true);

        DispatchTools.Match_Ready_Cres(Dispatch);
    }
    /// <summary>
    /// 自己取消准备
    /// </summary>
    private void OnClickCancel()
    {
        myPlayerReady.gameObject.SetActive(false);
        readyBtn.gameObject.SetActive(true);
        cancelBtn.gameObject.SetActive(false);

        DispatchTools.Match_CancleReady_Cres(Dispatch);
    }
    /// <summary>
    /// 检测准备
    /// </summary>
    private void CheckReady()
    {
        var leftPlayer = Models.GameModel.MatchRoomDto.leftUserDto;
        var rightPlayer = Models.GameModel.MatchRoomDto.rightUserDto;
        var readyList = Models.GameModel.MatchRoomDto.readyList;

        if (leftPlayer != null && readyList.Exists(item => item == leftPlayer.Id))
        {
            leftPlayerReady.gameObject.SetActive(true);
        }
        else
        {
            leftPlayerReady.gameObject.SetActive(false);
        }
        if (rightPlayer != null && readyList.Exists(item => item == rightPlayer.Id))
        {
            rightPlayerReady.gameObject.SetActive(true);
        }
        else
        {
            rightPlayerReady.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 重置动画
    /// </summary>
    private void ResetAnimaton()
    {
        CancleAllInvoke();
        ClearAllCollision();
        startTime = 30;
        sequenceIndex = 1;
        DOTween.KillAll();
    }

    /// <summary>
    /// 开始动画
    /// </summary>

    private void StartAnimation()
    {
        StartTimer();
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
        var rectTrans = rectQueue.Dequeue();

        StartSequenceAnimation(rectTrans);

        rectQueue.Enqueue(rectTrans);

        if (sequenceIndex == 9)
        {
            sequenceIndex = 1;
            Invoke(nameof(AnimationForVertical), .5f);
        }
        else
        {
            sequenceIndex++;
            Invoke(nameof(AnimationForVertical), .1f);
        }
    }

    /// <summary>
    ///  序列动画方法
    /// </summary>
    /// <param name="rectTrans"></param>

    private void StartSequenceAnimation(RectTransform rectTrans)
    {
        Sequence sequence = DOTween.Sequence();
        var startPos = rectTrans.anchoredPosition;
        var endPos = new Vector2(rectTrans.anchoredPosition.x, rectTrans.anchoredPosition.y + 12.0f);
        var t1 = DOTween.To(() => rectTrans.anchoredPosition, x => rectTrans.anchoredPosition = x, endPos, .2f);
        var t2 = DOTween.To(() => rectTrans.anchoredPosition, x => rectTrans.anchoredPosition = x, startPos, .2f);
        sequence
            .Append(t1)
            .Append(t2);
        sequence.SetEase(Ease.Flash);
        sequence.onKill = () =>
        {
            rectTrans.anchoredPosition = startPos;
        }; // 被杀了就重置位置
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    private void StartTimer()
    {
        if (startTime <= 0) startTime = 30;
        SwitchCase(startTime % 10, geWei);  // 个位
        SwitchCase((startTime % 100) / 10, shiWei); // 十位
        startTime -= 1;
        Invoke(nameof(StartTimer), 1.0f);
    }

    /// <summary>
    /// 计时动画条件
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="targerImage"></param>
    private void SwitchCase(int condition, Image targerImage)
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

    private void SetMathchTipsActive(bool value) => MatchTips.SetActive(value);

    private void SetReadyAreaActive(bool value) => ReadyArea.SetActive(value);

    private void SetStartBtnActive(bool value) => StartBtn.SetActive(value);

    /// <summary>
    /// 移除所有按钮监听
    /// </summary>
    private void RemoveAllBtnListen()
    {
        mingPaiStart.onClick.RemoveAllListeners();
        defaultStart.onClick.RemoveAllListeners();
        cancleMatch.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 取消所有异步事件
    /// </summary>
    private void CancleAllInvoke()
    {
        CancelInvoke(nameof(AnimationForVertical));
        CancelInvoke(nameof(StartTimer));
    }

    /// <summary>
    /// 清除所有集合
    /// </summary>
    private void ClearAllCollision()
    {
        rectQueue.Clear();
    }
}