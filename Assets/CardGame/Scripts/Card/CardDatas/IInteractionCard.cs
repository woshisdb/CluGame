using System.Collections.Generic;

/// <summary>
/// 可交互的卡片
/// </summary>
public interface IInteractionCard
{
    /// <summary>
    /// 所需要的交互
    /// </summary>
    /// <param name="inputs"></param>
    void Interaction(Dictionary<string,CardModel> inputs,TaskPanelModel task);
}