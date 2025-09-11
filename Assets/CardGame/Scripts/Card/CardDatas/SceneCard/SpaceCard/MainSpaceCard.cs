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
}