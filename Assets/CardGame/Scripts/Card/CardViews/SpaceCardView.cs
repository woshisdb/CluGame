using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpaceCardView : CardView
{
    public GameObject hereFlag;
    public override void Refresh()
    {
        base.Refresh();
        var model = (SpaceCardModel)cardModel;
        if (model.npcs!=null&&model.npcs.Count!=0)
        {
            hereFlag.SetActive(true);
        }
        else
        {
            hereFlag.SetActive(false);
        }
    }
    [Button]
    public void AddNpcByID(string id)
    {
        var npc = GameFrameWork.Instance.data.saveFile.npcs.Find(e =>
        {
            return e.npcId == id;
        });
        ((SpaceCardModel)cardModel).Enter(npc);
    }
    public void RemoveNpcByID(string id)
    {
        
    }
}
