using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptPanel : UIBase
{
    private Text tips;
    private CanvasGroup canvasGroup;

    [SerializeField]
    [Range(0, 3)]
    private float showTime = 1.0f;

    private float timer = 0;

    private void Awake()
    {
        tips = transform.Find("TipsBg/Tips").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();

        Bind(UIEvent.Prompt_Msg);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Prompt_Msg:
                PromptMsg promptMsg = message as PromptMsg;
                PromptMessage(promptMsg.text, promptMsg.color);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 提示消息
    /// </summary>
    private void PromptMessage(string text, Color color)
    {
        tips.text = text;
        tips.color = color;
        canvasGroup.alpha = 0;
        timer = 0;

        StartCoroutine(StartAnimation());
    }

    /// <summary>
    /// 显示动画
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartAnimation()
    {
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
        while (timer < showTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
    }
}