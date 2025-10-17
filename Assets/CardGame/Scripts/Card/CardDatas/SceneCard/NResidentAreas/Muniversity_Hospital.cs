using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muniversity_Hospital : SpaceCardData
{
    public Muniversity_Hospital() : base()
    {
        title = "医院";
        description = "N";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.Muniversity_Hospital;
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

public class Muniversity_HospitalModel : SpaceCardModel
{
    public Muniversity_HospitalModel(CardData cardData) : base(cardData)
    {
        
    }

}