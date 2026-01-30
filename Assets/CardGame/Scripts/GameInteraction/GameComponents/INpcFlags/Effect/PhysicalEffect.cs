/// <summary>
/// 物理刺激
/// </summary>
public class PhysicalEffect : IEffectState
{
    /// <summary>
    /// 影响对比例是多少
    /// </summary>
    public int EffectRate;

    public PhysicalEffect(int EffectRate)
    {
        
    }
}

/// <summary>
/// 被触碰
/// </summary>
public class TouchEffect : PhysicalEffect
{
    public TouchEffect(int EffectRate) : base(EffectRate)
    {
    }
}


/// <summary>
/// 疼痛刺激
/// </summary>
public class PainEffect : PhysicalEffect
{
    public PainEffect(int EffectRate) : base(EffectRate)
    {
    }
}

/// <summary>
/// 响声刺激
/// </summary>
public class LoudNoiseEffect : PhysicalEffect
{
    public LoudNoiseEffect(int EffectRate) : base(EffectRate)
    {
    }
}


/// <summary>
/// 震动
/// </summary>
public class VibrationEffect : PhysicalEffect
{
    public VibrationEffect(int EffectRate) : base(EffectRate)
    {
    }
}

/// <summary>
/// 温度变化
/// </summary>
public class TemperatureChangeEffect : PhysicalEffect
{
    public TemperatureChangeEffect(int EffectRate) : base(EffectRate)
    {
    }
}

/// <summary>
/// 强光照射
/// </summary>
public class StrongLightEffect : PhysicalEffect
{
    public StrongLightEffect(int EffectRate) : base(EffectRate)
    {
    }
}

/// <summary>
/// 空气变化（缺氧、烟雾等）
/// </summary>
public class AirQualityEffect : PhysicalEffect
{
    public AirQualityEffect(int EffectRate) : base(EffectRate)
    {
    }
}


