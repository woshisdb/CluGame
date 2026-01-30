using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 隐蔽行为
/// </summary>
public class HiddenMapBehave:MapBehave
{
    public HiddenMapBehave(MapLoader map) : base(map)
    {
    }

    public override void Run()
    {
        throw new System.NotImplementedException();
    }
}