using System;

/// <summary>
/// 攻击失败时事件
/// </summary>
public interface IAttackFail_GameLogic
{
    void OnAttackFail(Action done);
}