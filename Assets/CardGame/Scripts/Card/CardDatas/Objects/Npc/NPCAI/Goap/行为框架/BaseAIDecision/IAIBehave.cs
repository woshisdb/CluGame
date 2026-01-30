using System;
using System.Collections.Generic;

/// <summary>
/// AI行为配置
/// </summary>
public class AIBehave
{
    /// <summary>
    /// 获取下一个执行函数
    /// </summary>
    public Func<Dictionary<string, CardModel>,List<CardModel>> GetCanSelectCards;
    /// <summary>
    /// 是否选择结束
    /// </summary>
    protected Func<CardModel,(string,CardModel)> OnSelect;

    public Dictionary<Func<CardModel,bool>, AIBehave> nextDics;
    public bool isEnd;
    public Action<Dictionary<string, CardModel>> endAction;
    
    public Func<CardModel,string> SelectDescriptionFunc;
    public Func<string> DescriptionFunc;
    public Dictionary<string, CardModel> retData;
    public AIBehave(Func<Dictionary<string, CardModel>,List<CardModel>> GetCanSelectCards,Func<CardModel,(string,CardModel)> onSelect,bool isEnd,Func<CardModel,string> SelectDescriptionFunc,Func<string> DescriptionFunc)
    {
        nextDics = new Dictionary<Func<CardModel,bool>, AIBehave>();
        this.GetCanSelectCards = GetCanSelectCards;
        this.OnSelect = onSelect;
        this.isEnd = isEnd;
        this.SelectDescriptionFunc = SelectDescriptionFunc;
        this.DescriptionFunc = DescriptionFunc;
    }

    public bool IsEnd()
    {
        return isEnd;
    }

    public void EndAction()
    {
        endAction?.Invoke(retData);
    }
    public AIBehave SetRetData(Dictionary<string, CardModel> retData)
    {
        this.retData = retData;
        return this;
    }
    
    public AIBehave WhenSelect(CardModel cardModel)
    {
        var ret = OnSelect(cardModel);
        retData[ret.Item1] = ret.Item2;
        foreach (var x in nextDics)
        {
            if (x.Key(cardModel))
            {
                var succModel = x.Value;
                succModel.SetRetData(retData);
                // succModel.SetEndAction(endAction);
                return succModel;
            }
        }
        return null;
    }

    public Dictionary<string, CardModel> GetRet()
    {
        return retData;
    }

    public AIBehave SetEndAction(Action<Dictionary<string, CardModel>> endAction)
    {
        this.endAction = endAction;
        return this;
    }

    public AIBehave When(Func<CardModel,bool> isSat,AIBehave aiBehave)
    {
        nextDics[isSat] = aiBehave;
        return this;
    }
}