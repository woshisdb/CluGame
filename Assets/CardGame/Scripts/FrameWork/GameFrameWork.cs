using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFrameWork : MonoBehaviour
{
    public static GameFrameWork Instance { get {
            return GameObject.Find("GameFrameWork").GetComponent<GameFrameWork>();
    } }
    public GameConfig gameConfig;
    public GameObject taskPanel;
    public Transform Table;
    // Update is called once per frame
    void Awake()
    {
        
    }
    public void AddCardByCardData(CardData cardData)
    {
        var cardTemplate = gameConfig.viewDic[cardData.viewType];
        var obj = GameObject.Instantiate(cardTemplate);
        obj.transform.SetParent(Table);
    }
}
