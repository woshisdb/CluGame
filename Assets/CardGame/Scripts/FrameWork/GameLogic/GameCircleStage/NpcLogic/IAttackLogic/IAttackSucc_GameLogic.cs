using System;

/// <summary>
/// 当被攻击成功的时候
/// </summary>
public interface IAttackSucc_GameLogic
{
    void OnAttackSucc(Action done);
}