/// <summary>
/// 用于把接口绑定到枚举的 Attribute
/// </summary>
[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public sealed class InterfaceBindAttribute : Attribute
{
    /// <summary>
    /// 绑定的枚举值
    /// </summary>
    public CardFlag BindType { get; }

    public InterfaceBindAttribute(CardFlag bindType)
    {
        BindType = bindType;
    }
}