using System.Collections.Generic;
using Sirenix.OdinInspector;
using Studio.OverOne.DragMe.Components;
using UnityEngine;

[SerializeField]
public class CardsManager
{
    /// <summary>
    /// 整个游戏的一系列的卡牌
    /// </summary>
    public List<CardModel> cardmodels { get
        {
            return GameFrameWork.Instance.gameConfig.saveData.saveFile.cards; 
        } }
    public CardsManager()
    {
    }
    /// <summary>
    /// 创建卡牌
    /// </summary>
    [Button]
    public void CreateCard(CardEnum card,Vector3 pos)
    {
        var model = GameFrameWork.Instance.gameConfig.CardMap[card].CreateModel();
        cardmodels.Add(model);
        GameFrameWork.Instance.AddCardByCardModel(model, pos);
    }
    /// <summary>
    /// 创建卡牌
    /// </summary>
    [Button]
    public void CreateCard(CardData card)
    {
        cardmodels.Add( card.CreateModel() );
    }
    /// <summary>
    /// 刷新每一帧的行为
    /// </summary>
    /// <returns></returns>
    public void Update()
    {
        foreach (var cardModel in cardmodels)
        {
            if (cardModel.NeedRefresh())
            {
                if (cardModel.hasSwitch())
                {
                    GameFrameWork.Instance.viewModelManager.RefreshView(cardModel);
                }
            }
        }
        foreach(var card in cardmodels)
        {
            var view = FindCardByEnum(card.cardEnum);
            card.tablePos = view.transform.localPosition;
        }
    }
    public void Init()
    {
        int no = 0;
        foreach(var x in cardmodels)
        {
            GameFrameWork.Instance.AddCardByCardModel(x, x.tablePos);
            //var card = GameFrameWork.Instance.gameConfig.viewDic[x.cardData.viewType];
            //var obj = GameObject.Instantiate(card);
            //obj.transform.GetComponent<DragMe>().SetOriginPos(new Vector3(no*3,1,0));
            //obj.GetComponent<CardView>().BindModel(x);
            no++;
        }
    }
    public CardView FindCardByEnum(CardEnum cardEnum)
    {
        foreach(var x in cardmodels)
        {
            if( x.cardEnum == cardEnum)
            {
                var objs = GameFrameWork.instance.viewModelManager.FindViews(x);
                foreach(var c in objs)
                {
                    if(c is CardView)
                    {
                        return (CardView)c;
                    }
                }
            }
        }
        return null;
    }
}