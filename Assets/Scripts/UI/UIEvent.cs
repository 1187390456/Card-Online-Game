using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 存储所有UI事件码
/// </summary>
public class UIEvent
{
    public const int Start_Panel_Active = 0; // 登录面板显示
    public const int Regist_Panel_Active = 1; // 注册面包显示
    public const int UserInfoArea_RenderView = 2; // 角色信息面板刷新
    public const int Match_Success = 3; // 匹配成功

    public const int Prompt_Msg = 4; // 提示面板显示
}