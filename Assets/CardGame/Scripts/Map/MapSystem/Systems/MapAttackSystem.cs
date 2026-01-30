public class MapAttackSystem:BaseMapSystem
{
    /// <summary>
    /// 进行攻击
    /// </summary>
    /// <param name="attackMapBehave"></param>
    public void Attack(AttackMapBehave attackMapBehave)
    {
        var attack = attackMapBehave.attack;//攻击方式
        var from = attackMapBehave.from;
        var to = attackMapBehave.to;
        if (CheckCanAttack(attackMapBehave))//尝试进行攻击
        {
            if (GameFrameWork.Instance.rollSystem.Check(10)!=null)
            {
                attackMapBehave.to.OnBeAttackSucc(attackMapBehave);
                attackMapBehave.attack.OnSuccAttack(attackMapBehave);
                attackMapBehave.from.OnAttackSucc(attackMapBehave);
            }
            else
            {
                attackMapBehave.to.OnBeAttackFail(attackMapBehave);
                attackMapBehave.attack.OnFailAttack(attackMapBehave);
                attackMapBehave.from.OnAttackFail(attackMapBehave);
            }
        }
    }

    public bool CheckCanAttack(AttackMapBehave attackMapBehave)
    {
        return true;
    }
}