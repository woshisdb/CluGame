using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// 游戏主循环
/// </summary>
public class GameLogic
{
    public IPlayer nowPlayer;
    public List<IPlayer> players;//一系列的玩家
    /// <summary>
    /// 行动顺序
    /// </summary>
    public void GetNextPlayer()
    {
        var index = players.FindIndex(e => { return e == nowPlayer;});
        if (index!=-1)//存在
        {
            
        }
    }
}