using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class SceneMgr : ManagerBase
{
    public static SceneMgr Instance;
    public Action Temp_LoadedCallBack; // 临时委托变量 场景加载完成

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += SceneManager_Loaded;
        Add(SceneEvent.Load_Scence, this);
    }

    /// <summary>
    /// 场景加载完成回调
    /// </summary>
    /// <param name="arg0">场景名 索引等</param>
    /// <param name="arg1">加载模式</param>
    private void SceneManager_Loaded(Scene arg0, LoadSceneMode arg1)
    {
        if (Temp_LoadedCallBack != null) Temp_LoadedCallBack();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.Load_Scence:
                LoadSceneMsg loadSceneMsg = message as LoadSceneMsg;
                LoodScene(loadSceneMsg);
                break;

            default:
                break;
        }
    }

    private void LoodScene(LoadSceneMsg loadSceneMsg)
    {
        if (loadSceneMsg.sceneIndex != -1)
            SceneManager.LoadScene(loadSceneMsg.sceneIndex);
        if (loadSceneMsg.sceneName != null)
            SceneManager.LoadScene(loadSceneMsg.sceneName);
        Temp_LoadedCallBack = loadSceneMsg.loadedCallBack;

    }
}