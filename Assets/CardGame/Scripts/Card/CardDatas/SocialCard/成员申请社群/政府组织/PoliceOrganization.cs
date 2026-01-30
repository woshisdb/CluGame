using System.Collections.Generic;

/// <summary>
/// 警察局组织
/// </summary>
public class PoliceStation : Organization
{
    public static string chief = "局长";
    public static string deputyChief = "副局长";
    public static string detective = "侦探";
    public static string officer = "巡警";
    public static string forensic = "刑侦技术员";
    public static string clerk = "文书";

    public PoliceStation() : base()
    {
        this.Positions = new Dictionary<string, Position>()
        {
            { chief, new OnePosition(chief) },              // 局长（唯一）
            { deputyChief, new OnePosition(deputyChief) }, // 副局长（唯一）
            { detective, new PositionSet(detective,3) },     // 多位侦探
            { officer, new PositionSet(officer,3) },         // 多位巡警
            { forensic, new PositionSet(forensic,3) },       // 多位技术员
            { clerk, new OnePosition(clerk) }              // 文书（唯一）
        };
    }
}