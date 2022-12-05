using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : UIBase
{
    private void Start()
    {
        //TODO 标记测试账号0
        // TestCount();
        Dispatch(AreaCode.AUDIO, AudioEvent.Play_Music_Audio, BackGroundMuscicType.Welcome.ToString()); // 播放背景音乐
    }

    private void TestCount()
    {
        // 注册
        AccountDto accountDto1 = new AccountDto
        {
            Account = "1111",
            Password = "1111"
        };
        AccountDto accountDto2 = new AccountDto
        {
            Account = "2222",
            Password = "2222"
        };
        AccountDto accountDto3 = new AccountDto
        {
            Account = "3333",
            Password = "3333"
        };

        DispatchTools.Account_Regist_Cres(Dispatch, accountDto1);
        DispatchTools.Account_Regist_Cres(Dispatch, accountDto2);
        DispatchTools.Account_Regist_Cres(Dispatch, accountDto3);

        //  登录
        AccountDto accountDto = new AccountDto
        {
            Account = "1111",
            Password = "1111"
        }; // 构造账号模型
        DispatchTools.Account_Login(Dispatch, accountDto);
    }
}