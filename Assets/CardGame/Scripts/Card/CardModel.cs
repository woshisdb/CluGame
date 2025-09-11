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
    public CardData cardData
    {
        get {
            return GameFrameWork.Instance.gameConfig.CardMap[cardEnum];
        }
    }
    public CardEnum cardEnum;
    /// <summary>
    /// 卡牌的数据信息
    /// </summary>
    public Dictionary<string,int> cardInt;
    public Dictionary<string, bool> cardBool;
    public Dictionary<string, float> cardFloat;
    public Dictionary<string, object> cardObjects;
    public T GetObjectByKey<T>(string key) where T: class
    {
        if (cardObjects.ContainsKey(key))
        {
            return (T)cardObjects[key];
        }
        return (T)null;
    }
    public int GetIntByKey(string key)
    {
        if(cardInt.ContainsKey(key))
        {
            return cardInt[key];
        }
        return 0;
    }
    public float GetFloatByKey(string key)
    {
        if (cardFloat.ContainsKey(key))
        {
            return cardFloat[key];
        }
        return 0f;
    }
    public bool GetBoolByKey(string key)
    {
        if (cardBool.ContainsKey(key))
        {
            return cardBool[key];
        }
        return false;
    }
    public void SetDataByKey(string key,int val)
    {
        cardInt[key] = val;
    }
    public void SetDataByKey(string key, float val)
    {
        cardFloat[key] = val;
    }
    public void SetDataByKey(string key, bool val)
    {
        cardBool[key] = val;
    }
    public void SetDataByKey(string key, object val)
    {
        cardObjects[key] = val;
    }
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
    public CardModel(CardEnum cardEnum)
    {
        this.cardEnum = cardEnum;
        this.cardBool = new Dictionary<string, bool>();
        this.cardFloat = new Dictionary<string, float>();
        this.cardInt = new Dictionary<string, int>();
        this.cardObjects = new Dictionary<string, object>();
    }
    public CardModel(CardData cardData)
    {
        this.cardEnum = cardData.GetCardType();
        this.cardBool = new Dictionary<string, bool>();
        this.cardFloat = new Dictionary<string, float>();
        this.cardInt = new Dictionary<string, int>();
        this.cardObjects = new Dictionary<string, object>();
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
