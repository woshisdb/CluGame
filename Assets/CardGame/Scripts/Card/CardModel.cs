using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public string title;
    public string description;
    public ViewType viewType;
    public HashSet<CardFlag> cardFlags;
    public CardEnum cardEnum;
    public CardData()
    {
        cardFlags = new HashSet<CardFlag>();
        cardEnum = new CardEnum();
    }
}

public class CardModel:IModel
{
    public CardData cardData;
    public IView CreateView()
    {
        var template = GameFrameWork.Instance.gameConfig.viewDic[cardData.viewType];
        var obj = GameObject.Instantiate(template);
        var cardView = obj.GetComponent<CardView>();
        return cardView;
    }
}
