using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class CardModel:IModel
{
    /// <summary>
    /// 代表这个卡牌还可以用吗
    /// </summary>
    public bool enable;
    [SerializeField]
    public CardData cardData;
    public IView CreateView()
    {
        var template = GameFrameWork.Instance.gameConfig.viewDic[cardData.viewType];
        var obj = GameObject.Instantiate(template);
        var cardView = obj.GetComponent<CardView>();
        return cardView;
    }
    public bool hasFlag(CardFlag cardFlag)
    {
        return cardData.hasFlag(cardFlag);
    }
    public CardModel(CardData cardData)
    {
        this.cardData = cardData;
    }

    public bool NeedRefresh()
    {
        return cardData.needRefresh;
    }

    public bool hasSwitch()
    {
        return false;
    }
}
