using System;
using Sirenix.OdinInspector;

public class EnvirCircleLogicModule:SerializedMonoBehaviour
{
    /// <summary>
    /// 当前执行的事情
    /// </summary>
    /// <param name="done"></param>
    public void RunAction(Action done)
    {
        done?.Invoke();
    }
    /// <summary>
    /// 结束后添加时间
    /// </summary>
    public void AfterCircle(Action done)
    {
        GameFrameWork.Instance.GameTimeManager.UpdateTimeNode();
        GameFrameWork.Instance.GameTimeManager.Advance(1);//时间推进
        done?.Invoke();
    }
}