using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStreetSceneCard : SceneCardData
{
    public MainStreetSceneCard():base()
    {
        title = "雾森镇";
        description = "宽敞明亮";
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.FogforestTown;
    }
    public override void InitCardLineMgr(CardLineMgr cardLineMgr)
    {
        base.InitCardLineMgr(cardLineMgr);
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.FogforestTown, CardEnum.SResidentArea, "1回合"));
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.FogforestTown, CardEnum.NResidentArea, "1回合"));
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.FogforestTown, CardEnum.WResidentArea, "1回合"));
        cardLineMgr.AddCardLine(new CardLineData(CardEnum.FogforestTown, CardEnum.EResidentArea, "1回合"));
    }
}
