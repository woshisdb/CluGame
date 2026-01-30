using System;
/// <summary>
/// 结束玩家回合
/// </summary>
public interface IEndPlayerCircle_GameLogic
{
    void OnEndPlayer(Action done);
}