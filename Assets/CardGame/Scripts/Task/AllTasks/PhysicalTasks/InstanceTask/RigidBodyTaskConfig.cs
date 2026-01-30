using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class RigidBodyTaskConfig
{
    /// <summary>
    /// 尝试捡起来的交互
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static TaskConfig CreatePickUpTask(Action<Dictionary<string, CardModel>> result)
    {
        return null;
    }
    /// <summary>
    /// 藏身于其中
    /// </summary>
    /// <returns></returns>
    public static TaskConfig CreateHideTask(Action<Dictionary<string, CardModel>> result)
    {
        return null;
    }
    /// <summary>
    /// 攻击任务
    /// </summary>
    /// <returns></returns>
    public static TaskConfig CreateAttackTask(Action<Dictionary<string, CardModel>> result)
    {
        return null;
    }
    /// <summary>
    /// 观察
    /// </summary>
    /// <returns></returns>
    public static TaskConfig CreateSeeTask(Action<Dictionary<string, CardModel>> result)
    {
        return null;
    }
    /// <summary>
    /// 触碰任务
    /// </summary>
    /// <returns></returns>
    public static TaskConfig CreateTouchTask(Action<Dictionary<string, CardModel>> result )
    {
        return null;
    }
}