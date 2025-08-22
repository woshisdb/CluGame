using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameFrameWork : SerializedMonoBehaviour
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
        cardsManager.Init();
    }
    public void AddCardByCardData(CardData cardData)
    {
        var cardTemplate = gameConfig.viewDic[cardData.viewType];
        var obj = GameObject.Instantiate(cardTemplate);
        var cv = obj.GetComponent<CardView>();
        cv.BindModel(cardData.CreateModel());
        obj.transform.SetParent(Table);
    }

    public void AddCardByCardModel(CardModel cardModel)
    {
        var cardTemplate = gameConfig.viewDic[cardModel.cardData.viewType];
        var obj = GameObject.Instantiate(cardTemplate);
        var cv = obj.GetComponent<CardView>();
        cv.BindModel(cardModel);
        obj.transform.SetParent(Table);
    }
    public void Update()
    {
        cardsManager.Update();
        taskManager.Update();
    }
}
