using System;

/// <summary>
/// 攻击结束时的检测
/// </summary>
public interface IEndAttack_GameLogic
{
    void OnEndAttack(Action done);
}