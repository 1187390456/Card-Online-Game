using Protocol.Constant;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableCard : UIBase
{
    private List<CardDto> tableCardList;
    private Image[] imageList;
    private Sprite[] jokerList; // 4大王 5 小王 6 大王角色

    private Sprite tableCardBack; // 背面
    private Sprite tableCard; // 正面


    private void Awake()
    {
        imageList = new Image[transform.childCount];
        jokerList = Resources.LoadAll<Sprite>("Image/Card/Card");

        tableCardBack = Resources.Load<Sprite>("Image/Card/TableCardBack");
        tableCard = Resources.Load<Sprite>("Image/Card/TableCard");

        Bind(UIEvent.Show_TableCard, UIEvent.Set_TableCard_Active);
    }
    private void Start()
    {
        for (int i = 0; i < imageList.Length; i++) imageList[i] = transform.GetChild(i).GetComponent<Image>();
        SetTableCardActive(false);
        SetCardType(true);
    }
    // 设置底牌类型 正面 背面
    private void SetCardType(bool isBack)
    {
        for (int i = 0; i < imageList.Length; i++)
        {
            imageList[i].overrideSprite = isBack ? tableCardBack : tableCard;
            SetContentActive(imageList[i].gameObject, isBack);
            imageList[i].SetNativeSize();
        }
    }

    // 设置内容状态
    private void SetContentActive(GameObject gameObject, bool isBack)
    {
        for (var i = 0; i < gameObject.transform.childCount; i++) gameObject.transform.GetChild(i).gameObject.SetActive(!isBack);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.Show_TableCard:
                tableCardList = (List<CardDto>)message;
                SetCardType(false);
                for (var i = 0; i < tableCardList.Count; i++) SetCard(i);
                break;

            case UIEvent.Set_TableCard_Active:
                SetTableCardActive((bool)message);
                break;
            default:
                break;
        }
    }
    // 设置地面盒子状态
    private void SetTableCardActive(bool value) => gameObject.SetActive(value);

    // 设置卡牌
    private void SetCard(int index)
    {
        var weight = tableCardList[index].Weight;
        var color = tableCardList[index].Color;

        var leftWeight = imageList[index].transform.Find("Left/Weight").GetComponent<Image>();
        var leftColor = imageList[index].transform.Find("Left/Color").GetComponent<Image>();
        var rightColor = imageList[index].transform.Find("Right/HugColor").GetComponent<Image>();

        if (color != CardColor.None) // 不是大小王
        {
            var resColor = (color == CardColor.Heart || color == CardColor.Square) ? "Red" : "Black"; // 区分文件颜色

            leftWeight.overrideSprite = Resources.Load<Sprite>($"Image/Card/Weight/{resColor}/Small/{weight}"); // 左 权重
            leftColor.overrideSprite = Resources.Load<Sprite>($"Image/Card/Color/Small/{color}"); // 左 颜色
            rightColor.overrideSprite = Resources.Load<Sprite>($"Image/Card/Color/Huge/{color}"); // 右 颜色
        }
        else if (weight == CardWeight.SJoker)
        {
            leftColor.gameObject.SetActive(false);
            leftWeight.overrideSprite = jokerList[5];
            rightColor.overrideSprite = jokerList[7];
        }
        else if (weight == CardWeight.LJoker)
        {
            leftColor.gameObject.SetActive(false);
            leftWeight.overrideSprite = jokerList[4];
            rightColor.overrideSprite = jokerList[6];
        }
        leftWeight.SetNativeSize();
        leftColor.SetNativeSize();
        rightColor.SetNativeSize();
    }
}
