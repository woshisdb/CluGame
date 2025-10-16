using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Muniversity_Gym : SpaceCardData
{
    public Muniversity_Gym() : base()
    {
        title = "体育场";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Muniversity_Gym;
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