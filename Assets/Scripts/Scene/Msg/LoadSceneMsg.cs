using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoadSceneMsg
{
    public string sceneName { get; private set; }
    public int sceneIndex { get; private set; }
    public Action loadedCallBack { get; private set; }

    public LoadSceneMsg(int sceneIndex, Action loadedCallBack)
    {
        this.sceneIndex = sceneIndex;
        this.loadedCallBack = loadedCallBack;
        sceneName = null;
    }

    public LoadSceneMsg(string sceneName, Action loadedCallBack)
    {
        this.sceneName = sceneName;
        this.loadedCallBack = loadedCallBack;
        sceneIndex = -1;
    }
}