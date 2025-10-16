using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muniversity_Dorm : SpaceCardData
{
    public Muniversity_Dorm() : base()
    {
        title = "宿舍楼";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Muniversity_Dorm;
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