using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡片的符号
/// </summary>
public enum CardFlag
{
}

/// <summary>
/// 卡牌的枚举类，定义CoC跑团中各类卡牌类型
/// </summary>
public enum CardEnum
{
//-----------------------------书籍
//-----------------------------技能
    skill=2,
    npc=3,
    //--------------------------空间卡
    SpaceCard=4,
    JobRecord=5,
    JobCard=6,
    //--------------------------对象
    ObjectCard=7,
    //--------------------------怪物
    MonsterCard=9,
    //--------------------------目标
    TargetCard=10,
    //--------------------------单元
    CellCard=11,
    //------------------------------怪物相关的卡
    MonsterAttack=12,
    MonsterMove=13,
    MonsterReproduct=14,
    MonsterWeak=15,
    //--------------------------投骰子监测卡
    Roll=16,
    Check=17,
    cell=19,
    MonsterBody=20,
    MonsterEffect = 21,
    Interaction=22,//交互
    Monster = 23,
    FurnitureCard = 24,
    BuildingCard = 25,
}