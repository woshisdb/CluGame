using System.Collections.Generic;
using UnityEngine;

public abstract class ReadInfo
{
    public abstract void Use(SkillComponent seeer);
    public abstract bool CanUse(SkillComponent seeer);
}

public class SkillReadInfo : ReadInfo
{
    public NpcSkill card;
    public int maxVal;
    public int minVal;
    public int atMost;
    public int atLeast;
    /// <summary>
    /// 添加技能
    /// </summary>
    /// <param name="seeer"></param>
    public override void Use(SkillComponent seeer)
    {
        var val = seeer.GetNowSkill(card);
        var ret = val + UnityEngine.Random.Range(minVal, maxVal);
        seeer.SetNowSkill(card, Mathf.Max(Mathf.Min(ret,atMost),val));
    }
    public override bool CanUse(SkillComponent seeer)
    {
        var val = seeer.GetNowSkill(card);
        return val>=atLeast;
    }
}


public class BookComponent:IComponent
{
    public CardModel card;
    public List<ReadInfo> ReadInfos;
    public BookComponent(CardModel card,BookComponentCreator creator)
    {
        this.card = card;
        ReadInfos = creator.ReadInfos;
    }
    public CardModel GetCard()
    {
        return card;
    }
}

public class BookComponentCreator : IComponentCreator
{
    public List<ReadInfo> ReadInfos=new List<ReadInfo>();
    public ComponentType ComponentName()
    {
        return ComponentType.BookComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new BookComponent(cardModel,this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}