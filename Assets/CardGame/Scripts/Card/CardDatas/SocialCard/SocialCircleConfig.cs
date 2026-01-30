using System.Collections.Generic;
using Sirenix.OdinInspector;
public class SocialCircleConfig
{
    /// <summary>
    /// 社会圈层
    /// </summary>
    public List<SocialCircle> socialCircleDic;

    public SocialCircleConfig()
    {
        socialCircleDic = new List<SocialCircle>()
        {
            new EsotericOrderofDagon(),
            new BlackMarket(),
            new Council(),
            new FishFactory(),
            new PoliceStation(),
        };
    }
}