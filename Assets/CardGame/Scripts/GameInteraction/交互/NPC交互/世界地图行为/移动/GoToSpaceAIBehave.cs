using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 前往指定AI指定的位置
/// </summary>
public class GoToSpaceAIBehave:AISustainBehave
{
    public List<ISearchNode> Path;
    public override Task<AIBehave> GetNowBehave()
    {
        var aimSpace = npc.GetComponent<AIMindComponent>().aimSpace;
        var nowSpace = npc.GetComponent<BelongComponent>().belong.Value as SpaceCardModel;
        if (aimSpace == nowSpace)
        {
            return Task.FromResult<AIBehave>(null);
        }
        else
        {
            var nextSpace = Path[0] as SpaceCardModel;
            GameFrameWork.Instance.GoToSpace(new GoToSpaceCardDelegate(
                nextSpace,nowSpace.WasterTime(nextSpace),npc.npcId
            ));
            Path.RemoveAt(0);
            return Task.FromResult<AIBehave>(null);
        }
    }

    public override bool IsComplete()
    {
        return Path.Count == 0;
    }

    public override void OnComplete()
    {
    }
    /// <summary>
    /// 中断
    /// </summary>
    public override void OnBreak()
    {
    }

    public override bool CanStart()
    {
        var aimSpace = npc.GetComponent<AIMindComponent>().aimSpace;
        var nowSpace = npc.GetComponent<BelongComponent>().belong.Value as SpaceCardModel;
        var result = SearchAlgorithm.Search(nowSpace,aimSpace);
        if (result == null)
        {
            BreakMySelf("没找到路径");
            return false;
        }
        else
        {
            Path = result.Path;
            return true;
        }
    }

    public override Task Start()
    {
        return Task.CompletedTask;
    }
}