using System.Collections.Generic;

/// <summary>
/// 医院组织
/// </summary>
public class Hospital : Organization
{
    public static string director = "院长";
    public static string chiefPhysician = "主治医生";
    public static string nurse = "护士";
    public static string anesthetist = "麻醉师";
    public static string emergencyDoctor = "急诊医生";
    public static string pharmacist = "药剂师";

    public Hospital() : base()
    {
        this.Positions = new Dictionary<string, Position>()
        {
            { director, new OnePosition(director) },             // 唯一岗位
            { chiefPhysician, new PositionSet(chiefPhysician,3) }, // 多名医生
            { nurse, new PositionSet(nurse,3) },
            { anesthetist, new PositionSet(anesthetist,3) },
            { emergencyDoctor, new PositionSet(emergencyDoctor,3) },
            { pharmacist, new PositionSet(pharmacist,3) }
        };
    }
}