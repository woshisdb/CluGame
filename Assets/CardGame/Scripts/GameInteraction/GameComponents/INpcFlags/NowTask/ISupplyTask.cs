/// <summary>
/// 提供任务的类型,同一时间只能做一件
/// </summary>
public enum SupplyTaskType
{
    Free = 0,
    EatFood = 1,
    Sleep = 2,
    Play = 3,
    Study = 4,
    Chat = 5,
}
public interface ISupplyTask
{
    public SupplyTaskType TaskType { get; }
}