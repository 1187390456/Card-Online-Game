using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo
{
    public string Avatar;
    public string AvatarMask;
    public string Name;
    public string RankLogo;
    public string RankName;
    public string GradeLogo;
    public string GradeName;

    public UserInfo(string avatar, string avatarMask, string name, string rankLogo, string rankName, string gradeLogo, string gradeName)
    {
        Avatar = avatar;
        AvatarMask = avatarMask;
        Name = name;
        RankLogo = rankLogo;
        RankName = rankName;
        GradeLogo = gradeLogo;
        GradeName = gradeName;
    }
}

public class UserInfoArea : UIBase
{
    private Image Avatar;
    private Image AvatarMask;
    private Text Name;
    private Transform Rank;
    private Image RankLogo;
    private Text RankName;
    private Transform Grade;
    private Image GradeLogo;
    private Text GradeName;

    private void Awake()
    {
        Avatar = transform.Find("AvatarArea/Avatar").GetComponent<Image>();
        AvatarMask = Avatar.GetComponentInChildren<Image>();

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
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UserInfoArea_RenderView:
                // 刷新角色信息
                //  RenderView();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 刷新角色信息
    /// </summary>
    private void RenderView(UserInfo userInfo)
    {
        Name.text = userInfo.Name;
        RankName.text = userInfo.RankName;
        GradeName.text = userInfo.GradeName;
    }
}