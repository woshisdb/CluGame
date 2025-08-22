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
    public CardsManager cardsManager;
    public TaskManager taskManager;

    public ViewModelManager viewModelManager;
    void Awake()
    {
        cardsManager = new cardsManager();
        taskManager = new TaskManager();
        viewModelManager = new ViewModelManager();
    }
    public void AddCardByCardData(CardData cardData)
    {
        var cardTemplate = gameConfig.viewDic[cardData.viewType];
        var obj = GameObject.Instantiate(cardTemplate);
        var cv = obj.getComponet<CardView>();
        cv.BindMode(cardData.CreateModel());
        obj.transform.SetParent(Table);
    }

    public void AddCardByCardModel(CardModel cardModel)
    {
        var cardTemplate = gameConfig.viewDic[cardData.viewType];
        var obj = GameObject.Instantiate(cardTemplate);
        var cv = obj.getComponet<CardView>();
        cv.BindMode(cardModel);
        obj.transform.SetParent(Table);
    }
    
}
