using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MUniversity_TeachingBuilding : SpaceCardData
{
    public MUniversity_TeachingBuilding() : base()
    {
        title = "教学楼";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.MUniversity_TeachingBuilding;
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