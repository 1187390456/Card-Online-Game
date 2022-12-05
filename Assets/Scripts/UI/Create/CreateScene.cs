using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScene : UIBase
{
    private void Start()
    {
        //TODO 标记测试账号1
        DispatchTools.User_Create_Cres(Dispatch, "测试账号");
    }
}