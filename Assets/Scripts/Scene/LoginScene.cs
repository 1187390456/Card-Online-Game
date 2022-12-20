using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : UIBase
{
    private int loginCount = 1;

    private void Awake()
    {
        Bind(UIEvent.Account_Already_Login);
    }

    private void Start()
    {
        //TODO 测试用
        //  TestCount1();
        Dispatch(AreaCode.AUDIO, AudioEvent.Play_Music_Audio, BackGroundMuscicType.Welcome.ToString()); // 播放背景音乐
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Account_Already_Login:
                if (loginCount == 1)
                {
                    loginCount++;
                    TestCount2();
                }
                else if (loginCount == 2)
                {
                    loginCount++;
                    TestCount3();
                }
                break;

            default:
                break;
        }
    }

    private void TestCount1()
    {
        AccountDto accountDto1 = new AccountDto
        {
            Account = "1111",
            Password = "1111"
        };

        DispatchTools.Account_Regist_Cres(Dispatch, accountDto1);
        DispatchTools.Account_Login(Dispatch, accountDto1);
    }

    private void TestCount2()
    {
        AccountDto accountDto2 = new AccountDto
        {
            Account = "2222",
            Password = "2222"
        };
        DispatchTools.Account_Regist_Cres(Dispatch, accountDto2);
        DispatchTools.Account_Login(Dispatch, accountDto2);
    }

    private void TestCount3()
    {
        AccountDto accountDto3 = new AccountDto
        {
            Account = "3333",
            Password = "3333"
        };
        DispatchTools.Account_Regist_Cres(Dispatch, accountDto3);
        DispatchTools.Account_Login(Dispatch, accountDto3);
    }
}