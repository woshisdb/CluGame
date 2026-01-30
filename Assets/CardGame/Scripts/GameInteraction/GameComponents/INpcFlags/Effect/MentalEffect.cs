/// <summary>
/// 心理刺激
/// </summary>
public class MentalEffect : IEffectState
{
    /// <summary>
    /// 影响对比例是多少
    /// </summary>
    public int EffectRate;
    public MentalEffect(int EffectRate)
    {
        
    }
}

/// <summary>
/// 恐惧刺激
/// </summary>
public class FearEffect : MentalEffect
{

    public FearEffect(int rate):base(rate)
    {
    }
}
/// <summary>
/// 焦虑、不安
/// </summary>
public class AnxietyEffect : MentalEffect
{
    public AnxietyEffect(int rate):base(rate)
    {
    }
}
