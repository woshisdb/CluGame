// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// /// <summary>
// /// 工作的效果
// /// </summary>
// public interface IJobEffect
// {
//     
// }
//
// /// <summary>
// /// 具体工作的信息，例如普通医生，院长
// /// </summary>
// public class JobCardData: CardData
// {
//     /// <summary>
//     /// 工作的效果
//     /// </summary>
//     public List<IJobEffect> effects;
//
//     public JobCardData() : base()
//     {
//         viewType = ViewType.JobCard;
//     }
//     public override CardEnum GetCardType()
//     {
//         return CardEnum.JobCard;
//     }
//
//     public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
//     {
//         throw new System.NotImplementedException();
//     }
// }
//
// /// <summary>
// /// 工作
// /// </summary>
// public class JobCardModel : CardModel
// {
// }
