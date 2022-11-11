using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PromptMsg
{
    public string text;
    public Color color;

    public PromptMsg()
    {
    }

    public PromptMsg(string text, Color color)
    {
        this.text = text;
        this.color = color;
    }

    /// <summary>
    /// 避免了频繁new对象
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    public void Change(string text, Color color)
    {
        this.text = text;
        this.color = color;
    }
}