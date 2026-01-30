using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCardView : CardView
{
    public override void Refresh()
    {
        base.Refresh();
        if (cardModel!=null)
        {
            var jb = cardModel as NpcCardModel;
            var str = "";
            if (jb!=null)
            {
                foreach (var vb in jb.NpcPhyInfo.relationship)
                {
                    str+= vb.Key.ToString()+":"+vb.Value+"\n";
                }
                this.description.text = str;
            }
        }
    }
}
