using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHandBar : MonoBehaviour
{
    public List<CardModel> dicCards;
    public TextMeshProUGUI text;
    // Start is called before the first frame update

    public void OnTouch()
    {
        GameFrameWork.Instance.gameHandUI.SetCards(dicCards);
    }
}
