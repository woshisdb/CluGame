using System.Collections.Generic;

/// <summary>
/// 议会组织
/// </summary>
public class Council : Organization
{
    public static string speaker = "议长";
    public static string deputySpeaker = "副议长";
    public static string councilMember = "议员";
    public static string secretary = "秘书";
    public static string auditor = "审计官";

    public Council() : base()
    {
        this.Positions = new Dictionary<string, Position>()
        {
            { speaker, new OnePosition(speaker) },                // 议长（唯一）
            { deputySpeaker, new OnePosition(deputySpeaker) },    // 副议长（唯一）
            { councilMember, new PositionSet(councilMember,3) },    // 多个议员
            { secretary, new OnePosition(secretary) },            // 秘书（唯一）
            { auditor, new PositionSet(auditor,3) }                 // 多名审计官
        };
    }
}