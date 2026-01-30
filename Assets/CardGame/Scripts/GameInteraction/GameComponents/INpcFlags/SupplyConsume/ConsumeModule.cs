using System;

public interface IConsume<T> where T:IComponent
{
    public ConsumeModule<T> GetConsume();
}

public class ConsumeModule<T> where T:IComponent
{
    public CardModel cardModel;
    public T comp;
    public T belong;
    public Action<T> startUse;
    public Action<T> endUse;
    public Func<T, bool> canUse;
    public ConsumeModule(CardModel cardModel,Func<T,bool> canUse,Action<T> StartUse,Action<T> EndUse)
    {
        this.cardModel = cardModel;
        comp = this.cardModel.GetComponent<T>();
        this.startUse = StartUse;
        this.endUse = EndUse;
    }

    public bool CanUse(T cmp)
    {
        if (canUse!=null)
        {
            return canUse(cmp);
        }
        else
        {
            return true;
        }
    }
    public void StartUse(T cmp)
    {
        belong = cmp;
        startUse?.Invoke(cmp);
    }

    public void EndUse()
    {
        endUse?.Invoke(belong);
        belong = default(T);
    }
}