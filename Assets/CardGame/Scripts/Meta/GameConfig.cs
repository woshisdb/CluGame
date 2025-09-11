using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : SerializedScriptableObject
{
    public GameObject slotTemplate;
    public Dictionary<ViewType, GameObject> viewDic;
    public GameObject taskTemplate;
    public GameObject kvItemUI;
    public GameObject buttonUI;
    public GameObject tableItemUI;

    public Dictionary<CardEnum, CardData> CardMap = new Dictionary<CardEnum, CardData>()
    {
        {CardEnum.FogforestTown,new MainStreetSceneCard()},
        {CardEnum.NResidentArea,new NResidentArea() },
        {CardEnum.SResidentArea,new SResidentArea() },
        {CardEnum.WResidentArea,new WResidentArea() },
        {CardEnum.EResidentArea,new EResidentArea() },
        {CardEnum.CentralHospital,new CentralHospital() },
        {CardEnum.MUniversity,new MUniversity() },
        {CardEnum.Museum,new Museum() },
        {CardEnum.Market,new Market() },
        {CardEnum.WoodLand,new WoodLand() },
    };
    [Button]
    public void CardMapInit()
    {
        CardMap = new Dictionary<CardEnum, CardData>()
        {
            {CardEnum.FogforestTown,new MainStreetSceneCard()},
            {CardEnum.NResidentArea,new NResidentArea() },
            {CardEnum.SResidentArea,new SResidentArea() },
            {CardEnum.WResidentArea,new WResidentArea() },
            {CardEnum.EResidentArea,new EResidentArea() },
            {CardEnum.CentralHospital,new CentralHospital() },
            {CardEnum.MUniversity,new MUniversity() },
            {CardEnum.Museum,new Museum() },
            {CardEnum.Market,new Market() },
            {CardEnum.WoodLand,new WoodLand() },
        };
    }
}
