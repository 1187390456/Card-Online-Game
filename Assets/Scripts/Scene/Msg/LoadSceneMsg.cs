using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoadSceneMsg
{
    public string name { get; private set; }
    public int index { get; private set; }
    public Action loadedCallBack { get; private set; }

    public LoadSceneMsg()
    {

    }
    public void Change(int index = -1, Action loadedCallBack = null)
    {
        this.index = index;
        this.loadedCallBack = loadedCallBack;
    }
    public void Change(string name = null, Action loadedCallBack = null)
    {
        this.name = name;
        this.loadedCallBack = loadedCallBack;
    }
}