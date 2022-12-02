using UnityEngine.UI;

public class FightBottomArea : UIBase
{
    private Button chatBtn;
    private bool defaultChatPanelState = false;

    private void Awake()
    {
        chatBtn = transform.Find("ChatBox").GetComponent<Button>();
        chatBtn.onClick.AddListener(OnClickChat);
    }

    private void OnClickChat()
    {
        Dispatch(AreaCode.UI, UIEvent.Chat_Panel_Active, !defaultChatPanelState);
        defaultChatPanelState = !defaultChatPanelState;
    }
}