using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muniversity_ArtBuilding : SpaceCardData
{
    public Muniversity_ArtBuilding() : base()
    {
        title = "艺术楼";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Muniversity_ArtBuilding;
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