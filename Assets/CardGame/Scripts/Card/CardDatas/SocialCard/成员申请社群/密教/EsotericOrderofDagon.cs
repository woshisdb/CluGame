using System.Collections.Generic;

/// <summary>
/// 大公
/// </summary>
public class EsotericOrderofDagon:OccultCultOrganization
{
    public static string highPriest = "大祭司";
    public static string devotedOnes = "信仰者";
    public static string hybrids = "秩序维护者";

    public EsotericOrderofDagon() : base()
    {
        this.Positions = new Dictionary<string, Position>()
        {
            { highPriest, new OnePosition(highPriest) },
            { devotedOnes, new PositionSet(devotedOnes,3) },
            {hybrids,new PositionSet(hybrids,3)}
        };
    }
}