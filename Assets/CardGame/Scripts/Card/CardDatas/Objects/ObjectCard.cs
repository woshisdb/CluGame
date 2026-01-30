using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCardCreateInfo : CardCreateInfo
{
    public ObjectCardConfig objectCfg;
    public CardEnum Belong()
    {
        return CardEnum.ObjectCard;
    }
}
public class ObjectCardData : CardData
{
    public ObjectCardData() :base()
    {
        this.viewType = ViewType.ObjectCard;
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.ObjectCard;
    }

    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        var ct = CardCreateInfo as ObjectCardCreateInfo;
        return new ObjectCardModel(this,ct.objectCfg);
    }

    public override CardModel CreateCardModelByInfo(CardCreateInfo data)
    {
        return CreateModel(data);
    }
}

public class ObjectCardModel : CardModel,IObjectCard
{
    public ObjectCardModel(CardData cardData,BaseCardConfig cfg) : base(cardData,cfg)
    {
        
    }
    public override string GetDescription()
    {
        if (IsSatComponent<DescriptionComponent>())
        {
            return GetComponent<DescriptionComponent>().description;
        }

        return base.GetDescription();
    }

    public override string GetTitle()
    {
        if (IsSatComponent<DescriptionComponent>())
        {
            return GetComponent<DescriptionComponent>().title;
        }
        else
        {
            return base.GetTitle();
        }
    }
}