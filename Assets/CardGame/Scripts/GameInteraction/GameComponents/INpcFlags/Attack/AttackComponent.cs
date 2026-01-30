using System.Collections.Generic;
using Sirenix.OdinInspector;

public enum AttackType
{
    [LabelText("切割（锋利造成创伤）")] Cutting = 1 << 0,
    [LabelText("穿刺（锐角贯穿目标）")] Piercing = 1 << 1,
    [LabelText("冲击（钝力或爆炸冲击）")] Impact = 1 << 2,
    [LabelText("压碎（高强度挤压破坏）")] Crushing = 1 << 3,
    [LabelText("高温（灼烧、熔化）")] Thermal = 1 << 4,
    [LabelText("低温（冻结、脆化）")] Cryogenic = 1 << 5,
    [LabelText("电流（麻痹、灼烧）")] Electric = 1 << 6,
    [LabelText("酸腐（化学溶解）")] Corrosive = 1 << 7,
    [LabelText("辐射（能量粒子伤害）")] Radiation = 1 << 8,
    [LabelText("高压（压缩气体、液体冲击）")] Pressure = 1 << 9,
    [LabelText("毒素（生化侵蚀）")] Toxic = 1 << 10,
    [LabelText("精神冲击（意识层面伤害）")] Psychic = 1 << 11,
    [LabelText("重力（空间压迫）")] Gravitic = 1 << 12,
}

public interface IAttackComponent
{
    public void OnAttackSucc(AttackMapBehave attackMapBehave);
    public void OnAttackFail(AttackMapBehave attackMapBehave);
    public List<MonsterAttackCfg> GetAttackMethods();
}

public class AttackComponent:IComponent
{
    /// <summary>
    /// 攻击的卡组
    /// </summary>
    public List<MonsterAttackCfg> attackCards;
    public CardModel CardModel;
    /// <summary>
    /// 获取攻击方法
    /// </summary>
    /// <returns></returns>
    public List<MonsterAttackCfg> GetAttackMethods()
    {
        if (CardModel is IAttackComponent)
        {
            var obj = CardModel as IAttackComponent;
            return obj.GetAttackMethods();
        }

        return attackCards;
    }

    public void OnAttackSucc(AttackMapBehave attackMapBehave)
    {
        if (CardModel is IAttackComponent)
        {
            var obj = CardModel as IAttackComponent;
            obj.OnAttackSucc(attackMapBehave);
        }
        else
        {
            
        }
    }

    public void OnAttackFail(AttackMapBehave attackMapBehave)
    {
        if (CardModel is IAttackComponent)
        {
            var obj = CardModel as IAttackComponent;
            obj.OnAttackFail(attackMapBehave);
        }
        else
        {
            
        }
    }

    public CardModel GetCard()
    {
        return CardModel;
    }

    public AttackComponent(CardModel cardModel,AttackComponentCreator creator)
    {
        this.attackCards = creator.attackCards;
        this.CardModel = cardModel;
    }
}

public class AttackComponentCreator:IComponentCreator
{
    public List<MonsterAttackCfg> attackCards;
    public ComponentType ComponentName()
    {
        return ComponentType.AttackComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new AttackComponent(cardModel, this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}