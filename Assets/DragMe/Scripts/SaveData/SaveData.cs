using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class SaveFile
{
    public List<CardModel> cards;
    public List<TaskPanelModel> tasks;
    /// <summary>
    /// 工作的职业
    /// </summary>
    public List<JobCardModel> jobs;
    /// <summary>
    /// npc角色
    /// </summary>
    public List<NpcCardModel> npcs;
}

[CreateAssetMenu(fileName = "newSaveData", menuName = "SaveData/newSaveData")]
public class SaveData : SerializedScriptableObject
{
    public SaveFile saveFile;
    [Button]
    public void Save()
    {
        var filePath = "Assets/Resources/Saves" + "/tablesaveData.dat";
        Debug.Log(filePath);
        var json = SerializationUtility.SerializeValue(saveFile, DataFormat.JSON);
        File.WriteAllBytes(filePath, json);
        Debug.Log("Data Saved: " + json);
    }
    [Button]
    public void Load()
    {
        var filePath = "Assets/Resources/Saves" + "/tablesaveData.dat";
        var json = File.ReadAllBytes(filePath);
        saveFile = SerializationUtility.DeserializeValue<SaveFile>(json, DataFormat.JSON);
    }
    /// <summary>
    /// 创建卡牌
    /// </summary>
    [Button]
    public void CreateCard(CardEnum card)
    {
        saveFile.cards.Add(GameFrameWork.Instance.gameConfig.CardMap[card].CreateModel());
    }
    /// <summary>
    /// 创建卡牌
    /// </summary>
    [Button]
    public void CreateCard(CardData card)
    {
        saveFile.cards.Add(card.CreateModel());
    }
    [Button]
    public void CreateJobRecord(JobInfo jobInfo)
    {
    }
}
