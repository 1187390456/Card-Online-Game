using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FightTopArea : UIBase
{
    private Button Exit;
    private Transform Left;
    private Transform Right;

    private void Awake()
    {
        Left = transform.Find("Left");
        Right = transform.Find("Right");

        Exit = Left.Find("Exit").GetComponent<Button>();
        Exit.onClick.AddListener(OnClickExit);
    }

    private void Start()
    {
        StartAnimation();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Exit.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 起始动画
    /// </summary>
    private void StartAnimation()
    {
        Left.localScale = Vector3.zero;
        Right.localScale = Vector3.zero;

        Left.DOScale(Vector3.one, .4f);
        Right.DOScale(new Vector3(-1, 1, 1), .4f);
    }

    /// <summary>
    /// 点击退出
    /// </summary>
    private void OnClickExit()
    {
        LoadSceneMsg loadSceneMsg = new LoadSceneMsg(1, () =>
          {
          });
        Dispatch(AreaCode.SCENCE, SceneEvent.Load_Scence, loadSceneMsg);
    }
}