using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muniversity_Library : SpaceCardData
{
    public Muniversity_Library() : base()
    {
        title = "图书馆";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Muniversity_Library;
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
