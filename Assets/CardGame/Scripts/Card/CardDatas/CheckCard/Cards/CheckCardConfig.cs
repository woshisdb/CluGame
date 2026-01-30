using System;
using System.Collections.Generic;

public class CheckCardConfig
{
    public string id;
    public string title;
    public string description;
    public CheckState CheckState;

    public virtual void OnEffect(NpcCardModel npc,Action done)
    {
        done?.Invoke();
        return;
    }
}

public static class GlobalCheckCardConfig
{
    public static Dictionary<CardEnum,Dictionary<string, CheckCardConfig>> CheckCfgs = new Dictionary<CardEnum,Dictionary<string, CheckCardConfig>>()
    {
        // { CardEnum.climb ,new Dictionary<string, CheckCardConfig>()
        // {
        //     {"攀爬1",new CheckCardConfig()
        //     {
        //         id = "攀爬1",
        //         title = "攀爬",
        //         description = "攀爬成功",
        //         CheckState = CheckState.Succ
        //     }}
        // }}
    };

    public static CheckCardConfig GetConfig(CardEnum cardEnum, string id)
    {
        return CheckCfgs[cardEnum][id];
    }
}