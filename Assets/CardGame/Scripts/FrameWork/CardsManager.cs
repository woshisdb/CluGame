using System.Collections.Generic;
using Studio.OverOne.DragMe.Components;
using UnityEngine;

[SerializeField]
public class CardsManager
{
    /// <summary>
    /// 整个游戏的一系列的卡牌
    /// </summary>
    public List<CardModel> cardmodels;
    public CardsManager()
    {
        cardmodels = new List<CardModel>();
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
    }
    public void Init()
    {
        int no = 0;
        foreach(var x in cardmodels)
        {
            var card = GameFrameWork.Instance.gameConfig.viewDic[x.cardData.viewType];
            var obj = GameObject.Instantiate(card);
            obj.transform.GetComponent<DragMe>().SetOriginPos(new Vector3(no*3,1,0));
            no++;
        }
    }
}