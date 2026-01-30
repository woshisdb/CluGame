using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// AI的决策模块，里面需要实现对AI行为的描述
/// </summary>
public class AIDecision
{
    public AIBehave behave;
    public AIDecision(AIBehave behave)
    {
        this.behave = behave;
    }
    /// <summary>
    /// 执行当前行为
    /// </summary>
    public async Task Exe(AIBehave behave = null)//结束就返回true
    {
        if (behave == null)
        {
            behave = this.behave;
        }
        if (behave.isEnd)//如果是最后一个行为，那直接结束
        {
            if (behave.GetCanSelectCards!=null)
            {
                // var select = await RandomOne(behave.GetCanSelectCards(behave.retData));
                var cards = behave.GetCanSelectCards(behave.retData);
                List<string> selectStrs = new List<string>();
                foreach (var x in cards)
                {
                    selectStrs.Add(behave.SelectDescriptionFunc(x));
                }
                var message = GetMessage(behave.DescriptionFunc(),selectStrs);
                var val = await WaitGTPResponse(message);
                behave.WhenSelect(cards[val]);
                behave.EndAction();
                return;
            }
            else
            {
                behave.EndAction();
                return;
            }
        }
        else
        {
            // var select = await RandomOne(behave.GetCanSelectCards(behave.retData));
            var cards = behave.GetCanSelectCards(behave.retData);
            List<string> selectStrs = new List<string>();
            foreach (var x in cards)
            {
                selectStrs.Add(behave.SelectDescriptionFunc(x));
            }
            var message = GetMessage(behave.DescriptionFunc(),selectStrs);
            var val = await WaitGTPResponse(message);
            var newBehave = behave.WhenSelect(cards[val]);
            await Exe(newBehave);
        }
    }

    public QwenChatMessage GetMessage(string Description,List<string> strs)
    {
        var message = new QwenChatMessage();
        message.role = "system";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Description+"。你是一个严格的决策引擎。\n\n请从以下选项中【只选择一个】。\n【只允许返回一个阿拉伯数字】。\n【不要返回任何解释、文字、符号或标点】。\n否则结果视为无效。");
        for (int i = 0; i < strs.Count; i++)
        {
            stringBuilder.Append("["+i+"]:"+strs[i]+"。\n");
        }
        message.content = stringBuilder.ToString();
        return message;
    }

    public async Task<int> WaitGTPResponse(QwenChatMessage gpt)
    {
        var ret = await GameFrameWork.Instance.GptSystem.ChatToGPT<int>(new List<QwenChatMessage>(){gpt});
        return ret;
    }
    public async Task<CardModel> RandomOne(List<CardModel> list)
    {
        if (list == null || list.Count == 0)
            return null;

        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }
    
}