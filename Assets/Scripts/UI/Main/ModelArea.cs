using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelArea : UIBase
{
    private Button classifyModel;

    private void Awake()
    {
        classifyModel = transform.Find("Area/Classic").GetComponent<Button>();

        classifyModel.onClick.AddListener(OnClickClassifyModel);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        classifyModel.onClick.RemoveAllListeners();
    }

    private void OnClickClassifyModel()
    {
        LoadSceneMsg loadSceneMsg = new LoadSceneMsg(2, () =>
          {
          });
        Dispatch(AreaCode.SCENCE, SceneEvent.Load_Scence, loadSceneMsg);
    }
}