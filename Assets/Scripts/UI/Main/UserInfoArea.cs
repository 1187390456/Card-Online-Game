using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoArea : UIBase
{
    public static UserInfoArea Instance = null;
    private SocketMsg socketMsg = new SocketMsg();

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
        Instance = this;
    }

    private void Start()
    {
        Grade.gameObject.SetActive(false);

        DotweenTools.DoTransScale(transform, Vector3.zero, Vector3.one, .4f); // 起始动画
        DispatchTools.User_Get_Cres(Dispatch);     // 获取角色信息 刷新角色信息
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
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    private void OnClickExit()
    {
        // DispatchTools.Load_Scence(Dispatch, 0);
    }
}