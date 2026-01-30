using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BehavePoint : MonoBehaviour
{
    public TextMeshProUGUI textModel;
    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        var nowPlayer = GameFrameWork.Instance.playerManager.nowPlayer;
        if (nowPlayer!=null&&nowPlayer.BehavePointComponent!=null)
        {
            textModel.text = "行动点:"+nowPlayer.BehavePointComponent.pointNum+"/"+nowPlayer.BehavePointComponent.maxPointNum;
        }
    }
}
