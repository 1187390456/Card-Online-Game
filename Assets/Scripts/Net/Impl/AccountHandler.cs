using Protocol.Code;
using Protocol.Code.SubCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AccountHandler : HandlerBase
{
    private PromptMsg promptMsg = new PromptMsg();

    /// <summary>
    /// 接收账户数据
    /// </summary>
    /// <param name="subCode"></param>
    /// <param name="value"></param>
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.Regist_Sres:
                RegistRes((int)value);
                break;

            case AccountCode.LOGIN:
                LoginRes((int)value);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 登录响应
    /// </summary>
    private void LoginRes(int value)
    {
        switch (value)
        {
            case 0: // 不存在角色
                LoadSceneMsg loadSceneMsg0 = new LoadSceneMsg(3, () =>
                {
                });
                Dispatch(AreaCode.SCENCE, SceneEvent.Load_Scence, loadSceneMsg0);

                break;

            case -1:

                PromptMsg("账号不存在!", Color.red);
                break;

            case -2:
                PromptMsg("账号已登录!", Color.red);
                break;

            case -3:
                PromptMsg("密码错误!", Color.red);
                break;

            case 1: // 存在角色
                LoadSceneMsg loadSceneMsg1 = new LoadSceneMsg(1, () =>
                {
                    // 获取角色信息
                    SocketMsg socketMsg = new SocketMsg(OpCode.User, UserCode.Get_Cres, null);
                    Dispatch(AreaCode.NET, 0, socketMsg);
                });
                Dispatch(AreaCode.SCENCE, SceneEvent.Load_Scence, loadSceneMsg1);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 注册响应
    /// </summary>
    private void RegistRes(int value)
    {
        switch (value)
        {
            case 0:
                PromptMsg("注册成功!", Color.green);
                Dispatch(AreaCode.UI, UIEvent.Regist_Panel_Active, false);
                break;

            case -1:

                PromptMsg("账号已存在!", Color.red);
                break;

            case -2:
                PromptMsg("账号不合法!", Color.red);
                break;

            case -3:
                PromptMsg("密码不合法!", Color.red);
                break;

            default:
                break;
        }
    }

    /// <summary>
    ///  发送消息面板封装
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="color">颜色</param>
    private void PromptMsg(string value, Color color)
    {
        promptMsg.Change(value, color);
        Dispatch(AreaCode.UI, UIEvent.Prompt_Msg, promptMsg);
    }
}