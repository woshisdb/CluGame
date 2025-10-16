using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muniversity_LabBuilding : SpaceCardData
{
    public Muniversity_LabBuilding() : base()
    {
        title = "实验室";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Muniversity_LabBuilding;
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