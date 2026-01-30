using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PassStageAnim : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void ShowAnimation(string name,Action callback)
    {
        text.text = name;
        transform.localPosition = new Vector3(-1929f,0f, 0f);
        transform.DOMoveX(961.4872f,0.3f).OnComplete(() => {
            // 渐隐完成后可隐藏对象（可选）
            transform.localPosition = new Vector3(-1929f,0f, 0f);
            callback?.Invoke();
        });
    }
}
