using Protocol.Code;
using Protocol.Constant;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChatHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case ChatCode.Send_Quick_Bro:
                QuickSend((ChatDto)value);
                break;

            case ChatCode.Send_ZiDingYi_Bro:
                ZiDingYiSend((ChatDto)value);
                break;

            case ChatCode.Send_Emoji_Bro:
                EmojiSend((ChatDto)value);
                break;

            default:
                break;
        }
    }

    // 有人快捷发送
    private void QuickSend(ChatDto chatDto) => Dispatch(AreaCode.UI, UIEvent.Send_Quick_Chat, chatDto);

    // 有人自定义发送
    private void ZiDingYiSend(ChatDto chatDto) => Dispatch(AreaCode.UI, UIEvent.Send_ZiDingYi_Chat, chatDto);

    private void EmojiSend(ChatDto chatDto) => Dispatch(AreaCode.UI, UIEvent.Send_Emoji_Chat, chatDto);
}