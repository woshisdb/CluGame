using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardLineMgr
{
    CardModel cardModel;
    private CardView CardView;
    
    public Dictionary<CardLineData,ObjectConnector> connectors;
    public CardLineMgr(CardModel cardModel,CardView CardView)
    {
        this.CardView = CardView;
        this.cardModel = cardModel;
        connectors = new Dictionary<CardLineData, ObjectConnector>();
    }

    public bool IsLineEnable()
    {
        if (cardModel!=null&&cardModel.IsSatComponent<DrawLineComponent>())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<CardLineData> GetCardLineData()
    {
        return cardModel.GetComponent<DrawLineComponent>().CardLineDatas;
    }
    public void CreateLine(CardLineData data)
    {
        var fromObj = GameFrameWork.instance.cardsManager.FindCardByCfg(data.from);
        var toObj = GameFrameWork.instance.cardsManager.FindCardByCfg(data.to);
        if(fromObj && toObj)
        {
            var lineTempate = GameFrameWork.Instance.gameConfig.lineTemplate;
            var obj = GameObject.Instantiate(lineTempate);
            var cmp = obj.GetComponent<ObjectConnector>();
            cmp.Bind(fromObj.transform, toObj.transform, data.action);
            cmp.transform.SetParent(CardView.transform);
            connectors[data] = cmp;
        }
    }
}
