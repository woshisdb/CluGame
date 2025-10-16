using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardLineData
{
    public string action;
    public CardEnum from;
    public CardEnum to;
    public CardLineData(CardEnum from,CardEnum to,string action)
    {
        this.from = from;
        this.to = to;
        this.action = action;
    }
}

public class CardLineMgr
{
    CardModel cardModel;
    public List<CardLineData> cardLines;
    public Dictionary<CardLineData,ObjectConnector> connectors;
    public CardLineMgr(CardModel cardModel)
    {
        this.cardModel = cardModel;
        cardLines = new List<CardLineData>();
        connectors = new Dictionary<CardLineData, ObjectConnector>();
        cardModel.InitCardLineMgr(this);
    }
    public void AddCardLine(CardLineData lineData)
    {
        cardLines.Add(lineData);
    }
}
