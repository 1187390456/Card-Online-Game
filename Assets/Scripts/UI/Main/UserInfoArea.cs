using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Protocol.Dto;
using UnityEngine.Networking;

public class UserInfoArea : UIBase
{
    private Button Exit;

    private RawImage Avatar;
    private RawImage AvatarMask;

    private Text Name;
    private Transform Rank;
    private Image RankLogo;
    private Text RankName;
    private Transform Grade;
    private Image GradeLogo;
    private Text GradeName;

    private void Awake()
    {
        Exit = transform.Find("AvatarArea/Exit").GetComponent<Button>();
        Exit.onClick.AddListener(OnClickExit);

        Avatar = transform.Find("AvatarArea/Avatar").GetComponent<RawImage>();
        AvatarMask = Avatar.transform.GetChild(0).GetComponent<RawImage>();

        Name = transform.Find("InfoArea/Name").GetComponent<Text>();

        Rank = transform.Find("InfoArea/Scroll/Rank");
        RankLogo = Rank.GetComponentInChildren<Image>();
        RankName = Rank.GetComponentInChildren<Text>();

        Grade = transform.Find("InfoArea/Scroll/Grade");
        GradeLogo = Grade.GetComponent<Image>();
        GradeName = Grade.GetComponentInChildren<Text>();

        Bind(UIEvent.UserInfoArea_RenderView);
    }

    private void Start()
    {
        Grade.gameObject.SetActive(false);
        StartAnimation();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Exit.onClick.RemoveAllListeners();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UserInfoArea_RenderView:
                UserDto userDto = (UserDto)message;
                RenderView(userDto);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 刷新角色信息
    /// </summary>
    private void RenderView(UserDto userDto)
    {
        Name.text = userDto.Name;
        RankName.text = userDto.RankName;
        GradeName.text = userDto.GradeName;
        StartCoroutine(GetImage(userDto.Avatar, Avatar));
        StartCoroutine(GetImage(userDto.AvatarMask, AvatarMask));
    }

    /// <summary>
    /// 起始动画
    /// </summary>
    private void StartAnimation()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, .4f);
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    private void OnClickExit()
    {
        LoadSceneMsg loadSceneMsg = new LoadSceneMsg(0, () =>
          {
          });
        Dispatch(AreaCode.SCENCE, SceneEvent.Load_Scence, loadSceneMsg);
    }

    private IEnumerator GetImage(string url, RawImage rawImage)
    {
        // 暂时先用
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error)) rawImage.texture = www.texture;
        }
    }
}