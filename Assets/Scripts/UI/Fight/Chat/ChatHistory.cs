using UnityEngine;
using UnityEngine.UI;

public class ChatHistory : MonoBehaviour
{
    private Text Name;
    private Text Chat;
    private GameObject Emoji;
    private SpriteAnimation spriteAnimation;

    public void SetChatHistory(string name, string chat)
    {
        GetAttribute();

        SetActive(false);

        Name.text = $"{name}: ";
        Chat.text = chat;
    }

    public void SetEmojiHistory(string name, string emojiName)
    {
        GetAttribute();

        SetActive(true);

        Debug.Log(emojiName);
        Name.text = $"{name}: ";
        spriteAnimation.SpriteFrames = Resources.LoadAll<Sprite>($"Image/Chat/Emoji/{emojiName}");
    }

    // 激活显示
    private void SetActive(bool isEmoji)
    {
        Emoji.gameObject.SetActive(isEmoji);
        Chat.gameObject.SetActive(!isEmoji);
    }

    // 获取属性

    private void GetAttribute()
    {
        Name = transform.Find("Name").GetComponent<Text>();
        Chat = transform.Find("Chat").GetComponent<Text>();
        Emoji = transform.Find("Emoji").gameObject;
        spriteAnimation = Emoji.GetComponent<SpriteAnimation>();
    }
}