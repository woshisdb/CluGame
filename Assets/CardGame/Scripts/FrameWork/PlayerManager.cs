using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandBase
{
    
}

/// <summary>
/// 页签
/// </summary>
public class PlayerDicHand : PlayerHandBase
{
    public string id;
    public List<CardModel> dics;
}

// public class PlayerHandCard : PlayerHandBase
// {
//     public CardModel CardModel;
// }

/// <summary>
/// 当前正在进行处理的对象的管理
/// </summary>
public class PlayerManager
{
    public bool canOpt;
    /// <summary>
    /// 当前这个就是玩家
    /// </summary>
    public NpcCardModel nowPlayer
    {
        get
        {
            return GameFrameWork.Instance.data.saveFile.npcs.Find(e =>
            {
                return e.npcId == nowName;
            });
        }
    }

    public void SetPlayerCanOperation(bool flag)
    {
        canOpt = flag;
    }

    public bool GetOperation()
    {
        return canOpt;
    }
    public List<NpcCardModel> allNpc
    {
        get
        {
            return GameFrameWork.Instance.data.saveFile.npcs;
        }
    }
    public string nowName;
    public void Init()
    {
        // GameFrameWork.Instance.gameHandUI.CreateBars(GetPlayerHand());
    }
    // /// <summary>
    // /// 获取一系列手牌
    // /// </summary>
    // /// <returns></returns>
    // public List<PlayerDicHand> GetPlayerHand()
    // {
    //     // return nowPlayer.GetNpcHand();
    // }
}
