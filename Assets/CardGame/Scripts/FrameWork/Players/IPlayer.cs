using System;

public enum PlayerType
{
    player,
    enemy,
}

public interface IPlayer
{
    /// <summary>
    /// 所要做的事情
    /// </summary>
    void DoAction(Action done);
}