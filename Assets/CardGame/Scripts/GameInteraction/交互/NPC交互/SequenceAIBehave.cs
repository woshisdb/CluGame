using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SequenceAISustainBehave : AISustainBehave,AIConclusion
{
    private readonly List<AISustainBehave> _children;
    private int _currentIndex = 0;

    public SequenceAISustainBehave(IEnumerable<AISustainBehave> children)
    {
        if (_children!=null)
        {
            _children = children.ToList();
        }
    }

    private AISustainBehave Current =>
        _currentIndex < _children.Count ? _children[_currentIndex] : null;

    public override bool CanStart()
    {
        return _children.Count > 0 && _children[0].CanStart();
    }

    public override async Task Start()
    {
        isStart = true;
        _currentIndex = 0;
        await Current.Start();
    }

    public override async Task<AIBehave> GetNowBehave()
    {
        if (Current == null)
            return null;
        if (!Current.isStart)
            await Current.Start();
        return  await Current.GetNowBehave();
    }

    public override bool IsComplete()
    {
        if (Current == null)
            return true;

        if (Current.IsComplete())
        {
            Current.OnComplete();
            _currentIndex++;
        }

        return _currentIndex >= _children.Count;
    }

    public override void OnBreak()
    {
        if (Current != null)
            Current.OnBreak();
    }

    public override void OnComplete()
    {
        // 整个序列完成
    }

    public override void Bind(NpcCardModel npc)
    {
        base.Bind(npc);
        foreach (var child in _children)
            child.Bind(npc);
    }

    public virtual string ConclusionAIInfo(NpcCardModel npc)
    {
        return "";
    }
}