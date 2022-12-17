using Protocol.Constant;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDeal : MonoBehaviour
{
    private Image weight;
    private Image color;
    private Image joker;

    private Sprite[] jokerArr;

    private void Awake()
    {
        weight = transform.Find("Fix/Weight").GetComponent<Image>();
        color = transform.Find("Fix/Color").GetComponent<Image>();
        joker = transform.Find("Joker").GetComponent<Image>();

        jokerArr = Resources.LoadAll<Sprite>("Image/Card/Card");
    }
    private void Start() => joker.gameObject.SetActive(false);

    public void SetCard(CardDto cardDto)
    {
        var color = cardDto.Color;
        var weight = cardDto.Weight;

        if (color != CardColor.None) // 不是大小王
        {
            var resColor = (color == CardColor.Heart || color == CardColor.Square) ? "Red" : "Black"; // 区分文件颜色

            var weightSprite = Resources.Load<Sprite>($"Image/Card/Weight/{resColor}/Medium/{weight}");
            var cloreSprite = Resources.Load<Sprite>($"Image/Card/Color/Medium/{color}");

            this.color.overrideSprite = cloreSprite;
            this.weight.overrideSprite = weightSprite;
        }
        else if (weight == CardWeight.SJoker)
        {
            this.color.transform.gameObject.SetActive(false);
            this.color.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(20.0f, -45.0f);

            this.weight.overrideSprite = jokerArr[5];
            joker.gameObject.SetActive(true);
            joker.overrideSprite = jokerArr[7];
        }
        else if (weight == CardWeight.LJoker)
        {
            this.color.transform.gameObject.SetActive(false);
            this.color.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(20.0f, -45.0f);

            this.weight.overrideSprite = jokerArr[4];
            joker.gameObject.SetActive(true);
            joker.overrideSprite = jokerArr[6];
        }
        this.color.SetNativeSize();
        this.weight.SetNativeSize();
        this.joker.SetNativeSize();
    }


}
