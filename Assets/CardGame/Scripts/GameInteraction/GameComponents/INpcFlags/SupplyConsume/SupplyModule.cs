using System;
using System.Collections.Generic;

public interface ISupply<T> where T:IComponent
{
    public SupplyModule<T> GetSupply();
}

public class SupplyModule<T> where T:IComponent
{
    public CardModel cardModel;
    public List<T> res;
    public int MaxNum;
    private Func<T,bool> canUse;
    public Action<T> startUse;
    public Action<T> endUse;
    public SupplyModule(CardModel cardModel,int num,Func<T,bool> canUse,Action<T> StartUse,Action<T> EndUse)
    {
        MaxNum = num;
        res= new List<T>();
        this.cardModel = cardModel;
        this.canUse = canUse;
        this.startUse = StartUse;
        this.endUse = EndUse;
    }

    public bool CanUse(T cmp)
    {
        if (res.Count < MaxNum)
        {
            if (canUse!=null)
            {
                return canUse.Invoke(cmp);
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    public void StartUse(T cmp)
    {
        res.Add(cmp);
        startUse?.Invoke(cmp);
    }

    public void EndUse(T cmp)
    {
        res.Remove(cmp);
        endUse?.Invoke(cmp);
    }
}