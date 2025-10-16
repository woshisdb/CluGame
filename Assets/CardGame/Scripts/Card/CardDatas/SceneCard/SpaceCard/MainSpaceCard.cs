using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NResidentArea : SpaceCardData
{
    public NResidentArea() : base()
    {
        title = "北方居民区";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.NResidentArea;
    }
    public override bool CanGo()
    {
        return true;
    }
    public override SpaceType GetSpace()
    {
        return SpaceType.space1;
    }
}

public class SResidentArea : SpaceCardData
{
    public SResidentArea() : base()
    {
        title = "南方居民区";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.SResidentArea;
    }
}

public class WResidentArea : SpaceCardData
{
    public WResidentArea() : base()
    {
        title = "西方居民区";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.WResidentArea;
    }
}

public class EResidentArea : SpaceCardData
{
    public EResidentArea() : base()
    {
        title = "东方居民区";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.EResidentArea;
    }
}

public class CentralHospital : SpaceCardData
{
    public CentralHospital() : base()
    {
        title = "中央医院";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.CentralHospital;
    }
    public override void InitCardLineMgr(CardLineMgr cardLineMgr)
    {
        base.InitCardLineMgr(cardLineMgr);
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.CentralHospital, CardEnum.WResidentArea, "1回合"));
    }
}

public class MUniversity : SpaceCardData
{
    public MUniversity() : base()
    {
        title = "本地大学";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.MUniversity;
    }
    public override void InitCardLineMgr(CardLineMgr cardLineMgr)
    {
        base.InitCardLineMgr(cardLineMgr);
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.MUniversity, CardEnum.NResidentArea, "1回合"));
    }
}

public class Museum : SpaceCardData
{
    public Museum() : base()
    {
        title = "博物馆";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Museum;
    }
    public override void InitCardLineMgr(CardLineMgr cardLineMgr)
    {
        base.InitCardLineMgr(cardLineMgr);
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.Museum, CardEnum.NResidentArea, "1回合"));
    }
}

public class Market : SpaceCardData
{
    public Market() : base()
    {
        title = "市场";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Market;
    }
    public override void InitCardLineMgr(CardLineMgr cardLineMgr)
    {
        base.InitCardLineMgr(cardLineMgr);
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.Market, CardEnum.SResidentArea, "1回合"));
    }
}

public class WoodLand : SpaceCardData
{
    public WoodLand() : base()
    {
        title = "古墓林场";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.WoodLand;
    }
    public override void InitCardLineMgr(CardLineMgr cardLineMgr)
    {
        base.InitCardLineMgr(cardLineMgr);
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.WoodLand, CardEnum.EResidentArea, "1回合"));
    }
}

public class Pool : SpaceCardData
{
    public Pool() : base()
    {
        title = "水潭渔场";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Pool;
    }
    public override void InitCardLineMgr(CardLineMgr cardLineMgr)
    {
        base.InitCardLineMgr(cardLineMgr);
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.Pool, CardEnum.EResidentArea, "1回合"));
    }
}