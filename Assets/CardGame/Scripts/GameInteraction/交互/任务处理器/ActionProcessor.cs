using Sirenix.OdinInspector;

/// <summary>
/// 行为处理器
/// </summary>
public enum TaskPredicate
{
    [LabelText("前往")]
    Move,

    [LabelText("获取")]
    Acquire,

    [LabelText("说服")]
    Persuade,

    [LabelText("破坏")]
    Destroy,

    [LabelText("消灭")]
    Eliminate,

    [LabelText("执行")]
    Execute,
}