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
    // [Button]
    // public void CreateNpcCard(string name,Vector3 pos)
    // {
    //     var data = GameFrameWork.Instance.gameConfig.CardMap[CardEnum.npc] as NpcCardData;
    //     var obj = data.CreateModelByNpc(name);
    //     cardmodels.Add(obj);
    //     GameFrameWork.Instance.gameConfig.saveData.saveFile.npcs.Add((NpcCardModel)obj);///某个地点的某个职业
    //     GameFrameWork.Instance.AddCardByCardModel(obj, pos);
    // }
    // [Button]
    // public void CreateJobCard(IHaveJob provider,string jobId,Vector3 pos)
    // {
    //     var obj = GameFrameWork.Instance.gameConfig.JobCardDatas[jobId].CreateSpecialJob(provider);
    //     cardmodels.Add(obj);
    //     GameFrameWork.Instance.gameConfig.saveData.saveFile.jobs.Add((JobCardModel)obj);///某个地点的某个职业
    //     GameFrameWork.Instance.AddCardByCardModel(obj, pos);
    // }
    /// <summary>
    /// 创建卡牌
    /// </summary>
    [Button]
    public void CreateCard(CardCreateInfo CardCreateInfo,Vector3 pos)
    {
        var model = CardCreateInfo.GetCardData().CreateModel(CardCreateInfo);
        cardmodels.Add(model);
        GameFrameWork.Instance.AddCardByCardModel(model, pos);
    }

    public CardModel JustCreateCard(CardCreateInfo CardCreateInfo)
    {
        return CardCreateInfo.GetCardData().CreateModel(CardCreateInfo);
    }
    // /// <summary>
    // /// 创建卡牌
    // /// </summary>
    // [Button]
    // public void CreateCard(CardData card,CardCreateInfo CardCreateInfo)
    // {
    //     cardmodels.Add( card.CreateModel(CardCreateInfo) );
    // }
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
            // var view = FindCardByEnum(card.cardEnum);
            var views = GameFrameWork.Instance.viewModelManager.FindViews(card);
            if (views!=null && views.Count>0)
            {
                card.tablePos = ((CardView)(views[0])).transform.localPosition;
            }
        }
    }
    public void Init()
    {
        foreach(var x in cardmodels)
        {
            if (x.needShowCard)
            {
                GameFrameWork.Instance.AddCardByCardModel(x, x.tablePos);
            }
            //var card = GameFrameWork.Instance.gameConfig.viewDic[x.cardData.viewType];
            //var obj = GameObject.Instantiate(card);
            //obj.transform.GetComponent<DragMe>().SetOriginPos(new Vector3(no*3,1,0));
            //obj.GetComponent<CardView>().BindModel(x);
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
    
    public CardView FindCardByCfg(BaseCardConfig cfg)
    {
        foreach(var x in cardmodels)
        {
            if(x.cfg!=null && x.cfg.Value == cfg)
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

    // public CardModel FindCardModelByEnum(CardEnum cardEnum)
    // {
    //     foreach (var x in cardmodels)
    //     {
    //         if (x.cardEnum == cardEnum)
    //         {
    //             return x;
    //         }
    //     }
    //
    //     return null;
    // }
}