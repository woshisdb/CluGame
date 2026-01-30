using System.Collections.Generic;

public class FishFactory:Factory
{
    public static string boss = "公司老板";
    public static string captain = "船长";
    public static string fisherman = "渔工";
    public static string navigator = "导航员";
    public static string financeManager = "经管";
    public static string hr = "hr";
    public static string WarehouseKeeper = "仓管";
    public static string robot = "劳工";

    public FishFactory() : base()
    {
        this.Positions = new Dictionary<string, Position>()
        {
            { boss, new OnePosition(boss) },
            { captain, new PositionSet(captain,3) },
            {fisherman,new PositionSet(fisherman,3)},
            {navigator,new PositionSet(navigator,3)},
            {financeManager,new OnePosition(financeManager)},
            {hr,new OnePosition(hr)},
            {WarehouseKeeper,new OnePosition(WarehouseKeeper)},
            {robot,new PositionSet(robot,3)},
        };
    }
}