using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行动
/// </summary>
public class RunMapBehave:MapBehave
{
    public NpcCardModel npc;
    public CellView start;
    public CellView aim;//地图的目标位置
    public MovementForm movementForm;
    public RunMapBehave(NpcCardModel npc,MapLoader map):base(map)
    {
        this.npc = npc;
    }

    public override void Run()
    {
        map.mapMoveSystem.Move(this);
    }
}