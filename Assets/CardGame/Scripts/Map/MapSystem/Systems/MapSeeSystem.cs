using UnityEngine;

/// <summary>
/// 对地图的观察，简单的观察
/// </summary>
public class MapSeeSystem:BaseMapSystem
{
    public Transform transform;
    public int viewRadius = 10;
    public float eyeHeight = 1.6f;
    public float cellSize = 1f;
    /// <summary>
    /// 查看对象
    /// </summary>
    /// <param name="seeMapBehave"></param>
    public void See(SeeMapBehave seeMapBehave)
    {
        var seer = seeMapBehave.fromObj;
        var to = seeMapBehave.toObj;
    }

    public void TrySee(Vector2 pos)
    {
        
    }
}