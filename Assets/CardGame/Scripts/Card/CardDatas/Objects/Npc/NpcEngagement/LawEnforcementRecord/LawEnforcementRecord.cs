public enum LawActionType
{
    Arrest,        // 逮捕某人
    Help,          // 帮助
    Interrogate,   // 审问
    Rescue         // 解救别人
}
/// <summary>
/// 执法行为
/// </summary>
public abstract class LawEnforcementRecord : SocialImpactRecord
{
    
}
/// <summary>
/// 执法行为
/// </summary>
public abstract class ArrestLawEnforcementRecord : LawEnforcementRecord
{
    
}

/// <summary>
/// 执法行为
/// </summary>
public abstract class HelpLawEnforcementRecord : LawEnforcementRecord
{
    
}

/// <summary>
/// 执法行为
/// </summary>
public abstract class InterrogateLawEnforcementRecord : LawEnforcementRecord
{
    
}

/// <summary>
/// 执法行为
/// </summary>
public abstract class RescueLawEnforcementRecord : LawEnforcementRecord
{
    
}