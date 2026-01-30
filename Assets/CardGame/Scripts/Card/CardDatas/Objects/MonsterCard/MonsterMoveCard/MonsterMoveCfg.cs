using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "怪物移动方式配置", menuName = "怪物/移动配置")]
public class MonsterMoveCfg:SerializedScriptableObject
{
    public MovementForm MovementForm;//移动方式
    public int maxDistance;
    public int minDistance;
    /// <summary>
    /// 移动成功
    /// </summary>
    public void OnMoveSucc()
    {
        
    }
    /// <summary>
    /// 开始移动
    /// </summary>
    public void OnStartMove()
    {
        
    }
    /// <summary>
    /// 当移动失败
    /// </summary>
    public void OnMoveFail()
    {
        
    }
}