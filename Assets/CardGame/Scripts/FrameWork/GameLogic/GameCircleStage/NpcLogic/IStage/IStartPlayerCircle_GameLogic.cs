using System;

/// <summary>
/// 开始一个玩家循环
/// </summary>
public interface IStartPlayerCircle_GameLogic
{
    void OnStartPlayer(Action done);
    bool CanJoin(MapLoader mapLoader);
}