/// <summary>
/// 存储所有UI事件码
/// </summary>
public class UIEvent
{
    public const int Start_Panel_Active = 0; // 登录面板显示
    public const int Regist_Panel_Active = 1; // 注册面包显示
    public const int UserInfoArea_RenderView = 2; // 角色信息面板刷新
    public const int Match_Success = 3; // 匹配成功
    public const int Prompt_Msg = 4; // 提示面板显

    public const int My_User_Render = 5; // 自己渲染
    public const int Left_User_Render = 6; // 左侧玩家渲染
    public const int Left_User_Leave = 7; // 左侧玩家离开
    public const int Right_User_Render = 8; //右侧玩家渲染
    public const int Right_User_Leave = 9; // 右侧玩家离开

    public const int Chat_Panel_Active = 10; // 聊天面板
    public const int Send_Quick_Chat = 11;     // 有人发送快捷消息
    public const int Send_ZiDingYi_Chat = 12; // 有人自定义发送
    public const int Send_Emoji_Chat = 13; // 有人发送表情

    public const int Create_Chat_History = 14; // 创建聊天历史
    public const int Crear_All_History = 15; // 清空聊天历史记录
    public const int Render_Chat_ScrollView = 16; // 刷新聊天滚动视图

    public const int Dispatch_Card = 17; // 分发玩家手牌

    public const int Set_MatchTips_Active = 18; // 设置匹配提示显示
    public const int Set_StartBtn_Active = 19; // 设置开始按钮显示
    public const int Set_ReadyBtn_Active = 20; // 设置准备按钮显示

    public const int Set_MingPaiBtn_Active = 21; // 显示明牌按钮
    public const int Start_Fight = 22; // 开始战斗
    public const int Set_GrabLandownerBtn_Active = 23; //  设置 开始抢地主 按钮 
    public const int Turn_GrabLandowner = 24; // 转换抢地主
    public const int GrabLandowner_Success = 25; // 抢地主成功

    public const int Set_TableCard_Active = 26; // 显示底牌盒子
    public const int Show_TableCard = 27; // 显示底牌

    public const int Turn_Deal = 28; //轮换出牌
    public const int Set_TurnPanel_Active = 29; // 设置出牌轮换面板


    // 测试用
    public const int Account_Already_Login = 99; // 账号已登录
}