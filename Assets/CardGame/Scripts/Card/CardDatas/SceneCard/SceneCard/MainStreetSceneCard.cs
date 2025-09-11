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
}
