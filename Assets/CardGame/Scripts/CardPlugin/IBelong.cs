using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBelong:INpcMgr,IRegisterID
{
    void Enter(NpcCardModel npc);
    void Exit(NpcCardModel npc);
    
    Transform GetTransform();
}
