using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoadSceneMsg
{
    public string sceneName { get; private set; }
    public int sceneIndex { get; private set; }
    public Action loadedCallBack { get; private set; }

    public LoadSceneMsg()
    {

    }
    public void Change(int sceneIndex = -1, Action loadedCallBack = null)
    {
        this.sceneIndex = sceneIndex;
        this.loadedCallBack = loadedCallBack;
    }
    public void Change(string sceneName = null, Action loadedCallBack = null)
    {
        this.sceneName = sceneName;
        this.loadedCallBack = loadedCallBack;
    }
}