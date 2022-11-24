using Protocol.Code;
using Protocol.Code.SubCode;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UserHandler : HandlerBase
{
    private PromptMsg promptMsg = new PromptMsg();
    private SocketMsg socketMsg = new SocketMsg();

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.Create_Sres:
                Create((int)value);
                break;

            case UserCode.Onine_Sres:
                Online((int)value);
                break;

            case UserCode.Get_Sres:
                UserDto userDto = (UserDto)value;
                GetUserInfo(userDto);
                break;

            default:
                break;
        }
    }

    /// <summary>
    ///  创建角色
    /// </summary>
    /// <param name="value"></param>
    private void Create(int value)
    {
        switch (value)
        {
            case -1:
                PromptMsg("非法登录!", Color.red);
                break;

            case -2:
                PromptMsg("重复创建!", Color.red);
                break;

            case 0:
                // 创建成功 获取角色信息 上线角色
                socketMsg.Change(OpCode.User, UserCode.Get_Cres, null);
                Dispatch(AreaCode.NET, 0, socketMsg);

                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 获取角色信息
    /// </summary>
    /// <param name="value"></param>
    private void GetUserInfo(UserDto value)
    {
        if (value == null)
        {
            PromptMsg("服务端数据错误", Color.red);
            return;
        }
        Models.GameModel.UserDto = value; // 存储角色数据

        if (UserInfoArea.Instance != null) Dispatch(AreaCode.UI, UIEvent.UserInfoArea_RenderView, value); // 刷新角色信息
    }

    /// <summary>
    /// 上线
    /// </summary>
    /// <param name="value"></param>
    private void Online(int value)
    {
        switch (value)
        {
            case -1:
                PromptMsg("非法登录!", Color.red);
                break;

            case -2:
                PromptMsg("不存在角色!", Color.red);
                break;

            case -3:
                PromptMsg("角色已在线上!", Color.red);
                break;

            case 0:
                LoadSceneMsg loadSceneMsg = new LoadSceneMsg(1, () => PromptMsg("登录成功!", Color.green));
                Dispatch(AreaCode.SCENCE, SceneEvent.Load_Scence, loadSceneMsg);
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