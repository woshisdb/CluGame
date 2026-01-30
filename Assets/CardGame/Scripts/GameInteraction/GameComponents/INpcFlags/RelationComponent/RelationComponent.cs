using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

public class RelationRecord
{
    /// <summary>
    /// 对方 NPC 的唯一 ID
    /// </summary>
    public string TargetNpcId;

    /// <summary>
    /// 我对这个 NPC 的当前态度（主观、可变）
    /// 例："我厌恶他，但仍然不得不依赖他的情报"
    /// </summary>
    public string AttitudeDescription;

    /// <summary>
    /// 我们关系的整体总结（长期记忆）
    /// 例："我们曾是盟友，但多次背叛让我不再信任他"
    /// </summary>
    public string RelationshipSummary;
}
[Serializable]

public class GptRelationResult
{
    public string AttitudeDescription;
    public string RelationshipSummary;
}


public class RelationComponent : BaseComponent
{
    /// <summary>
    /// key = 其他 NPC Id
    /// value = 当前 NPC 对其的关系认知
    /// </summary>
    public Dictionary<string, RelationRecord> Relations = new();

    public RelationComponent(CardModel cardModel, RelationComponentCreator creator)
        : base(cardModel, creator)
    {
        this.Relations = Relations;
    }

    public RelationRecord GetOrCreateRelation(string targetNpcId)
    {
        if (!Relations.TryGetValue(targetNpcId, out var record))
        {
            record = new RelationRecord
            {
                TargetNpcId = targetNpcId,
                AttitudeDescription = string.Empty,
                RelationshipSummary = string.Empty
            };
            Relations[targetNpcId] = record;
        }

        return record;
    }

    /// <summary>
    /// 用 GPT 返回的结果直接更新关系
    /// </summary>
    public void ApplyGptResult(string targetNpcId,string attitudeDescription,string relationshipSummary)
    {
        var relation = GetOrCreateRelation(targetNpcId);
        relation.AttitudeDescription = attitudeDescription;
        relation.RelationshipSummary = relationshipSummary;
    }
    [Button]
    public async void RequestRelationShip(string eventDescription,string otherId)
    {
        var tags = CardModel.GetComponent<SpiritComponent>().GetPersonalityTags();
        var strb = new StringBuilder();
        foreach (var tag in tags)
        {
            strb.Append(tag.Description);
            strb.Append(",");
        }

        var selfPersonalitySummary = strb.ToString();
        string currentAttitudeDescription = null;
        string currentRelationshipSummary = null;
        if (Relations.ContainsKey(otherId))
        {
            currentAttitudeDescription = Relations[otherId].AttitudeDescription;
            currentRelationshipSummary = Relations[otherId].RelationshipSummary;
        }
        var res = await UpdateRelationByGpt(eventDescription,selfPersonalitySummary,currentAttitudeDescription,currentRelationshipSummary);
        ApplyGptResult(otherId,res.AttitudeDescription,res.RelationshipSummary);
    }
    
    public async Task<GptRelationResult> UpdateRelationByGpt(
        string eventDescription,
        string selfPersonalitySummary,
        string currentAttitudeDescription,
        string currentRelationshipSummary
    )
    {
        var schema = GptSchemaBuilder.BuildSchema(typeof(GptRelationResult));

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你正在扮演一个克苏鲁风格世界中的NPC内心意识。
要求：
- 使用第一人称
- 表达主观判断,内容要简洁，但不能去掉重要细节
- 不允许添加额外的细节
- 不要描述事件本身"
            },

            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【我的人格概述】
{selfPersonalitySummary}

【我与该NPC当前的态度】
{(string.IsNullOrEmpty(currentAttitudeDescription) ? "尚未形成明确态度" : currentAttitudeDescription)}

【我与该NPC的关系总结】
{(string.IsNullOrEmpty(currentRelationshipSummary) ? "尚无明确关系" : currentRelationshipSummary)}

【新发生的互动事件】
{eventDescription}
请严格使用 JSON 格式返回：
{schema}。
字段约束说明：
- AttitudeDescription：
  表示我在这次互动之后，对该 NPC 的【主观态度变化】，不能引入未发生的事实。

- RelationshipSummary：
  是我与该 NPC 的【事实性关系摘要】，
  必须明确保留关键行为（例如：谋杀、背叛、献祭、欺骗），
  不允许弱化为抽象概念（如“共谋”“黑暗行为”）。"
            }
        };

        return await GameFrameWork.Instance.GptSystem
            .ChatToGPT<GptRelationResult>(messages);
    }
}

public class RelationComponentCreator : BaseComponentCreator<RelationComponent>
{
    public Dictionary<string, RelationRecord> Relations = new();
    public override ComponentType ComponentName()
    {
        return ComponentType.RelationComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new RelationComponent(cardModel, this);
    }
}
