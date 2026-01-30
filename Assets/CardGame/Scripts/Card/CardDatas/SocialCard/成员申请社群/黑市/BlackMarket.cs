using System.Collections.Generic;

/// <summary>
/// 黑市集团
/// </summary>
public class BlackMarket:Organization
{
    public static string boss = "黑市老板";
    public static string overseer = "监理者";
    public static string overseerBoss = "监管者boss";//负责维护秩序
    public static string streetDoctor = "黑医";
    public static string smuggler = "走私";
    public BlackMarket() : base()
    {
        this.Positions = new Dictionary<string, Position>()
        {
            { boss, new OnePosition(boss) },
            { overseer, new PositionSet(overseer,3) },
            {overseerBoss,new OnePosition(overseerBoss)},
            {streetDoctor,new OnePosition(streetDoctor)},
            {smuggler,new OnePosition(smuggler)},
        };
    }
}