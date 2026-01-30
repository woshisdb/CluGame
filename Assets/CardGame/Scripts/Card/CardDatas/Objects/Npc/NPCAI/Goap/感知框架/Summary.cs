// using System;
// using System.Collections.Generic;
//
// public interface IAISummary
// {
//     
// }
//
// [Serializable]
// public class AIInteractionSummary:IAISummary
// {
//     public string actSummary;  // 如：攻击、对话、搜刮
// }
//
// [Serializable]
// public class AIObjectSummary : IAISummary
// {
//     public string description;
// }
//
// [Serializable]
// public class AICellObjectAndBehaveSummary:IAISummary
// {
//     /// <summary>
//     /// 对象的描述
//     /// </summary>
//     public AIObjectSummary objectSummary;
//
//     /// 该对象可进行的互动
//     public List<AIInteractionSummary> interactions;
//
//     public AICellObjectAndBehaveSummary()
//     {
//         interactions = new List<AIInteractionSummary>();
//     }
// }
//
// [Serializable]
// public class AISpaceObjectAndBehaveSummary:IAISummary
// {
//     /// <summary>
//     /// 对象的描述
//     /// </summary>
//     public AIObjectSummary objectSummary;
//
//     /// 该对象可进行的互动
//     public List<AIInteractionSummary> interactions;
//
//     public AISpaceObjectAndBehaveSummary()
//     {
//         interactions = new List<AIInteractionSummary>();
//     }
// }
//
// [Serializable]
// public class AICellSummary:IAISummary
// {
//     /// AI 与该地块的关系
//     public string description;
//     /// 地块内的可感知对象
//     public List<AICellObjectAndBehaveSummary> objects;
//
//     public int distance;
//     public AICellSummary()
//     {
//         objects = new List<AICellObjectAndBehaveSummary>();
//     }
// }
//
// [Serializable]
// public class AISpaceSummary:IAISummary
// {
//     /// AI 与该地块的关系
//     public string description;
//     /// 地块内的可感知对象
//     public List<AIInteractionSummary> objects;
//
//     public int distance;
//     public AISpaceSummary()
//     {
//         objects = new List<AIInteractionSummary>();
//     }
// }